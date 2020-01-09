using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    /// <summary>
    /// From DB [dbo].[ItemTypes]
    /// </summary>
    public static class ItemTypes
    {
        /// <summary>
        /// Item Type ID 0
        /// </summary>
        public const int Standard = 0;
        /// <summary>
        /// Item Type ID 1
        /// </summary>
        public const int StaticKit = 1;
        /// <summary>
        /// Item Type ID 2
        /// </summary>
        public const int DynamicKit = 2;


        /// <summary>
        /// --Do not use unless necessary for legacy support--
        /// Item Type ID 3
        /// </summary>
        //public const int StaticKit_Legacy = 3;
    }
}