using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class EditShippingAddressViewModel
    {
        public IEnumerable<ShippingAddress> Addresses { get; set; }
    }
}