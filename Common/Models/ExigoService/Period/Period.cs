using System;

namespace ExigoService
{
    public class Period : IPeriod
    {
        public int PeriodID { get; set; }
        public int PeriodTypeID { get; set; }
        public string PeriodDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? AcceptedDate { get; set; }

        public static Period Default
        {
            get 
            {
                return new Period
                {
                    PeriodDescription = "Unknown"
                };
            }
        }
    }
}