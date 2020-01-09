using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    /// <summary>
    /// Object based on Database structure from [dbo].[WebCategoryItems]0
    /// </summary>
    public class WebCategoryItem
    {
        public int WebID { get; set; }
        public int WebCategoryID { get; set; }
        public string ItemCode { get; set; }
        public int SortOrder { get; set; }
    }
}