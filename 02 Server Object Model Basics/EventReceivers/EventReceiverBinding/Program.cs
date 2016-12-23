using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.SharePoint;

namespace EventReceiverBinding
{
    class Program
    {
        static void Main(string[] args)
        {
            SPSite site = new SPSite("http://sp2013/");
            SPWeb web = site.RootWeb;

            SPList list = web.Lists["OfferDocs"];

            string assName = Assembly.GetExecutingAssembly().FullName;
            string className = "";

            list.EventReceivers.Add(SPEventReceiverType.ItemAdding, assName, className);
        }
    }
}
