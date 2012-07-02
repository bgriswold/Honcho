using System.Data;
using System.Data.SqlClient;

namespace Honcho.Migrations.DatabasePurge
{
    public partial class Migrate 
    {
        public static void Execute(string connectionString)
        {
            string db = GetDatabaseName(connectionString);

            string script = PurgeRolesScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeProceduresScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeConstraintsScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeFunctionsScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeViewsScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeForeignKeysScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeTablesScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeUdtScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);

            script = PurgeSchemasScript.Replace(Keys.ReplacementTokens.DatabaseName, db);
            ExecuteSqlCommands(script, connectionString);
        }

        private static void ExecuteSqlCommands(string sql, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = conn.CreateCommand();
                sqlCommand.CommandText = sql;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    reader.Close();
                }
                
                conn.Close();
            }
        }

        private static string GetDatabaseName(string connectionString)
        {
            string db;
            using (var connection = new SqlConnection(connectionString))
            {
                db = connection.Database;
            }

            return db;
        }
      
      
    }
}