using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ExigoService;

namespace Backoffice.Services
{
    public static class RankService
    {
        public static IEnumerable<Rank> GetRanks()
        {
            var ranks = new List<Rank>();
            using (var context = ExigoDAL.Sql())
            {
                ranks = context.Query<Rank>(@"
                        SELECT 
	                        r.RankID
	                        ,r.RankDescription

                        FROM
	                        Ranks r                                
                        ").OrderBy(c => c.RankID).ToList();
            }

            //Ensure that rank 0 exists
            if(ranks.Where(c => c.RankID == 0).FirstOrDefault() == null)
            {
                ranks.Insert(0, new Rank() { RankID = 0, RankDescription = "" });
            }

            foreach (var rank in ranks)
            {
                yield return rank;
            }
        }

        public static Rank GetRank(int rankID)
        {
            return GetRanks()
                .Where(c => c.RankID == rankID)
                .FirstOrDefault();
        }

        public static IEnumerable<Rank> GetNextRanks(int rankID)
        {
            return GetRanks()
                .Where(c => c.RankID > rankID)
                .OrderBy(c => c.RankID)
                .ToList();
        }
        public static Rank GetNextRank(int rankID)
        {
            return GetNextRanks(rankID).FirstOrDefault();
        }

        public static IEnumerable<Rank> GetPreviousRanks(int rankID)
        {
            return GetRanks()
                .Where(c => c.RankID < rankID)
                .OrderByDescending(c => c.RankID)
                .ToList();
        }
        public static Rank GetPreviousRank(int rankID)
        {
            return GetPreviousRanks(rankID).FirstOrDefault();
        }
        
    }
}