using MTConnectAgent.Model;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTConnectAgent.BLL
{

    public class MTConnectClient
    {      
        public ITag ParseXMLRecursif(XElement element)
        {
            ITag tag = new Tag(element.Name.ToString().Split('}')[1]);

            if (element.Attributes().Count() != 0)
            {
                foreach (var attribute in element.Attributes().ToList())
                {
                    if (attribute.Name.ToString().Equals("id") || attribute.Name.ToString().EndsWith("Id"))
                    {
                        tag.Id = attribute.Value;
                        tag.AddAttribut(attribute.Name.ToString(), attribute.Value);
                    }
                    else
                    {
                        tag.AddAttribut(attribute.Name.ToString(), attribute.Value);
                    }
                }
            }

            if (element.Elements().Count() != 0)
            {
                foreach (var child in element.Elements())
                {
                    tag.AddChild(ParseXMLRecursif(child));
                }
            }
            else
            {
                tag.Value = element.Value;
            }
            return tag;
        }

        public async Task<XDocument> getProbeAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "probe");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;
        }

        public async Task<XDocument> getCurrentAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "current");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;

        }

        public async Task<XDocument> getAssetsAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "assets");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;
        }

        public async Task<XDocument> getSampleAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "sample");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;
        }
    }
}
