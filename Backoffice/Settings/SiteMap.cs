
using System.Linq;
using System.Collections.Generic;
using Backoffice.Models.SiteMap;
using Common;

namespace Backoffice
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
                            new NavigationSiteMapNode("dashboard") { Icon = "fa-home", Action = "index", Controller = "dashboard" },

                            new NavigationSiteMapNode("commissions", Resources.Common.Commissions, new List<ISiteMapNode>()                    
                            {
                                new NavigationSiteMapNode("page-commissions", Resources.Common.Commissions) { Action = "commissiondetail", Controller = "commissions" },
                                new NavigationSiteMapNode("page-rank", Resources.Common.RankAdvancement) { Action = "rank", Controller = "commissions" },
                                new NavigationSiteMapNode("page-volumes", Resources.Common.Volumes) { Action = "volumelist", Controller = "commissions" }
                            }),

                            new NavigationSiteMapNode("organization", Resources.Common.Organization, new List<ISiteMapNode>()
                            {
                                new NavigationSiteMapNode("enroll", Resources.Common.EnrollNew) { Action="EnrollmentRedirect", Controller="App", Target = "_blank", Icon = "fa-plus" },
                                new DividerNode(),
                                new NavigationSiteMapNode("personallyenrolled", Resources.Common.PersonallyEnrolledTeam) { Action = "personallyenrolledlist", Controller = "organization" },
                                new NavigationSiteMapNode("mycustomers", Resources.Common.MyCustomers) { Action = "mycustomerslist", Controller = "organization" },
                                new NavigationSiteMapNode("upcomingpromotions", Resources.Common.UpcomingPromotions) { Action = "upcomingpromotionslist", Controller = "organization" },                        
                                new NavigationSiteMapNode("downlineranks", Resources.Common.DownlineRanks) { Action = "downlinerankslist", Controller = "organization" },
                                new NavigationSiteMapNode("downlineorders", Resources.Common.DownlineOrders) { Action = "downlineorderslist", Controller = "organization" },
                                new NavigationSiteMapNode("downlineautoorders", Resources.Common.DownlineAutoOrders) { Action = "downlineautoorderslist", Controller = "organization" },
                                new NavigationSiteMapNode("newdistributors", Resources.Common.NewDistributorsList) { Action = "NewDistributorsList", Controller = "organization" },
                                new NavigationSiteMapNode("recentactivity", Resources.Common.RecentActivityList) { Action = "RecentActivityList", Controller = "organization" },
                                new NavigationSiteMapNode("unileveltreeviewer", "Unilevel Tree Viewer") { Action = "unileveltreeviewer", Controller = "organization" },
                                //new NavigationSiteMapNode("treeviewer", "Tree Viewer") { Action = "treeviewer", Controller = "organization" }
                            }),

                            new NavigationSiteMapNode("events", Resources.Common.Events, new List<ISiteMapNode>()
                            {
                                new NavigationSiteMapNode("calendar", Resources.Common.Calendar) { Action = "calendar", Controller = "events" },
                                new NavigationSiteMapNode("subscriptions", Resources.Common.Subscriptions) { Action = "subscriptions", Controller = "events" }
                            }),

                            new NavigationSiteMapNode("autoorders", Resources.Common.AutoOrders) { Action = "AutoOrderList", Controller = "autoorders" },

                            new NavigationSiteMapNode("orders", Resources.Common.Orders) { Action = "orderlist", Controller = "orders" },

                            //new NavigationSiteMapNode("parties", Resources.Common.Parties)
                            //{
                            //    Action = "index", Controller = "parties", Children = new List<ISiteMapNode>()
                            //    {
                            //        new NavigationSiteMapNode("partydashboard", Resources.Common.Dashboard) { Action = "index", Controller = "parties" },
                            //        new NavigationSiteMapNode("addnewparty", Resources.Common.CreateNewParty) { Icon = "fa-plus", Action = "createparty", Controller = "parties" },
                            //        new NavigationSiteMapNode("openparties", Resources.Common.ViewOpenParties) { Action = "openparties", Controller = "parties" },
                            //        new NavigationSiteMapNode("closedparties", Resources.Common.ViewClosedParties) { Action = "closedparties", Controller = "parties" }
                            //    }
                            //},

                            new NavigationSiteMapNode("resources", Resources.Common.Resources, new List<ISiteMapNode>()
                            {
                                new NavigationSiteMapNode("resourcelist", Resources.Common.ResourcesLibrary) { Action = "resourcelist", Controller = "resources" },
                                new NavigationSiteMapNode("manageresources", Resources.Common.ManageResources) { Action = "manageresources", Controller = "resources", IsVisible = () => new[] { GlobalSettings.Backoffices.AdministratorID}.Contains(Identity.Current.CustomerID)}
                            }),

                            //2016-12-06 Ivan S. Content Manager
                            //This feature is being developed as a Phase 2 of the content Manager module
                            //We don't want to lose the code we have done, that is why we didn't remove this line of code
                            //new NavigationSiteMapNode("contentmanager", Resources.Common.Content) { Action = "contentmanager", Controller = "content", IsVisible = () => new[] { GlobalSettings.Backoffices.AdministratorID}.Contains(Identity.Current.CustomerID) },

                            //new NavigationSiteMapNode("store", Resources.Common.Store)
                            //{
                            //    Action = "itemlist", Controller = "Shopping", Children = new List<ISiteMapNode>()
                            //    {
                            //        new NavigationSiteMapNode("items", Resources.Common.Products) { Action = "itemlist", Controller = "Shopping" },
                            //        new NavigationSiteMapNode("cart", Resources.Common.MyCart) { Action = "cart", Controller = "Shopping" }
                            //    }
                            //},
                            new NavigationSiteMapNode("companynews", Resources.Common.CompanyNews){ Action = "companynewslist", Controller = "companynews" },

                            new NavigationSiteMapNode("account", Resources.Common.Account, new List<ISiteMapNode>()
                            {
                                new NavigationSiteMapNode("accountsettings", Resources.Common.AccountSettings) { Action = "index", Controller = "account" },
                                new NavigationSiteMapNode("avatar", Resources.Common.Avatar) { Action = "manageavatar", Controller = "account" },
                                new NavigationSiteMapNode("addresses", Resources.Common.Addresses) { Action = "addresslist", Controller = "account" },
                                new NavigationSiteMapNode("paymentmethods", Resources.Common.PaymentMethods) { Action = "paymentmethodlist", Controller = "account" },
                                new NavigationSiteMapNode("directdeposit", Resources.Common.DirectDeposit) { Action = "commissionpayout", Controller = "account" },
                                new NavigationSiteMapNode("notifications", Resources.Common.Notifications) { Action = "notifications", Controller = "account" },
                                new NavigationSiteMapNode("signout", Resources.Common.SignOut) { Action = "logout", Controller = "authentication" }
                            }),

                            new NavigationSiteMapNode("signout", Resources.Common.SignOut) { Action = "logout", Controller = "authentication", DeviceVisibilityCssClass = "d-block d-sm-none" }
                        }
                    };
                }
            }
        }
    }
}