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
        public void FindTagByIdTest()
        {
            // Arrange
            Tag dataItem = new Tag("DataItem", "Mazak03-S_6");
            Tag dataItems = new Tag("DataItems");
            dataItems.AddChild(dataItem);
            Tag axes = new Tag("Axes");
            axes.AddChild(dataItems);
            Tag components = new Tag("Components");
            components.AddChild(axes);
            Tag device = new Tag("Device", "Mazak03");
            device.AddChild(components);

            // Act
            Tag tagSpecifique = mtConnectClient.FindTagById(device, "Mazak03-S_6");

            // Assert
            Assert.IsNotNull(tagSpecifique);
            Assert.AreEqual("Mazak03-S_6", tagSpecifique.Id);
            Assert.AreEqual("DataItem", tagSpecifique.Name);
        }

        [TestMethod]
        public void FindTagByNameTest()
        {
            // Arrange
            Tag dataItem = new Tag("DataItem", "Mazak03-S_6");
            Tag dataItems = new Tag("DataItems");
            dataItems.AddChild(dataItem);
            Tag axes = new Tag("Axes");
            axes.AddChild(dataItems);
            Tag components = new Tag("Components");
            components.AddChild(axes);
            Tag device = new Tag("Device", "Mazak03");
            device.AddChild(components);

            // Act
            Tag tagSpecifique = mtConnectClient.FindTagByName(device, "DataItem");

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
            Tag devices = new Tag("Devices");
            Tag device1 = new Tag("Device", "GFAgie01");
            Tag device2 = new Tag("Device", "Mazak01");
            Tag dataItems = new Tag("DataItems");
            Tag dataItem = new Tag("DataItem", "Mazak01-dtop_1");
            dataItems.AddChild(dataItem);
            device2.AddChild(dataItems);
            devices.AddChild(device1);
            devices.AddChild(device2);

            //Act
            //List<string> resultatObtenu = mtConnectClient.GenererPath(devices, url, false);

            //Assert
            for (int i = 0; i < resultatAttendu.Count; i++)
            {
               // Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }

        [TestMethod]
        public void SiOrActiveEtOrImpossibleAlorsListeStringRetourne()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            List<string> resultatAttendu = new List<string>()
            {
                "https://smstestbed.nist.gov/vds/current?path=//Devices//Device[@id=\"GFAgie01\"]//DataItems//DataItem[@id=\"GFAgie01-dtop_1\" or @id=\"GFAgie01-dtop_2\"]",
                "https://smstestbed.nist.gov/vds/current?path=//Devices//Device[@id=\"Mazak01\"]//DataItems//DataItem[@id=\"Mazak01-dtop_1\"]"
            };
            Tag dataItem1 = new Tag("DataItem", "GFAgie01-dtop_1");
            Tag dataItem2 = new Tag("DataItem", "GFAgie01-dtop_2");
            Tag dataItem3 = new Tag("DataItem", "Mazak01-dtop_1");
            Tag dataItems1 = new Tag("DataItems");
            Tag dataItems2 = new Tag("DataItems");
            Tag device1 = new Tag("Device", "GFAgie01");
            Tag device2 = new Tag("Device", "Mazak01");
            Tag devices = new Tag("Devices");
            dataItems1.AddChild(dataItem1);
            dataItems1.AddChild(dataItem2);
            dataItems2.AddChild(dataItem3);
            device1.AddChild(dataItems1);
            device2.AddChild(dataItems2);
            devices.AddChild(device1);
            devices.AddChild(device2);

            //Act
            //List<string> resultatObtenu = mtConnectClient.GenererPath(devices, url, true);

            //Assert
            for(int i = 0; i < resultatAttendu.Count; i++)
            {
              //  Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }

        [TestMethod]
        public void SiPremierTagDemandeAlorsListeStringRetourne()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            List<string> resultatAttendu = new List<string>()
            {
                "https://smstestbed.nist.gov/vds/current?path=//Device[@id=\"GFAgie01\"]"
            };
            Tag device = new Tag("Device", "GFAgie01");

            //Act
            //List<string> resultatObtenu = mtConnectClient.GenererPath(device, url, true);

            //Assert
            for (int i = 0; i < resultatAttendu.Count; i++)
            {
               // Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }

        [TestMethod]
        public void SiTagAvantOrPossiblePossedeIdAlorsListeStringGeneree()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            List<string> resultatAttendu = new List<string>()
            {
                "https://smstestbed.nist.gov/vds/current?path=//Devices[@id=\"test\"]//Device[@id=\"GFAgie01\"]"
            };
            Tag device = new Tag("Device", "GFAgie01");
            Tag devices = new Tag("Devices", "test");
            devices.AddChild(device);

            //Act
            //List<string> resultatObtenu = mtConnectClient.GenererPath(devices, url, true);

            //Assert
            for (int i = 0; i < resultatAttendu.Count; i++)
            {
              //  Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }

        [TestMethod]
        public void SiOrActifMaisIdVideAlorsOrEstInactif()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            List<string> resultatAttendu = new List<string>()
            {
                "https://smstestbed.nist.gov/vds/current?path=//DataItems"
            };
            Tag dataItems = new Tag("DataItems");

            //Act
            //List<string> resultatObtenu = mtConnectClient.GenererPath(dataItems, url, true);

            //Assert
            for (int i = 0; i < resultatAttendu.Count; i++)
            {
                //Assert.AreEqual(resultatAttendu[i], resultatObtenu[i]);
            }
        }
    }
}