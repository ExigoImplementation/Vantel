using ExigoService;
using System.Collections.Generic;
using Common.Models;

namespace Backoffice.ViewModels
{
    public class PartyDashboardViewModel
    {
        public PartyDashboardViewModel()
        {
            this.Parties = new List<Party>();
            this.WeeklySalesTotals = new List<WeekSalesTotalNode>();
        }

        public List<Party> Parties { get; set; }
        public List<WeekSalesTotalNode> WeeklySalesTotals { get; set; }
        public decimal MonthlyTotalSales { get; set; }
    }
}