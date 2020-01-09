using System.Data.SqlClient;
using Common;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static SqlConnection Sql()
        {
            return new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting);
        }
        public static SqlConnection Sql(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}