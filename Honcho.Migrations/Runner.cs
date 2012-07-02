using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;
using FluentMigrator.Runner.Generators.SqlServer;
using Honcho.Migrations.AspNetMembership;

namespace Honcho.Migrations
{
    public class Runner
    {
        public static void MigrateUp(string connectionString)
        {
            MigrateUp(connectionString, null, null, false);
        }

        public static void MigrateUp(string connectionString, long? version)
        {
            MigrateUp(connectionString, version, null, false);
        }

        public static void MigrateUp(string connectionString, long? version, Stream stream, bool previewOnly)
        {
            if (Migrate.Exists(connectionString))
            {
                Console.WriteLine(string.Format("AspNet Membership Exists. No Action Needed."));
            }
            else
            {
                Console.WriteLine(string.Format("Installing AspNet Membership"));
                Migrate.InstallIfDoesNotExist(connectionString);
                Console.WriteLine(string.Format("AspNet Membership Install Complete"));
            }
            
            Console.WriteLine(string.Format("Executing Migration Scripts."));
            using (var connection = new SqlConnection(connectionString))
            {
                IAnnouncer announcer = new NullAnnouncer(); // TextWriterAnnouncer(Console.Out);
                var processorOptions = new ProcessorOptions
                {
                    PreviewOnly = previewOnly
                };

                if (stream != null)
                {
                    announcer = new TextWriterAnnouncer(new StreamWriter(stream));
                }

                var runnerContext = new RunnerContext(announcer);

                var processor = new SqlServerProcessor(connection, new SqlServer2008Generator(), announcer, processorOptions);
                var runner = new MigrationRunner(Assembly.GetAssembly(typeof(Runner)), runnerContext, processor);
         
                if (version.HasValue)
                    runner.MigrateUp(version.Value);
                else
                        runner.MigrateUp();
            }
            Console.WriteLine(string.Format("Migration Script Execution Complete"));
        }

        public static void MigrateDown(string connectionString, long version)
        {
            MigrateDown(connectionString, version, null, false);
        }

        public static void MigrateDown(string connectionString, long version, Stream stream, bool previewOnly)
        {
            if (version == 0)
            {
                Console.WriteLine(string.Format("Purging Database"));
                Honcho.Migrations.DatabasePurge.Migrate.Execute(connectionString);
                Console.WriteLine(string.Format("Database Purge Complete"));
            }
            else
            {
                Console.WriteLine(string.Format("Executing Migration Rollback Scripts."));
                using (var connection = new SqlConnection(connectionString))
                {
                    IAnnouncer announcer = new NullAnnouncer(); // TextWriterAnnouncer(Console.Out);
                    var processorOptions = new ProcessorOptions
                                               {
                                                   PreviewOnly = previewOnly
                                               };

                    if (stream != null)
                    {
                        announcer = new TextWriterAnnouncer(new StreamWriter(stream));
                    }

                    var runnerContext = new RunnerContext(announcer);

                    var processor = new SqlServerProcessor(connection, new SqlServer2008Generator(), announcer,
                                                           processorOptions);
                    var runner = new MigrationRunner(Assembly.GetAssembly(typeof (Runner)), runnerContext,
                                                     processor);

                    runner.RollbackToVersion(version);
                }
                Console.WriteLine(string.Format("Migration Rollback Script Execution Complete"));
            }
        }

        public static Int64 CurrentVersion(string connectionString)
        {
            Int64 version = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                const string sql = "select max(version) from VersionInfo;";

                SqlCommand sqlCommand = conn.CreateCommand();
                sqlCommand.CommandText = sql;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                try
                {
                    version = (Int64) sqlCommand.ExecuteScalar();
                }
                catch
                {
                    
                }
                conn.Close();
            }

            return version;
        }
    }
}
