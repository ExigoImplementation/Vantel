using Backoffice;
using Common.Helpers;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public static class LinqExtensions
    {
        public static List<CustomerWallItem> Tokenize(this IEnumerable<CustomerWallItem> list)
        {
            var results = new List<CustomerWallItem>();
            foreach (var item in list)
            {
                    item.CustomerIDToken = Common.Services.Security.Encrypt(item.Field2, Identity.Current.CustomerID);
            }
            return list.ToList();
        }
    }
}