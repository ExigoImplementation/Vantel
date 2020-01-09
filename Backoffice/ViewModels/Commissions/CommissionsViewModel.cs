using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class CommissionDetailViewModel
    {
        public IEnumerable<ICommission> CommissionPeriods { get; set; }
        public IEnumerable<ICommission> Commissions { get; set; }
        public int PeriodID { get; set; }

        public bool IsCurrentCommissions { get; set; }
    }
}