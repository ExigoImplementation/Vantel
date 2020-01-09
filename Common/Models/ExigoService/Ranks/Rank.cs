namespace ExigoService
{
    public class Rank : IRank
    {
        public int RankID { get; set; }
        public string RankDescription { get; set; }

        public static Rank Default
        {
            get
            {
                return new Rank
                {
                    RankDescription = "Unknown"
                };
            }
        }
    }
}