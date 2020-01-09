using System;

namespace ExigoService
{
    public interface IResource
    {
        Guid ItemID { get; set; }
        string TypeName { get; set; }
        string ItemDescription { get; set; }
        string Url { get; set; }
        Guid TypeID { get; set; }
        Guid CategoryID { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
