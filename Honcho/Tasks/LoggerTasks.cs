using System;
using System.IO;
using Honcho.Utils;

namespace Honcho.Tasks
{
    public class LoggerTasks
    {
        public static void DeleteLog()
        {
            Console.WriteLine(string.Format("Deleting log [{0}]", Paths.LogFile));
            File.Delete(Paths.LogFile);
            Console.WriteLine("Log deleted{0}", DateTime.Now);
        }
    }
}