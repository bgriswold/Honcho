using System;
using System.IO;
using System.Xml;

namespace Honcho.Tasks
{
    public class EntityFrameworkTasks
    {
        public static void UpdateProviderManifestToken(string edmxFilename, string version = "2005")
        {
            new FileInfo(edmxFilename) {IsReadOnly = false};

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(edmxFilename);

            var mgr = new XmlNamespaceManager(xmlDoc.NameTable);
            mgr.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2008/10/edmx");
            mgr.AddNamespace("ssdl", "http://schemas.microsoft.com/ado/2009/02/edm/ssdl");

            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/edmx:Edmx/edmx:Runtime/edmx:StorageModels/ssdl:Schema", mgr);
            if (node == null)
            {
                Console.WriteLine("Could not find Schema node");
            }
            else
            {
                Console.WriteLine("Setting EDMX version to {0} in file {1}", version, edmxFilename);
                node.Attributes["ProviderManifestToken"].Value = version;
                xmlDoc.Save(edmxFilename);
            }
        }
    }
}