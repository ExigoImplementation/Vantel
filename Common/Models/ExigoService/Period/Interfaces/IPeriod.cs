using System;

namespace ExigoService
{
    public interface IPeriod
    {
        int PeriodID { get; set; }
        int PeriodTypeID { get; set; }
        string PeriodDescription { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}