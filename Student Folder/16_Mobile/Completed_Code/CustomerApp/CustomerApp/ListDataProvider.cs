﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.SharePoint.Phone.Application;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace CustomerApp
{
    /// <summary>
    /// Provides operations to fetch Items in a SharePoint List. 
    /// </summary>
    public class ListDataProvider : ListDataProviderBase
    {
        /// <summary>
        /// Provides access to ClientContext object which is used to execute queries to fetch ListItems from SharePoint server
        /// </summary>
        private ClientContext m_Context;
        public override ClientContext Context
        {
            get
            {
                if (m_Context != null)
                    return m_Context;

                m_Context = new ClientContext(SiteUrl);

                Authenticator at = new Authenticator();
                at.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                at.PromptOnFailure = true;

                m_Context.Credentials = at;

                return m_Context;
            }
        }

        /// <summary>
        /// Loads an Item from in-memory cache if already fetched once. If not, connects to SharePoint server to load the item asynchronously and update the cache
        /// </summary>
        /// <param name="id">Unique identifier of the Item (In case of external list, BDCIdentity else ListItem.Id)</param>
        /// <param name="loadItemCompletedCallback">Delegate to be passed to the caller upon completion of the operation</param>
        /// <param name="filterParameters">Optional contextual parameters required for loading item</param>
        public override void LoadItem(string id, Action<LoadItemCompletedEventArgs> loadItemCompletedCallback, params object[] filterParameters)
        {
            ListItem cachedItem = GetCachedItem(id);
            if (cachedItem != null)
            {
                loadItemCompletedCallback(new LoadItemCompletedEventArgs { Item = cachedItem });
                return;
            }

            LoadItemFromServer(id, loadItemCompletedCallback, filterParameters);
        }

        /// <summary>
        /// Loads a SharePoint View from in-memory cache if already fetched once. If not, queries SharePoint server to load the view and update the Cache.
        /// </summary>
        /// <param name="ViewName">Name of the SharePoint View</param>
        /// <param name="loadViewCompletedCallback">Delegate to be passed to the caller upon completion of the operation</param>
        /// <param name="filterParameters">>Optional contextual parameters required for loading views</param>
        public override void LoadData(string ViewName, Action<LoadViewCompletedEventArgs> loadViewCompletedCallback, params object[] filterParameters)
        {
            List<ListItem> CachedItems = GetCachedView(ViewName);
            if (CachedItems != null)
            {
                loadViewCompletedCallback(new LoadViewCompletedEventArgs { ViewName = ViewName, Items = CachedItems });
                return;
            }

            LoadDataFromServer(ViewName, loadViewCompletedCallback, filterParameters);
        }


        /// <summary>
        /// Refreshes an Item by connecting to SharePoint server updates the cache
        /// </summary>
        /// <param name="id">Unique identifier of the Item (In case of external list, BDCIdentity else ListItem.Id)</param>
        /// <param name="loadItemCompletedCallback">Delegate to be passed to the caller upon completion of the operation</param>
        /// <param name="filterParameters">Optional contextual parameters required for loading item</param>
        public void RefreshItem(string id, Action<LoadItemCompletedEventArgs> loadItemCompletedCallback, params object[] filterParameters)
        {
            LoadItemFromServer(id, loadItemCompletedCallback, filterParameters);
        }

        /// <summary>
        /// Refreshes a SharePoint View by querying SharePoint server and updates the Cache.
        /// </summary>
        /// <param name="ViewName">Name of the SharePoint View</param>
        /// <param name="loadViewCompletedCallback">Delegate to be passed to the caller upon completion of the operation</param>
        /// <param name="filterParameters">>Optional contextual parameters required for loading views</param>
        public void RefreshData(string ViewName, Action<LoadViewCompletedEventArgs> loadViewCompletedCallback, params object[] filterParameters)
        {
            LoadDataFromServer(ViewName, loadViewCompletedCallback, filterParameters);
        }

        /// <summary>
        /// Loads SharePoint list item with specified id from server
        /// </summary>
        /// <param name="id">Unique identifier of the Item (In case of external list, BDCIdentity else ListItem.Id)</param>
        /// <param name="loadItemCompletedCallback">Delegate to be passed to the caller upon completion of the operation</param>
        /// <param name="filterParameters">Optional contextual parameters required for loading item</param>
        private void LoadItemFromServer(string id, Action<LoadItemCompletedEventArgs> loadItemCompletedCallback, params object[] filterParameters)
        {
            ListItem spListItem = Context.Web.Lists.GetByTitle(ListTitle).GetItemById(id);
            Context.Load(spListItem);
            Context.Load(spListItem, Item => Item.AttachmentFiles);

            Context.ExecuteQueryAsync(
                delegate(object sender1, ClientRequestSucceededEventArgs args)
                {
                    base.CacheItem(spListItem);
                    loadItemCompletedCallback(new LoadItemCompletedEventArgs { Item = spListItem });
                },
                delegate(object sender1, ClientRequestFailedEventArgs args)
                {
                    loadItemCompletedCallback(new LoadItemCompletedEventArgs { Error = args.Exception });
                });
        }

        /// <summary>
        /// Loads SharePoint list items for the specified view from server
        /// </summary>
        /// <param name="ViewName">Name of the SharePoint View</param>
        /// <param name="loadViewCompletedCallback">Delegate to be passed to the caller upon completion of the operation</param>
        /// <param name="filterParameters">>Optional contextual parameters required for loading views</param>
        private void LoadDataFromServer(string ViewName, Action<LoadViewCompletedEventArgs> loadItemCompletedCallback, params object[] filterParameters)
        {
            CamlQuery query = CamlQueryBuilder.GetCamlQuery(ViewName);
            ListItemCollection items = Context.Web.Lists.GetByTitle(ListTitle).GetItems(query);
            Context.Load(items);
            Context.Load(items, listItems => listItems.Include(item => item.FieldValuesAsText));

            Context.ExecuteQueryAsync(
                delegate(object sender, ClientRequestSucceededEventArgs args)
                {
                    base.CacheView(ViewName, items);
                    loadItemCompletedCallback(new LoadViewCompletedEventArgs { ViewName = ViewName, Items = base.GetCachedView(ViewName) });
                },
                delegate(object sender, ClientRequestFailedEventArgs args)
                {
                    loadItemCompletedCallback(new LoadViewCompletedEventArgs { Error = args.Exception });
                });
        }
    }

    /// <summary>
    /// Builds CamlQuery for fetching ListItems in SharePoint Views
    /// </summary>
    public static class CamlQueryBuilder
    {
        /// <summary>
        /// Provides access to ViewXml for SharePoint Views.
        /// </summary>
        static Dictionary<string, string> ViewXmls = new Dictionary<string, string>()
        {   
            {"View1",   @"<View><Query><OrderBy><FieldRef Name='CustomerID' /></OrderBy></Query><RowLimit>30</RowLimit><ViewFields>{0}</ViewFields><Method Name='ReadAllCustomer'></Method></View>"}
        };

        /// <summary>
        /// Provides access to ViewFields which should be fetched for every View. Since, all the Views uses the same Display,Edit and NewForms the ViewFields got 
        /// to be the same for all of them.
        /// </summary>
        static string ViewFields = @"<FieldRef Name='CompanyName'/><FieldRef Name='ContactName'/><FieldRef Name='Phone'/><FieldRef Name='BdcIdentity'/><FieldRef Name='CustomerID'/><FieldRef Name='ContactTitle'/><FieldRef Name='Address'/><FieldRef Name='City'/><FieldRef Name='Region'/><FieldRef Name='PostalCode'/><FieldRef Name='Country'/><FieldRef Name='Fax'/>";

        /// <summary>
        /// Returns CamlQuery to fetching ListItems for SharePoint view with the specified name
        /// </summary>
        /// <param name="viewName">Name of the View</param>
        /// <returns>CamlQuery for the View</returns>
        public static CamlQuery GetCamlQuery(string viewName)
        {
            string viewXml = ViewXmls[viewName];
            //Add ViewFields to the ViewXml
            viewXml = string.Format(viewXml, ViewFields);

            return new CamlQuery { ViewXml = viewXml };
        }
    }
}
