using System;
namespace ExigoService
{
    public interface IResourceType
    {
        Guid TypeID { get; set; }
        string TypeDescription { get; set; }
    }
}
