using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    interface IItemDetailFilterRequest
    {
        bool IncludeShortDescriptions { get; set; }
        bool IncludeLongDescriptions { get; set; }
        bool IncludeExtendedFields { get; set; }
        bool IncludeExtendedlItemPrices { get; set; }
    }
}
