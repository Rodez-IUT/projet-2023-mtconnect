using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTConnectAgent.BLL;
using MTConnectAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTConnectAgent.BLL.Tests
{
    /// <summary>
    /// Test des méthodes de la classe MTConnectClient
    /// </summary>
    [TestClass]
    public class MTConnectClientTests
    {
        private MTConnectClient mtConnectClient;
        string agentUrl = "http://mtconnect.mazakcorp.com:5701/";
        string agentUrl2 = "https://smstestbed.nist.gov/vds/";

        /// <summary>
        /// Initialisation de la variable 
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            mtConnectClient = new MTConnectClient();
        }

        [TestMethod()]
        public void ParseXMLRecursifTest()
        {
            // Arrange
            XDocument document = mtConnectClient.getProbeAsync(agentUrl2).Result;

            // Act
            ITag root = mtConnectClient.ParseXMLRecursif(document.Root);

            // Assert
            Assert.IsNotNull(root);
        }

        [TestMethod()]
        public void GetProbeAsyncTest()
        {
            // Arrange & Act
            XDocument document = mtConnectClient.getProbeAsync(agentUrl2).Result;

            // Assert
            Assert.IsNotNull(document);
        }

        [TestMethod()]
        public void GetCurrentAsyncTest()
        {
            // Arrange & Act
            XDocument document = mtConnectClient.getCurrentAsync(agentUrl2).Result;

            // Assert
            Assert.IsNotNull(document);
        }

        [TestMethod()]
        public void GetAssetsAsyncTest()
        {
            // Arrange & Act
            XDocument document = mtConnectClient.getAssetsAsync(agentUrl2).Result;

            // Assert
            Assert.IsNotNull(document);
        }

        [TestMethod()]
        public void GetSampleAsyncTest()
        {
            // Arrange & Act
            XDocument document = mtConnectClient.getSampleAsync(agentUrl2).Result;

            // Assert
            Assert.IsNotNull(document);
        }

        [TestMethod]
        public void CreateSpecifiqueTagTest()
        {
            // Arrange 
            ITag dataItem = new Tag("DataItem", "Mazak03-S_6");
            ITag dataItems = new Tag("DataItems");
            dataItems.AddChild(dataItem);
            ITag axes = new Tag("Axes");
            axes.AddChild(dataItems);
            ITag components = new Tag("Components");
            components.AddChild(axes);
            ITag device = new Tag("Device", "Mazak03");
            device.AddChild(components);

            Queue<string> identifiantQueue = new Queue<string>();
            string id = "Mazak03";
            identifiantQueue.Enqueue(id);

            identifiantQueue.Enqueue("Components");
            identifiantQueue.Enqueue("Axes");
            identifiantQueue.Enqueue("DataItems");
            identifiantQueue.Enqueue("Mazak03-S_6");



            // Act
            ITag tagSpecifique = mtConnectClient.CreateSpecifiqueTag(device, identifiantQueue);

            // Assert
            Assert.IsNotNull(tagSpecifique);
            Assert.AreEqual(device.Id, tagSpecifique.Id);
            Assert.AreEqual("Mazak03-S_6", device.Child[0].Child[0].Child[0].Child[0].Id);
        }

        [TestMethod]
        public void FindTagByIdTest()
        {
            // Arrange
            ITag dataItem = new Tag("DataItem", "Mazak03-S_6");
            ITag dataItems = new Tag("DataItems");
            dataItems.AddChild(dataItem);
            ITag axes = new Tag("Axes");
            axes.AddChild(dataItems);
            ITag components = new Tag("Components");
            components.AddChild(axes);
            ITag device = new Tag("Device", "Mazak03");
            device.AddChild(components);

            // Act
            ITag tagSpecifique = mtConnectClient.FindTagById(device, "Mazak03-S_6");

            // Assert
            Assert.IsNotNull(tagSpecifique);
            Assert.AreEqual("Mazak03-S_6", tagSpecifique.Id);
            Assert.AreEqual("DataItem", tagSpecifique.Name);
        }

        [TestMethod]
        public void FindTagByNameTest()
        {
            // Arrange
            ITag dataItem = new Tag("DataItem", "Mazak03-S_6");
            ITag dataItems = new Tag("DataItems");
            dataItems.AddChild(dataItem);
            ITag axes = new Tag("Axes");
            axes.AddChild(dataItems);
            ITag components = new Tag("Components");
            components.AddChild(axes);
            ITag device = new Tag("Device", "Mazak03");
            device.AddChild(components);

            // Act
            ITag tagSpecifique = mtConnectClient.FindTagByName(device, "DataItem");

            // Assert
            Assert.IsNotNull(tagSpecifique);
            Assert.AreEqual("Mazak03-S_6", tagSpecifique.Id);
            Assert.AreEqual("DataItem", tagSpecifique.Name);
        }

        [TestMethod]
        public void SiOrInactifAlorsPathRetourne()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            List<string> resultatAttendu = new List<string>()
            {
                "https://smstestbed.nist.gov/vds/current?path=//Devices//Device[@id=\"GFAgie01\"]",
                "https://smstestbed.nist.gov/vds/current?path=//Devices//Device[@id=\"Mazak01\"]//DataItems//DataItem[@id=\"Mazak01-dtop_1\"]"
            };
            ITag devices = new Tag("Devices");
            ITag device1 = new Tag("Device", "GFAgie01");
            ITag device2 = new Tag("Device", "Mazak01");
            ITag dataItems = new Tag("DataItems");
            ITag dataItem = new Tag("DataItem", "Mazak01-dtop_1");
            dataItems.AddChild(dataItem);
            device2.AddChild(dataItems);
            devices.AddChild(device1);
            devices.AddChild(device2);

            //Act
            List<string> resultatObtenu = mtConnectClient.GenererPath(devices, url, false);

            //Assert
            for (int i = 0; i <= resultatAttendu.Count; i++)
            {
                Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }

        [TestMethod]
        public void SiOrActiveEtOrImpossibleAlorsListeStringRetourne()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            List<string> resultatAttendu = new List<string>()
            {
                "//Device[@id=\"GFAgie01\"]//DataItems//DataItem[@id=\"GFAgie01-dtop_1\" or @id=\"GFAgie01-dtop_2\"]",
                "//Device[@id=\"Mazak01\"]//DataItems//DataItem[@id=\"Mazak01-dtop_1\"]"
            };
            ITag dataItem1 = new Tag("DataItem", "GFAgie01-dtop_1");
            ITag dataItem2 = new Tag("DataItem", "GFAgie01-dtop_2");
            ITag dataItem3 = new Tag("DataItem", "Mazak01-dtop_1");
            ITag dataItems1 = new Tag("DataItems");
            ITag dataItems2 = new Tag("DataItems");
            ITag device1 = new Tag("Device", "GFAgie01");
            ITag device2 = new Tag("Device", "Mazak01");
            ITag devices = new Tag("Devices");
            dataItems1.AddChild(dataItem1);
            dataItems1.AddChild(dataItem2);
            dataItems2.AddChild(dataItem3);
            device1.AddChild(dataItems1);
            device2.AddChild(dataItems2);
            devices.AddChild(device1);
            devices.AddChild(device2);

            //Act
            List<string> resultatObtenu = mtConnectClient.GenererPath(devices, url, true);

            //Assert
            for(int i = 0; i <= resultatAttendu.Count; i++)
            {
                Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }
    }
}