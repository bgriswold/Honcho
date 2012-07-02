using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Honcho.Migrations.AspNetMembership
{
    public partial class Migrate 
    {
        public static bool Exists(string connectionString)
        {
            bool found;

            using (var conn = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = conn.CreateCommand();
                sqlCommand.CommandText = ExistsMembershipScript;
                conn.Open();
                
                found =  (int) sqlCommand.ExecuteScalar() == 1;
                conn.Close();
            }

            return found;
        }

        public static void InstallIfDoesNotExist(string connectionString)
        {
            if (Exists(connectionString)) return;
            Install(connectionString);
        }

        public static void Install(string connectionString)
        {
            string db = GetDatabaseName(connectionString);
            // C:\Windows\Microsoft.NET\Framework\v4.0.30319\Aspnet_regsql.exe -d databasename -A all -sqlexportonly c:\Membership.sql
            IEnumerable<string> sqlCommands = ParseSqlCommands(InstallMembershipScript, db);
            ExecuteSqlCommands(sqlCommands, connectionString);
        }

        public static void Rollback(string connectionString)
        {
            string db = GetDatabaseName(connectionString);
            // C:\Windows\Microsoft.NET\Framework\v4.0.30319\Aspnet_regsql.exe -d databasename -R all -sqlexportonly c:\Membership Rollback.sql
            IEnumerable<string> sqlCommands = ParseSqlCommands(RollbackMembershipScript, db);
            ExecuteSqlCommands(sqlCommands, connectionString);
        }

        private static IEnumerable<string> ParseSqlCommands(string sql, string db)
        {
            sql = sql.Replace(Keys.ReplacementTokens.DatabaseName, db);

            var splitter = new[] { "\r\nGO\r\n" };
            return sql.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
        }

        private static void ExecuteSqlCommands(IEnumerable<string> sqlCommands, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                foreach (string sql in sqlCommands)
                {
                    SqlCommand sqlCommand = conn.CreateCommand();
                    sqlCommand.CommandText = sql;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        reader.Close();
                    }
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