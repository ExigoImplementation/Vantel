using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class AccountAddressesViewModel
    {
        public IEnumerable<IAddress> Addresses { get; set; }
    }
}