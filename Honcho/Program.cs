using System;
using System.Configuration;
using Honcho.Tasks;

namespace Honcho
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World. This is Honcho.");
            RunCommand("h");
            RunCommand("c");
            RunCommand("v", true);
        }

        private static void NextTaskPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("What now?");
            string command = Console.ReadLine();
            RunCommand(command, true);
        }

        private static void RunCommand(string command, bool withNextPrompt = false)
        {
            Console.WriteLine("");
            var cmd = command.Split(' ');
            command = cmd[0];

            var migrationTasks = BuildMigrationTasks();

            Int64 version;
            switch (command)
            {
                case "c":
                    migrationTasks.OutputConnectionString();
                    break;
                case "v":
                    Console.WriteLine(string.Format("Current Version: {0}", migrationTasks.CurrentVersion()));
                    break;
                case "p":
                    migrationTasks.MigrateDown(0);
                    break;
                case "r":
                    if (cmd.Length == 2 && Int64.TryParse(cmd[1], out version))
                    {
                        migrationTasks.MigrateDown(version);    
                    }
                    else
                    {
                        Console.WriteLine("Provide a valid migration number.");
                        Console.WriteLine("For example, r 2 will rollback to migration 2.");
                    }
                    break;
                case "m":
                    if (cmd.Length == 2 && Int64.TryParse(cmd[1], out version))
                    {
                        migrationTasks.MigrateUpTo(version);
                    }
                    else if (cmd.Length == 2)
                    {
                        Console.WriteLine("Provide a valid migration number.");
                        Console.WriteLine("For example, m 5 will migrate to migration 5.");
                    }
                    else
                    {
                        migrationTasks.MigrateUp();
                    }
                    
                    break;
                case "x":
                case "q":
                    Environment.Exit(0);
                    break;
                case "h":
                case "help":
                    Console.WriteLine("Usage:");
                    Console.WriteLine("     c - get configured connection string");
                    Console.WriteLine("     v - get current migration version");
                    Console.WriteLine("     m [version number] - migrate database up to version number");
                    Console.WriteLine("     r <version number> - rollback database to required version number");
                    Console.WriteLine("     p - purge all artifacts from database");
                    Console.WriteLine("     q - quit");
                    Console.WriteLine("     x - exit");
                    Console.WriteLine("     h - help");
                    break;
                default:
                    Console.WriteLine("Huh? It looks like you could use some help.");
                    RunCommand("h");
                    break;
            }

            Console.WriteLine("");

            if (withNextPrompt) NextTaskPrompt();
        }


        private static MigrationTasks BuildMigrationTasks()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MigrationDatabase"].ConnectionString;
            return new MigrationTasks(connectionString);
        }
       
    }
}