using System.Linq;
using System.Collections.Generic;
using ReplicatedSite.Models.SiteMap;
using Common;

namespace ReplicatedSite
{
    /// <summary>
    /// Site-specific settings
    /// </summary>
    public static partial class Settings
    {
        /// <summary>
        /// Site-wide navigation configurations
        /// </summary>
        public static class SiteMap
        {
            public static NavigationSiteMap Current
            {
                get 
                {
                    return new NavigationSiteMap()
                    {
                        Items = new List<ISiteMapNode>()
                        {
                            new NavigationSiteMapNode("dashboard", Resources.Common.Home) { Action = "index", Controller = "home" },
                            new NavigationSiteMapNode("about", Resources.Common.AboutUs) { Action = "about", Controller = "home" },
                            new NavigationSiteMapNode("shop", Resources.Common.Shop) { Action = "itemlist", Controller = "shopping" },
                            new NavigationSiteMapNode("join", Resources.Common.Join) { Action = "index", Controller = "enrollment" },
                            new NavigationSiteMapNode("login", Resources.Common.Login) { Action = "login", Controller = "account", IsVisible = () => (Identity.Customer == null) }, 
                            new NavigationSiteMapNode("myaccount", Resources.Common.MyAccount) { Action = "index", Controller = "account", IsVisible = () => (Identity.Customer != null) },
                            new NavigationSiteMapNode("account", Resources.Common.Account, new List<ISiteMapNode>()
                            {
                                new NavigationSiteMapNode("accountsettings", Resources.Common.AccountSettings) { Action = "index", Controller = "account" },
                                new NavigationSiteMapNode("orders", Resources.Common.Orders) { Action = "orderlist", Controller = "orders" },
                                new NavigationSiteMapNode("autoorders", Resources.Common.AutoOrders) { Action = "autoorderlist", Controller = "autoorders" },
                                new NavigationSiteMapNode("addresses", Resources.Common.Addresses) { Action = "addresslist", Controller = "account" },
                                new NavigationSiteMapNode("paymentmethods", Resources.Common.YourCreditCards) { Action = "paymentmethodlist", Controller = "account" },
                                new NavigationSiteMapNode("signout", Resources.Common.SignOut) { Action = "logout", Controller = "account" }
                            }),
                            new NavigationSiteMapNode("signout", Resources.Common.SignOut) { Action = "logout", Controller = "account", DeviceVisibilityCssClass = "visible-xs" }
                        }
                    };
                }
            }
        }
    }
}