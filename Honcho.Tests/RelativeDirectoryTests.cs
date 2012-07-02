using System;
using System.IO;
using Honcho.Utils;
using NUnit.Framework;
using Should;

namespace Honcho.Tests
{
    [TestFixture]
    public class RelativeDirectoryTests : TestBase
    {
        [Test]
        public void Constructor_Should_Set_CurrentDirectory()
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            new RelativeDirectory(currentDirectory.FullName).Name.ShouldEqual(currentDirectory.Name);
        }
    }
}