using Common.Api.ExigoWebService;
using System;

namespace ExigoService
{
    public class CompanyNewsItem : ICompanyNewsItem
    {
        public int Department{ get; set; }
        public int NewsID { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }       
        public DateTime CreatedDate { get; set; }
        public NewsWebSettings WebSettings { get; set; }
        public NewsCompanySettings CompanySettings { get; set; }
        public DepartmentInfo[] Departments { get; set; }

    }
}