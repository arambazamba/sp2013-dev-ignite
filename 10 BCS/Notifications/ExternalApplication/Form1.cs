using System;
using System.Xml;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiniCRM"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                //Add new record
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO MiniCRM.Names (Title, FirstName, MiddleName, LastName, Suffix, EMailAddress, Phone) ");
                sql.Append("VALUES ('");
                sql.Append(title.Text);
                sql.Append("','");
                sql.Append(firstName.Text);
                sql.Append("','");
                sql.Append(middleName.Text);
                sql.Append("','");
                sql.Append(lastName.Text);
                sql.Append("','");
                sql.Append(suffix.Text);
                sql.Append("','");
                sql.Append(email.Text);
                sql.Append("','");
                sql.Append(phone.Text);
                sql.Append("')");

                SqlCommand updateCommand = new SqlCommand(sql.ToString(), connection);
                updateCommand.ExecuteNonQuery();

                //Get the ID for the new record
                SqlCommand idCommand = new SqlCommand(
                    "SELECT ID FROM MiniCRM.Names WHERE LastName='" + lastName.Text + "' AND FirstName='" + firstName.Text + "'",
                    connection);
                object newId = idCommand.ExecuteScalar();

                //Get all endpoints
                SqlCommand deliveryCommand = 
                    new SqlCommand(
                        "SELECT DeliveryAddress, EventType FROM Subscriptions",
                        connection);

                SqlDataReader deliveryReader = deliveryCommand.ExecuteReader();

                if (deliveryReader.HasRows)
                {
                    while (deliveryReader.Read())
                    {
                        //SharePoint Notification Endpoint
                        //"1" is item added, which is all this application does
                        if (deliveryReader.GetString(0).Contains("ProcessExternalNotification") &&
                            deliveryReader.GetString(1) == "1")
                        {
                            //Get a Digest so we can call back into SharePoint
                            string digest = GenerateDigest();

                            //Build the request against the "Item Added" endpoint
                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(deliveryReader.GetString(0));
                            request.Credentials = CredentialCache.DefaultCredentials;
                            request.Method = "POST";
                            request.Headers["X-RequestDigest"] = digest;
                            request.Accept = "application/atom+xml";
                            byte[] bodyBytes = Encoding.UTF8.GetBytes(NotifyBody("ID",newId.ToString()));
                            request.ContentLength = bodyBytes.Length;
                            request.ContentType = "application/atom+xml";

                            Stream requestStream = request.GetRequestStream();
                            requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                            requestStream.Flush();

                            using (var response = (HttpWebResponse)request.GetResponse())
                            {
                                if (response.StatusCode != HttpStatusCode.OK)
                                    MessageBox.Show(response.StatusDescription);
                            }
                        }

                        //Custom Notification Endpoint
                        if (deliveryReader.GetString(0).Contains("NotificationEndpoint.svc"))
                        {
                            try
                            {
                                EndpointAddress endpoint = new EndpointAddress(deliveryReader.GetString(0));
                                BasicHttpBinding binding = new BasicHttpBinding();
                                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

                                MiniCRMNotifications.NotificationEndpointClient client =
                                    new MiniCRMNotifications.NotificationEndpointClient(binding, endpoint);
                                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                                client.ChannelFactory.Credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

                                client.Notify("Item Added", "ID: " + newId.ToString() + ", Last Name: " + lastName.Text);
                            }
                            catch (Exception x)
                            {
                                MessageBox.Show(x.Message);
                            }
                        }

                    }
                }

                connection.Close();
                MessageBox.Show("ID: " + newId.ToString() + ", Last Name: " + lastName.Text, "Item Added");
                ClearFields();
            }

        }

        private void clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            title.Text = string.Empty;
            firstName.Text = string.Empty;
            middleName.Text = string.Empty;
            lastName.Text = string.Empty;
            suffix.Text = string.Empty;
            email.Text = string.Empty;
            phone.Text = string.Empty;
        }

        private string GenerateDigest()
        {
            string digest = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://intranet.contoso.com/sites/bcs/_vti_bin/sites.asmx");
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Method = "POST";
            request.Headers["SOAPAction"] = "http://schemas.microsoft.com/sharepoint/soap/GetUpdatedFormDigestInformation";
            request.ContentType = "text/xml";
            request.Accept = "*";

            string body = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n  <soap:Body>\r\n    <GetUpdatedFormDigestInformation xmlns=\"http://schemas.microsoft.com/sharepoint/soap/\" />\r\n  </soap:Body>\r\n</soap:Envelope>";
            try
            {
                using (TextWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(body);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    string message = null;
                    using (StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        message = reader.ReadToEnd();
                    }

                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(message);

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xd.NameTable);
                    nsmgr.AddNamespace("soap", "http://schemas.microsoft.com/sharepoint/soap/");
                    XmlNode digestNode = xd.SelectSingleNode("//soap:DigestValue", nsmgr);
                    if (digestNode != null)
                        digest = digestNode.InnerText;
                }
            }
            catch
            {
                digest = string.Empty;
            }

            return digest;

        }

        private string NotifyBody(string CommaDelimitedIdentifierNames, string CommaDelimitedItemIds)
        {
            string format = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
                            <feed xml:base=""http://services.odata.org/OData/OData.svc/""
                            xmlns:d=""http://schemas.microsoft.com/ado/2007/08/dataservices""
                            xmlns:m=""http://schemas.microsoft.com/ado/2007/08/dataservices/metadata""
                            xmlns:b=""http://schemas.microsoft.com/bcs/2012/""
                            xmlns=""http://www.w3.org/2005/Atom"">
                            <title type=""text"">Categories</title>
                            <id>http://services.odata.org/OData/OData.svc/Categories</id>
                            <updated>2010-03-10T08:38:14Z</updated>
                            <link rel=""self"" title=""Categories"" href=""Categories"" />
                            <entry>
                            <id>http://services.odata.org/OData/OData.svc/Categories(0)</id>
                            <title type=""text"">Food</title>
                            <updated>2010-03-10T08:38:14Z</updated>
                            <author>
                            <name />
                            </author>
                            <link rel=""edit"" title=""Category"" href=""Categories(0)"" />
                            <link rel=""http://schemas.microsoft.com/ado/2007/08/dataservices/related/Products""
                                type=""application/atom+xml;type=feed""
                                title=""Products"" href=""Categories(0)/Products"" />
                            <category term=""ODataDemo.Category""
                                scheme=""http://schemas.microsoft.com/ado/2007/08/dataservices/scheme"" />
                            <content type=""application/xml"">
                            <m:properties>
                            <b:BcsItemIdentity m:type=""Edm.String"">{0}</b:BcsItemIdentity>
                            <d:Name>Food</d:Name>
                            </m:properties>
                            </content>
                            </entry>
                            <!-- <entry> elements representing additional Categories go here -->
                            </feed>
                            ";

            string[] identifierNames = CommaDelimitedIdentifierNames.Split(new char[] { ',' });
            string[] itemId = CommaDelimitedItemIds.Split(new char[] { ',' });

            StringBuilder idBldr = new StringBuilder();
            for (int i = 0; i < identifierNames.Length; i++)
            {
                idBldr.AppendFormat("<{0}>{1}</{0}>", identifierNames[i], itemId[i].ToString());
            }

            return string.Format(format, idBldr.ToString());

        }
    }
}
