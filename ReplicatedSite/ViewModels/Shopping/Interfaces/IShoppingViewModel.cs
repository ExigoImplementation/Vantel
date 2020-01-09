using ReplicatedSite.Models;

namespace ReplicatedSite.ViewModels
{
    public interface IShoppingViewModel
    {
        ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        string[] Errors { get; set; }
    }
}