using MTConnectAgent.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            string resultatAttendu = "https://smstestbed.nist.gov/vds/current?path=//Device[@id=\"d1\"]";
            string resultatObtenu;
            ITag tagDeRecherche, tagDepart;
            Queue<string> idTagQueue = new Queue<string>();
            Queue<string> nomTagQueue = new Queue<string>();
            string id = "d1";
            idTagQueue.Enqueue(id);
            nomTagQueue.Enqueue("Device");

            //Act
            tagDepart = MTConnectClient.FindTagById(Root,id);
            tagDeRecherche = MTConnectClient.CreateSpecifiqueTag(tagDepart, idTagQueue, nomTagQueue);
            resultatObtenu = MTConnectClient.GenererPath(tagDeRecherche);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }

        [TestMethod]
        public void RechercheDeviceAvecIdInvalide()
        {
            //Arrange
            string resultatAttendu = "Impossible de générer le path";
            string resultatObtenu;
            ITag tagDeRecherche, tagDepart;
            Queue<string> idTagQueue = new Queue<string>();
            Queue<string> nomTagQueue = new Queue<string>();
            string id = "idDeTest";
            idTagQueue.Enqueue(id);
            nomTagQueue.Enqueue("Device");

            //Act
            tagDepart = MTConnectClient.FindTagById(Root,id);
            tagDeRecherche = MTConnectClient.CreateSpecifiqueTag(tagDepart, idTagQueue, nomTagQueue);
            resultatObtenu = MTConnectClient.GenererPath(tagDeRecherche);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }

        [TestMethod]
        public void RechercheDeviceSansId()
        {
            //Arrange
            string resultatAttendu = "https://smstestbed.nist.gov/vds/current?path=//Device";
            string resultatObtenu;
            ITag tagDeRecherche, tagDepart;
            Queue<string> idTagQueue = new Queue<string>();
            Queue<string> nomTagQueue = new Queue<string>();
            string id = "";
            string name = "Device";
            idTagQueue.Enqueue(id);
            nomTagQueue.Enqueue(name);

            //Act
            tagDepart = MTConnectClient.FindTagByName(Root,name);
            tagDeRecherche = MTConnectClient.CreateSpecifiqueTag(tagDepart, idTagQueue, nomTagQueue);
            resultatObtenu = MTConnectClient.GenererPath(tagDeRecherche);

            //Assert
            Assert.AreEqual(resultatAttendu, resultatObtenu);
        }


    }
}
