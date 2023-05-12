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
    [TestClass()]
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
        public void SiDeviceAvecIdMazak03DemandeAlorsPathDonneDeviceAvecIdMazak03()
        {
            //Arrange
            string resultatAttendu = "https://smstestbed.nist.gov/vds/current?path=//Device[@id=\"Mazak03\"]";
            ITag device = new Tag("Device", "Mazak03");

            //Act
            string resultatObtenu = mtConnectClient.GenererPath(device);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);

        }

        [TestMethod]
        public void SiDataItemDemandeAlorsCheminCompletDataItemRendu()
        {
            //Arrange
            string resultatAttendu = "https://smstestbed.nist.gov/vds/current?path=//Device[@id=\"Mazak03\"]//Components//Axes//DataItems//DataItem[@id=\"Mazak03-S_6\"]";
            ITag dataItem = new Tag("DataItem", "Mazak03-S_6");
            ITag dataItems = new Tag("DataItems");
            dataItems.AddChild(dataItem);
            ITag axes = new Tag("Axes");
            axes.AddChild(dataItems);
            ITag components = new Tag("Components");
            components.AddChild(axes);
            ITag device = new Tag("Device", "Mazak03");
            device.AddChild(components);

            //Act
            string resultatObtenu = mtConnectClient.GenererPath(device);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }
    }
}