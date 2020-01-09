using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExigoService;
using System.Data.SqlClient;
using System.Data;

namespace Common.Services
{
    public class CommissionReportRefreshTaskService
    {

        //Creating tasks to periodically request the report and update an order to trigger the report cache to refresh. 
        //This is only an issue if the site isn't getting any traffic or order updates for multiple hours.
        public static void InitializeCommissionReportRefreshTask()
        {

            int delayMinutes = GlobalSettings.Backoffices.Reports.CommissionReportCacheRefresh.taskWaitTime; //We are only going to run once an hour.... this should be plenty to keep the report cache populated in Demo...
            int customerID = 1;
            int orderID = 1;

            Task.Run(async delegate
            {
                while (true)
                {

                    using (var connection = ExigoDAL.Sql())
                    {
                        //get an OrderID
                        connection.Open();
                        using (var command = new SqlCommand(@"SELECT TOP 1 OrderID from Orders", connection))
                        {
                            command.CommandType = CommandType.Text;
                            orderID = (int)command.ExecuteScalar();
                        }

                        //Get a customerid
                        using (var command = new SqlCommand(@"SELECT TOP 1 CustomerID from Customers", connection))
                        {
                            command.CommandType = CommandType.Text;
                            customerID = (int)command.ExecuteScalar();                          
                        }
                        connection.Close();
                    }

                    //Request the Rank Qual Report
                    ExigoDAL.WebService().GetRankQualifications(new Api.ExigoWebService.GetRankQualificationsRequest
                    {
                        CustomerID = orderID,
                        PeriodType = 1 //assuming periodtype 1 will always exist....
                    });

                    //Update the Order
                    //All we need is the modified date on the order to change, so we don't need to actually update anything on the order.
                    ExigoDAL.WebService().UpdateOrder(new Api.ExigoWebService.UpdateOrderRequest
                    {
                        OrderID = orderID
                    });

                    //Request the Rank Qual Report
                    ExigoDAL.WebService().GetRankQualifications(new Api.ExigoWebService.GetRankQualificationsRequest
                    {
                        CustomerID = orderID,
                        PeriodType = 1 //assuming periodtype 1 will always exist....
                    });

                    await Task.Delay(delayMinutes * 60 * 1000);
                }
            });
        }
    }
}