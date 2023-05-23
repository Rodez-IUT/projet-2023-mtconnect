using MTConnectAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTConnectAgent.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class MTConnectClient
    {

        private List<string> paths;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getProbeAsync(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url + "probe");
                response.EnsureSuccessStatusCode();

                var stream = response.Content.ReadAsStreamAsync().Result;

                XDocument document = XDocument.Load(stream);

                return document;
            }
            catch (AggregateException ex)
            {
                throw new ArgumentException("L'uri donnée n'est pas valide", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getCurrentAsync(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(url + "current");
                response.EnsureSuccessStatusCode();

                var stream = response.Content.ReadAsStreamAsync().Result;

                XDocument document = XDocument.Load(stream);

                return document;
            }
            catch (AggregateException ex)
            {
                throw new ArgumentException("L'uri donnée n'est pas valide", ex);
            }

        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getAssetsAsync(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(url + "assets");
                response.EnsureSuccessStatusCode();

                var stream = response.Content.ReadAsStreamAsync().Result;

                XDocument document = XDocument.Load(stream);

                return document;
            }
            catch (AggregateException ex)
            {
                throw new ArgumentException("L'uri donnée n'est pas valide", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getSampleAsync(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(url + "sample");
                response.EnsureSuccessStatusCode();

                var stream = response.Content.ReadAsStreamAsync().Result;

                XDocument document = XDocument.Load(stream);

                return document;
            }
            catch (AggregateException ex)
            {
                throw new ArgumentException("L'uri donnée n'est pas valide", ex);
            }
        }

        /// <summary>
        /// Création d'un tag qui posède une forme spécifique pour la génération des path
        /// </summary>
        /// <param name="root">Tag racine qui est le point de départ du path (Souvent un Device)</param>
        /// <param name="identifiantTagQueue">File d'id et de name de tag, id peut avoir la valeur "" pour représenter l'absence d'id pour un tag</param>
        /// <returns></returns>
        public ITag CreateSpecifiqueTag(ITag root, Queue<string> identifiantTagQueue)
        {
            if (root == null)
            {
                return null;
            }
            if (identifiantTagQueue.Count > 0)
            {
                if (!identifiantTagQueue.Peek().Equals("") && (root.Id.Equals(identifiantTagQueue.Peek()) || root.Name.Equals(identifiantTagQueue.Peek())))
                {
                    identifiantTagQueue.Dequeue();
                    foreach (ITag tagChild in root.Child)
                    {
                        var result = CreateSpecifiqueTag(tagChild, identifiantTagQueue);
                        if (result != null)
                        {
                            root.ClearChild();
                            root.AddChild(result);
                            return root;
                        }
                    }

                    root.ClearChild();
                    return root;
                }

                foreach (ITag tagChild in root.Child)
                {
                    var result = CreateSpecifiqueTag(tagChild, identifiantTagQueue);
                    if (result != null)
                    {
                        root.ClearChild();
                        root.AddChild(result);
                        return root;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Création d'un tag qui posède une forme spécifique pour la génération des path
        /// </summary>
        /// <param name="root">Tag racine qui est le point de départ du path (Souvent un Device)</param>
        /// <param name="idTagQueue">File d'id de tag, id peut avoir la valeur "" pour représenter l'absence d'id pour un tag</param>
        /// <param name="nomTagQueue">File de nom de tag, le nom peut avoir la valeur "" pour représenter l'absence de nom pour un tag</param>
        /// <returns></returns>
        public ITag CreateSpecifiqueTagOR(ITag root, Queue<string> identifiantTagQueue)
        {
            if (root == null)
            {
                return null;
            }
            if (identifiantTagQueue.Count > 0)
            {
                List<ITag> childList = new List<ITag>();
                if (!identifiantTagQueue.Peek().Equals("") && (root.Id.Equals(identifiantTagQueue.Peek()) || root.Name.Equals(identifiantTagQueue.Peek())))
                {
                    identifiantTagQueue.Dequeue();
                    foreach (ITag tagChild in root.Child)
                    {
                        var result = CreateSpecifiqueTagOR(tagChild, identifiantTagQueue);
                        if (result != null)
                        {
                            childList.Add(result);
                        }
                    }

                    root.SetChild(childList);
                    return root;
                }

                foreach (ITag tagChild in root.Child)
                {
                    var result = CreateSpecifiqueTagOR(tagChild, identifiantTagQueue);
                    if (result != null)
                    {
                        childList.Add(result);
                    }
                }
                if (childList.Count > 0)
                {
                    root.SetChild(childList);
                    return root;
                }
            }
            return null;
        }

        /// <summary>1
        /// 
        /// Recherche d'un tag spécifique dans tout les tag enfant de celui passer en paramètre
        /// </summary>
        /// <param name="tag">Tag dans lequel vas être effectué la recherche</param>
        /// <param name="name">Nom du tag recherché</param>
        /// <returns>Renvoi le tag qui correspond au critère de recherche sinon null</returns>
        public ITag FindTagByName(ITag tag, string name)
        {
            ITag result = null;
            if (tag.Name.Equals(name))
            {
                return tag;
            }

            if (tag.Child.Count > 0)
            {
                foreach (Tag tagChild in tag.Child)
                {
                    result = FindTagByName(tagChild, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Recherche d'un tag spécifique dans tout les tag enfant de celui passer en paramètre
        /// </summary>
        /// <param name="tag">Tag dans lequel vas être effectué la recherche</param>
        /// <param name="id">Id du tag recherché</param>
        /// <returns>Renvoi le tag qui correspond au critère de recherche sinon null</returns>
        public ITag FindTagById(ITag tag, string id)
        {
            ITag result = null;
            if (tag.Id.Equals(id))
            {
                return tag;
            }

            if (tag.Child.Count > 0)
            {
                foreach (Tag tagChild in tag.Child)
                {
                    result = FindTagById(tagChild, id);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Initialise la génération du path récursive
        /// </summary>
        /// <param name="tag">Noeud racine depuis lequel nous allons générer le path</param>
        /// <returns>Le path généré</returns>
        public List<string> GenererPath(ITag tag, string urlMachine, bool isOrActivated)
        {
            if (tag == null)
            {
                // todo
                //return "Impossible de générer le path";
            }
            while (urlMachine.EndsWith("/"))
            {
                urlMachine = urlMachine.Remove(urlMachine.Length - 1);
            }
            StringBuilder urlParente = new StringBuilder();
            paths = new List<string>();
            if (isOrActivated)
            {
                GenererPathAvecOr(tag, urlParente);
            }
            else
            {
                GenererPathSansOr(tag, urlParente);
            }
            return paths;
        }
        private void GenererPathSansOr(ITag tag, StringBuilder urlParent)
        {
            urlParent.Append("//");
            urlParent.Append(tag.Name);
            if (!tag.Id.Equals(""))
            {
                urlParent.Append("[@id=\"");
                urlParent.Append(tag.Id);
                urlParent.Append("\"]");
            }
            if (!tag.HasChild())
            {
                paths.Add(urlParent.ToString());
            }
            else
            {
                foreach (ITag tagEnfant in tag.Child)
                {
                    GenererPathSansOr(tagEnfant, urlParent);
                }
            }
            
        }

        private void GenererPathAvecOr(ITag tag, StringBuilder urlParent)
        {
            urlParent.Append("//");
            urlParent.Append(tag.Name);
            bool orPossible = true;
            if (!tag.HasChild())
            {
                paths.Add(urlParent.ToString());
            }
            else
            {
                foreach (ITag tagEnfant in tag.Child)
                {
                    if (tagEnfant.HasChild())
                    {
                        orPossible = false;
                    }
                }
                if (orPossible)
                {
                    urlParent.Append("[@id="),
                    bool isFirst = true;
                    foreach (ITag tagEnfant in tag.Child)
                    {
                        if (!isFirst)
                        {
                            urlParent.Append(" or ");
                        }
                        urlParent.Append("\"");
                        urlParent.Append(tagEnfant.Id);
                        urlParent.Append("\"");
                        isFirst = false;
                    }
                    urlParent.Append("]");
                    paths.Add(urlParent.ToString());
                }
                else
                {
                    foreach (ITag tagEnfant in tag.Child)
                    {
                        GenererPathAvecOr(tagEnfant, urlParent);
                    }
                }
            }
        }
    }
}