using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public abstract class BasePartyViewModel
    {
        public BasePartyViewModel()
        {

        }

        public Party Party { get; set; }
    }
}