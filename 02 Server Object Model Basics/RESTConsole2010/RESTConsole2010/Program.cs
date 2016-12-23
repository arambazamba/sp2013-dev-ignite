using System;
using System.Linq;
using System.Net;
using Microsoft.SharePoint;
using RESTConsole2010.Demo;

namespace RESTConsole2010
{
    class Program
    {
        static void Main(string[] args)
        {
            //Service Reference
            //http://sp2013:8070/_vti_bin/listdata.svc


            AppDevelopmentDataContext dc = new AppDevelopmentDataContext(new Uri("http://sp2013/_vti_bin/listdata.svc")) { Credentials = CredentialCache.DefaultNetworkCredentials };

            var result = from d in dc.Inventory
                         select new
                         {
                             Title = d.Title,
                             Description = d.Description,
                             Cost = d.Cost,
                         };

            foreach (var d in result)
            {
                Console.WriteLine(string.Format("Item {0} costs {1}", d.Title, d.Cost));
            }

            //Insert

            InventoryItem newItem = new InventoryItem
            {
                Title = "Boat",
                Description = "Little Yellow Boat",
                Cost = 300
            };
            dc.AddToInventory(newItem);
            dc.SaveChanges();

            //Update

            InventoryItem item = dc.Inventory.FirstOrDefault(i => i.Title == "Car");
            if (item != null)
            {
                item.Title = "Fast Car";
                item.Description = "Super Fast Car";
                item.Cost = 500;
                dc.UpdateObject(item);
                dc.SaveChanges();
            }

            item = null;
            item = dc.Inventory.FirstOrDefault(i => i.Title == "Boat");
            if (item != null)
            {
                dc.DeleteObject(item);
                dc.SaveChanges();
            }
        }
    }
}
