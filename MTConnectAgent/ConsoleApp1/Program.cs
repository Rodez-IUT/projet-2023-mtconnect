using MTConnectAgent.BLL;
using MTConnectAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            MTConnectClient mTConnectClient = new MTConnectClient();
            const string agentUrl = "http://mtconnect.mazakcorp.com:5701/";

            XDocument x = mTConnectClient.getProbeAsync(agentUrl).Result;

            ITag root = mTConnectClient.ParseXMLRecursif(x.Root);

            Queue<string> idTagQueue = new Queue<string>();
            Queue<string> nomTagQueue = new Queue<string>();

            idTagQueue.Enqueue("avail");
            nomTagQueue.Enqueue("");

            ITag tag = mTConnectClient.CreateSpecifiqueTag(root, idTagQueue, nomTagQueue);

            string path = mTConnectClient.GenererPath(tag,agentUrl);

            Console.WriteLine(path);
            
            while (true)
            {

            }
        }
    }
}
