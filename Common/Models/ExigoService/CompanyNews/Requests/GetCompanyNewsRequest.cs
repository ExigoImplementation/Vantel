using System;

namespace ExigoService
{
    public class GetCompanyNewsRequest : DataRequest
    {
        public int[] NewsItemIDs { get; set; }
        public int[] NewsDepartments { get; set; }
    }
}