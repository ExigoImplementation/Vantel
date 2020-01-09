using System;

namespace ExigoService
{
    public interface ICompanyNews
    {
        string Department { get; set; }
        int NewsID { get; set; }
        string Description { get; set; }
        string Content { get; set; }
        DateTime CreatedDate { get; set; }
    }
}