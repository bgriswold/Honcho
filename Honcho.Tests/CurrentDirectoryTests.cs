using System;
using System.IO;
using Honcho.Utils;
using NUnit.Framework;
using Should;

namespace Honcho.Tests
{
    [TestFixture]
    public class CurrentDirectoryTests : TestBase
    {
        [Test]
        public void Constructor_Should_Set_CurrentDirectory()
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory).Name;
            new CurrentDirectory().Name.ShouldEqual(currentDirectory);
        }

        [Test]
        public void MoveUpToDirectory_Should_Set_New_Directory_By_Name()
        {
            const string name = "Honcho.Tests";
            var currentDirectory = new CurrentDirectory();
            currentDirectory.MoveUpToDirectory(name);
            currentDirectory.Name.ShouldEqual(name);
        }

        [Test]
        public void MoveUpToDirectory_Should_Set_New_Directory_By_Level()
        {
            const string name = "Honcho.Tests";
            var currentDirectory = new CurrentDirectory();
            currentDirectory.MoveUpToDirectory(2);
            currentDirectory.Name.ShouldEqual(name);
        }

        [Test]
        public void MoveDownToDirectory_Should_Set_New_Directory_By_Like_Name()
        {
            var currentDirectory = new CurrentDirectory();
            var name = currentDirectory.Name;
            currentDirectory.MoveUpToDirectory();
            currentDirectory.MoveDownToDirectory(name.Substring(0, name.Length/2));
            currentDirectory.Name.ShouldEqual(name);
        }
    }
}
