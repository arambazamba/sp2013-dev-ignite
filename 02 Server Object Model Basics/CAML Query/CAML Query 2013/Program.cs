using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint;

namespace CAML_Query_2013
{
    class Program
    {
        static void Main(string[] args)
        {
            string camlQuery = @"<Where><Eq><FieldRef Name=""Title""/><Value Type=""Text"">Connell</Value></Eq></Where>";

            var col = new SPSite("http://sp2013");
            SPWeb web = col.RootWeb;
            SPList list = web.Lists["Customers"];

            var query = new SPQuery { Query = camlQuery };

            SPListItemCollection filteredItems = list.GetItems(query);

            Debug.WriteLine(string.Format("Found {0} items in query.", filteredItems.Count));
            foreach (SPListItem item in filteredItems)
            {
                Debug.WriteLine(string.Format("Found item: {0} modiefied on {1}",
                                              item["Title"],
                                              item["Modified"]));
            }
        }
    }
}
