using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTConnectAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTConnectAgent.BLL.Tests
{
    [TestClass]
    public class MTConnectClientTestsIntegration
    {
        const string agentUrl = "http://mtconnect.mazakcorp.com:5701/";

        const string agentUrl2 = "https://smstestbed.nist.gov/vds/";

        public MTConnectClient MTConnectClient { get; private set; }

        public XDocument XDocument { get; private set; }

        public ITag Root { get; private set; }

        [TestInitialize]
        public void Init()
        {
            MTConnectClient = new MTConnectClient();
            XDocument = MTConnectClient.getProbeAsync(agentUrl).Result;
            Root = MTConnectClient.ParseXMLRecursif(XDocument.Root);
        }


        [TestMethod]
        public void RechercheDeviceAvecIdValide()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            string resultatAttendu = url + "/current?path=//Device[@id=\"d1\"]";
            string resultatObtenu;
            ITag tagDeRecherche, tagDepart;
            Queue<string> identifiantTagQueue = new Queue<string>();
            string id = "d1";
            identifiantTagQueue.Enqueue(id);

            //Act
            tagDepart = MTConnectClient.FindTagById(Root,id);
            tagDeRecherche = MTConnectClient.CreateSpecifiqueTag(tagDepart, identifiantTagQueue);
            resultatObtenu = MTConnectClient.GenererPath(tagDeRecherche, url, false);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }

        [TestMethod]
        public void RechercheDeviceAvecIdInvalide()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            string resultatAttendu = "Impossible de générer le path";
            string resultatObtenu;
            ITag tagDeRecherche, tagDepart;
            Queue<string> identifiantTagQueue = new Queue<string>();
            string id = "idDeTest";
            identifiantTagQueue.Enqueue(id);

            //Act
            tagDepart = MTConnectClient.FindTagById(Root,id);
            tagDeRecherche = MTConnectClient.CreateSpecifiqueTag(tagDepart, identifiantTagQueue);
            resultatObtenu = MTConnectClient.GenererPath(tagDeRecherche, url, false);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }

        [TestMethod]
        public void RechercheDeviceSansId()
        {
            //Arrange
            string url = "https://smstestbed.nist.gov/vds";
            string resultatAttendu = url + "/current?path=//Device[@id=\"d1\"]";
            string resultatObtenu;
            ITag tagDeRecherche, tagDepart;
            Queue<string> identifiantTagQueue = new Queue<string>();
            string name = "Device";
            identifiantTagQueue.Enqueue(name);

            //Act
            tagDepart = MTConnectClient.FindTagByName(Root,name);
            tagDeRecherche = MTConnectClient.CreateSpecifiqueTag(tagDepart, identifiantTagQueue);
            resultatObtenu = MTConnectClient.GenererPath(tagDeRecherche,url, false);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }


    }
}
