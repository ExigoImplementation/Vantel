using System;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        private static Dictionary<Common.Api.ExigoWebService.FrequencyType, int> FrequencyTypeBindings
        {
            get
            {
                return new Dictionary<Common.Api.ExigoWebService.FrequencyType, int>()
                {
                    { Common.Api.ExigoWebService.FrequencyType.Weekly, 1 },
                    { Common.Api.ExigoWebService.FrequencyType.BiWeekly, 2 },
                    { Common.Api.ExigoWebService.FrequencyType.Monthly, 3 },
                    { Common.Api.ExigoWebService.FrequencyType.Quarterly, 4 },
                    { Common.Api.ExigoWebService.FrequencyType.SemiYearly, 5 },
                    { Common.Api.ExigoWebService.FrequencyType.Yearly, 6 },
                    { Common.Api.ExigoWebService.FrequencyType.BiMonthly, 7 },
                    { Common.Api.ExigoWebService.FrequencyType.EveryFourWeeks, 8 },
                    { Common.Api.ExigoWebService.FrequencyType.EverySixWeeks, 9 },
                    { Common.Api.ExigoWebService.FrequencyType.EveryEightWeeks, 10 },
                    { Common.Api.ExigoWebService.FrequencyType.EveryTwelveWeeks, 11 }    
                };
            }
        }

        public static int GetFrequencyTypeID(Common.Api.ExigoWebService.FrequencyType FrequencyType)
        {
            try
            {
                return FrequencyTypeBindings.Where(c => c.Key == FrequencyType).FirstOrDefault().Value;
            }
            catch
            {
                throw new Exception("Corresponding int not found for FrequencyType {0}.".FormatWith(FrequencyType.ToString()));
            }
        }
        public static Common.Api.ExigoWebService.FrequencyType GetFrequencyType(int FrequencyTypeID)
        {
            try
            {
                return FrequencyTypeBindings.Where(c => c.Value == FrequencyTypeID).FirstOrDefault().Key;
            }
            catch
            {
                throw new Exception("Corresponding FrequencyType not found for int {0}.".FormatWith(FrequencyTypeID));
            }
        }
    }
}
