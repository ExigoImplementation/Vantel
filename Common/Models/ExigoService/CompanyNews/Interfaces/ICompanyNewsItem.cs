using System;

namespace ExigoService
{
    public interface ICompanyNewsItem
    {
        int NewsID { get; set; }
        string Description { get; set; }
        string Content { get; set; }
        DateTime CreatedDate { get; set; }
    }
}