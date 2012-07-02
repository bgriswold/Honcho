using System;

namespace Honcho.Utils
{
    public class CurrentDirectory : RelativeDirectory
    {
        /// <summary>
        ///  Sets the relative directory location based on the environment's current directory.
        ///  Often this is the project's debug location when running locally.
        /// </summary>
        public CurrentDirectory() : base (Environment.CurrentDirectory)
        {
      
        }
    }
}