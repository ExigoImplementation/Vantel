using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class CommissionsListViewModel
    {
        public IEnumerable<ICommission> Commissions { get; set; }
    }
}