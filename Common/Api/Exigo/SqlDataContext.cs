using Common;
using Common.Api.ExigoSql;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static ExigoSqlDataContext SqlDataContext()
        {
            return new ExigoSqlDataContext(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting);
        }
        public static ExigoSqlDataContext SqlDataContext(string connectionString)
        {
            return new ExigoSqlDataContext(connectionString);
        }
    }
}