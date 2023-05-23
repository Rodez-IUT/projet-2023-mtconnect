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
    }
}
