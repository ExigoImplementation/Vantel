using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class RankViewModel
    {
        public IEnumerable<Rank> Ranks { get; set; }
        public Rank CurrentRank { get; set; }
        public Rank NextRank { get; set; }
    }
}