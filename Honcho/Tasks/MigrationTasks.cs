using System;
using Honcho.Migrations;

namespace Honcho.Tasks
{
    public class MigrationTasks
    {
        private readonly string _connectionString;

        public MigrationTasks(string connectionString)
        {
            _connectionString = connectionString;
        }
 
        public void MigrateUp()
        {
            Console.WriteLine(string.Format("Migrating DB UP to Latest Version."));

            OutputConnectionString();

            Int64 currentVersion = CurrentVersion();

            Console.WriteLine(string.Format("Current version: {0}", currentVersion));

            Runner.MigrateUp(_connectionString);

            Int64 migratedToVersion = CurrentVersion();

            Console.WriteLine(migratedToVersion == currentVersion
                                  ? string.Format("Already migrated to version {0}. No Action Taken.", currentVersion)
                                  : string.Format("DB Migration UP to Version {0} Complete.", migratedToVersion));
        }

        public void MigrateUpTo(Int64 version)
        {
            Console.WriteLine(string.Format("Migrating DB UP to Version {0}.", version));

            OutputConnectionString();

            Int64 currentVersion = CurrentVersion();

            if (version <= currentVersion)
            {
                Console.WriteLine(string.Format("Already migrated to version {0}. No Action Taken.", currentVersion));
            }
            else
            {
                Runner.MigrateUp(_connectionString, version);
                Console.WriteLine(string.Format("DB Migration UP to Version {0} Complete.", version));
            }
        }

        public void MigrateDown(Int64 version)
        {
            Console.WriteLine(string.Format("Migrating DB DOWN to Version {0}.", version));

            OutputConnectionString();

            Int64 currentVersion = CurrentVersion();

            if (version >= currentVersion && version != 0)
            {
                Console.WriteLine(string.Format("Already migrated to version {0}. No Action Taken.", currentVersion));
            }
            else
            {
                Runner.MigrateDown(_connectionString, version);
                Console.WriteLine(string.Format("DB Migration DOWN to Version {0} Complete.", version));
            }
        }

        public void OutputConnectionString()
        {
            Console.WriteLine(string.Format("Connection:"));
            foreach (var p in _connectionString.Split(';'))
            {
                Console.WriteLine(string.Format("   {0}", p));
            }
        }

        public Int64 CurrentVersion()
        {
            return Runner.CurrentVersion(_connectionString);
        }
    }
}