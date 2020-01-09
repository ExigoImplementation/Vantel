using System;

namespace ExigoService
{
    public interface ICategoryItem
    {
        Guid ItemID { get; set; }
        Guid CategoryID { get; set; }
        int ItemOrder { get; set; }
    }
}
