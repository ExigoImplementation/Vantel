using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetChildWebCategoriesRequest
    {
        public GetChildWebCategoriesRequest()
        {
            this.NestedType = NestedType.UniLevel;
        }
        public GetChildWebCategoriesRequest(GetChildWebCategoriesRequest request)
        {
            this.WebID                 = request.WebID;
            this.TopCategoryID         = request.TopCategoryID;
            this.GetAllChildCategories = request.GetAllChildCategories;
            this.NestedType            = request.NestedType;
        }
        /// <summary>
        /// WebID override
        /// <para>Leave null unless non-default webID is required</para>
        /// </summary>
        public int? WebID { get; set; }
        public int TopCategoryID { get; set; }
        public bool GetAllChildCategories { get; set; }
        public NestedType NestedType { get; set; }
    }
    public enum NestedType
    {
        UniLevel,
        Nested
    }
}