using Backoffice.Models;

namespace Backoffice.ViewModels
{
    public interface IShoppingViewModel
    {
        ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        string[] Errors { get; set; }
    }
}