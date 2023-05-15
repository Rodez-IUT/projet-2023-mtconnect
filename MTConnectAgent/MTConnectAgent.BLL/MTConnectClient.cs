﻿using MTConnectAgent.Model;
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
        /// <summary>
        /// Transformation d'un object de type <see cref="XElement"/> en objet <see cref="ITag"/> de manière récursive 
        /// </summary>
        /// <param name="element">element à parser</param>
        /// <returns>un <see cref="ITag"/> qui represent un fichier Xml</returns>
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
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "probe");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getCurrentAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "current");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;

        }

        /// <summary>
        /// Initialise la génération du path récursive
        /// </summary>
        /// <param name="tag">Noeud racine depuis lequel nous allons générer le path</param>
        /// <returns>Le path généré</returns>
        public string GenererPath(ITag tag)
        {
            if (tag == null)
            {
                return "Impossible de générer le path";
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("http://mtconnect.mazakcorp.com:5701/current?path=");
            stringBuilder = GenererPath(tag, stringBuilder);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Génére un path de façon récursive
        /// Si le tag contient une id non vide, alors elle est ajoutée en paramètre au path
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="stringBuilder"></param>
        /// <returns> Le stringBuilder d'entrée auquel on à concaténé le path</returns>
        private StringBuilder GenererPath(ITag tag, StringBuilder stringBuilder)
        {
            stringBuilder.Append("//");
            stringBuilder.Append(tag.Name);
            if (!tag.Id.Equals(""))
            {
                stringBuilder.Append("[@id=\"");
                stringBuilder.Append(tag.Id);
                stringBuilder.Append("\"]");
            }
            if (tag.HasChild())
            {
                stringBuilder = GenererPath(tag.Child[0], stringBuilder);
            }
            return stringBuilder;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getAssetsAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "assets");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<XDocument> getSampleAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url + "sample");
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            XDocument document = XDocument.Load(stream);

            return document;
        }

        /// <summary>
        /// Création d'un tag qui posède une forme spécifique pour la génération des path
        /// </summary>
        /// <param name="root">Tag racine qui est le point de départ du path (Souvent un Device)</param>
        /// <param name="idTagQueue">File d'id de tag, id peut avoir la valeur "" pour représenter l'absence d'id pour un tag</param>
        /// <param name="nomTagQueue">File de nom de tag, le nom peut avoir la valeur "" pour représenter l'absence de nom pour un tag</param>
        /// <returns></returns>
        public ITag CreateSpecifiqueTag(ITag root, Queue<string> idTagQueue, Queue<string> nomTagQueue)
        {
            if (root == null)
            {
                return null;
            }
            if (idTagQueue.Count > 0 && nomTagQueue.Count > 0)
            {
                if (!idTagQueue.Peek().Equals("") && root.Id.Equals(idTagQueue.Peek()))
                {
                    idTagQueue.Dequeue();
                    nomTagQueue.Dequeue();
                    foreach (ITag tagChild in root.Child)
                    {
                        var result = CreateSpecifiqueTag(tagChild, idTagQueue, nomTagQueue);
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
                else if (!nomTagQueue.Peek().Equals("") && root.Name.Equals(nomTagQueue.Peek()))
                {
                    idTagQueue.Dequeue();
                    nomTagQueue.Dequeue();
                    root.Id = "";
                    foreach (ITag tagChild in root.Child)
                    {
                        var result = CreateSpecifiqueTag(tagChild, idTagQueue, nomTagQueue);
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
                    var result = CreateSpecifiqueTag(tagChild, idTagQueue, nomTagQueue);
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
    }
}
