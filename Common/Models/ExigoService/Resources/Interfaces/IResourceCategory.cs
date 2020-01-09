using System;
namespace ExigoService
{
    public interface IResourceCategory
    {
        Guid CategoryID { get; set; }
        string CategoryDescription { get; set; }
    }
}
