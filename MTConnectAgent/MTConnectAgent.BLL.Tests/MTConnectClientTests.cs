using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTConnectAgent.BLL;
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
    }
}