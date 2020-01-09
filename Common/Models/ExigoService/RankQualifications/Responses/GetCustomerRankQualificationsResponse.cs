using System.Collections.Generic;

namespace ExigoService
{
    public class GetCustomerRankQualificationsResponse
    {
        public IEnumerable<RankQualificationLeg> QualificationLegs { get; set; }
        public decimal TotalPercentComplete { get; set; }
        public bool IsQualified { get; set; }

        public Rank Rank { get; set; }
        public Rank PreviousRank { get; set; }
        public Rank NextRank { get; set; }

        public bool IsUnavailable { get; set; }
        public string ErrorMessage { get; set; }
    }
}