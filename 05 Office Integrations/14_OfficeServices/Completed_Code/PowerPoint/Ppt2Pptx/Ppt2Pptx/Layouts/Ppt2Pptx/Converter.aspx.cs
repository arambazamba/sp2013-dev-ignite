using System;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.IO;
using Microsoft.Office.Server.PowerPoint.Conversion;
using Microsoft.SharePoint.Administration;

namespace Ppt2Pptx.Layouts.Ppt2Pptx
{
    public partial class Converter : LayoutsPageBase
    {

        string siteUrl = string.Empty;
        string listId = string.Empty;
        string itemId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string fileIn = string.Empty;
            string fileOut = string.Empty;


            try
            {
                siteUrl = Request.QueryString["SiteUrl"];
                listId = Request.QueryString["ListId"];
                itemId = Request.QueryString["ItemId"];

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            //Get item to convert
                            SPDocumentLibrary library = (SPDocumentLibrary)web.Lists[new Guid(listId)];
                            SPListItem item = library.GetItemById(int.Parse(itemId));
                            SPFile file = item.File;

                            //Get file names

                            fileIn = (string)file.Item[SPBuiltInFieldId.EncodedAbsUrl];
                            fileOut = fileIn.Replace(".ppt", ".pptx");

                            if (!fileIn.EndsWith(".ppt"))
                                throw new Exception("Input file must be PowerPoint 97-2003 file (.ppt)");

                            //Get file content to convert
                            byte[] buffer = file.OpenBinary();
                            MemoryStream inStream = new MemoryStream(buffer);
                            MemoryStream outStream = new MemoryStream();

                            // Create the presentation conversion request.                  
                            PresentationRequest request = new PresentationRequest(inStream, ".ppt", outStream);

                            // Synchronous Request
                            IAsyncResult result = request.BeginConvert(SPServiceContext.GetContext(site), null, null);
                            request.EndConvert(result);

                            // Add the converted file to the document library
                            web.AllowUnsafeUpdates = true;
                            web.Files.Add(fileOut, outStream.ToArray(), true);
                            web.AllowUnsafeUpdates = false;

                            //Go back to library
                            SPUtility.Redirect(library.DefaultViewUrl, SPRedirectFlags.Default, HttpContext.Current);

                        }
                    }
                });
            }
            catch (Exception x)
            {
                StringBuilder message = new StringBuilder();
                message.Append("<p>" + x.Message + "</p>");
                message.Append("<p>PPT File: ");
                message.Append(fileIn);
                message.Append("</p><p>PPTX File: ");
                message.Append(fileOut);
                message.Append("</p>");
                Messages.Text = message.ToString();

            }
        }
    }
}
