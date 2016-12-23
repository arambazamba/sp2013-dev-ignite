using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint;

namespace LinqConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SmartPortalDataContext dc = new SmartPortalDataContext("http://SP2013");

            foreach (CustomersContact c in dc.Customers)
            {
                Console.WriteLine(c.Title);
            }

            var connell = from c in dc.Customers where c.Title == "Connell" select c;

            TextWriter writer = new StreamWriter(@"c:\caml.txt", true);
            dc.Log = writer;

            foreach (CustomersContact c in connell)
            {
                Console.WriteLine(string.Format("{0} {1}", c.FirstName, c.Title));
            }

            CustomersContact nc = new CustomersContact() { Title = "Simpson", FirstName = "Homer" };
            dc.Customers.InsertOnSubmit(nc);
            dc.SubmitChanges();

            CustomersContact homer = (from c in dc.Customers where c.FirstName == "Homer" select c).FirstOrDefault();
            homer.FirstName = "Homer J.";
            dc.SubmitChanges();

            dc.Customers.DeleteOnSubmit(homer);
            dc.SubmitChanges();
        }
    }
}
