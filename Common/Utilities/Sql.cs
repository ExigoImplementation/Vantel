using ExigoService;
using System.Linq;
using Dapper;

namespace Common
{
    public static partial class GlobalUtilities
    {
        public static bool VerifySqlTableExists(string connectionString, string tableName)
        {
            tableName = tableName.Replace("[", "").Replace("]", "");

            var schema = "dbo";
            var table = tableName;

            // Check to see if we 
            if (tableName.Contains("."))
            {
                schema = tableName.Split('.')[0];
                table = tableName.Split('.')[1];
            }

            var exists = false;
            using (var context = ExigoDAL.Sql(connectionString))
            {
                exists = context.Query<bool>(@"
                    if (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table))
                    begin select cast(1 as bit) end
                    else begin select cast(0 as bit) end
                ", new
                 {
                     schema = schema,
                     table = table
                 }).FirstOrDefault();
            }

            return exists;
        }
    }
}