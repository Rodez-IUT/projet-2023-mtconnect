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

            idTagQueue.Enqueue("d1");
            idTagQueue.Enqueue("avail");
            idTagQueue.Enqueue("functionalmode");

            ITag tag = mTConnectClient.CreateSpecifiqueTag(root, idTagQueue);

            Program.AfficherTag(tag, "");

            Console.WriteLine("----------------------------------------");

            List<string> paths = mTConnectClient.GenererPath(tag, "http://mtconnect.mazakcorp.com:5701", true);

            foreach (string path in paths)
            {
                Console.WriteLine(path);
            }

            Console.WriteLine("----------------------------------------\n");

            idTagQueue = new Queue<string>();

            idTagQueue.Enqueue("d1");
            idTagQueue.Enqueue("avail");

            tag = mTConnectClient.CreateSpecifiqueTag(root, idTagQueue);

            Program.AfficherTag(tag, "");

            Console.WriteLine("----------------------------------------");

            paths = mTConnectClient.GenererPath(tag, "http://mtconnect.mazakcorp.com:5701", false);

            foreach (string path in paths)
            {
                Console.WriteLine(path);
            }

            while (true)
            {

            }
        }

        public static void AfficherTag(ITag tag,string marge)
        {
            Console.WriteLine(marge + tag.Name + " : " + tag.Id);

            foreach(ITag child in tag.Child)
            {
                AfficherTag(child, marge +"     ");
            }
        }
    }
}
