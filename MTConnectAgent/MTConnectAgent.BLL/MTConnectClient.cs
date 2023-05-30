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
        public Tag ParseXMLRecursif(XElement element)
        {
            Tag tag = new Tag(element.Name.ToString().Split('}')[1]);

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
        /// Génére un path de façon récursive
        /// Si le tag contient une id non vide, alors elle est ajoutée en paramètre au path
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="stringBuilder"></param>
        /// <returns> Le stringBuilder d'entrée auquel on à concaténé le path</returns>
        private StringBuilder GenererPath(Tag tag, StringBuilder stringBuilder, bool isOrActivated)
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
                if (tag.Child.Count >= 2 && isOrActivated)
                {
                    stringBuilder = GenererPathWithOr(tag.Child[0], stringBuilder, true);
                    for (int i = 1; i < tag.Child.Count; i++)
                    {
                        stringBuilder = GenererPathWithOr(tag.Child[i], stringBuilder, false);
                    }
                    stringBuilder.Append("]");
                } else
                { 
                    stringBuilder = GenererPath(tag.Child[0], stringBuilder,isOrActivated);
                }
            }
            return stringBuilder;
        }

        private StringBuilder GenererPathWithOr(ITag tag, StringBuilder stringBuilder, bool isFirst)
        {
            if (isFirst)
            {
                stringBuilder.Append("//");
                stringBuilder.Append(tag.Name);
                stringBuilder.Append("[");
            }
            else
            {
                stringBuilder.Append(" or ");
            }
            stringBuilder.Append("@id=\"");
            stringBuilder.Append(tag.Id);
            stringBuilder.Append("\"");
            return stringBuilder;
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

        /// <summary>1
        /// 
        /// Recherche d'un tag spécifique dans tout les tag enfant de celui passer en paramètre
        /// </summary>
        /// <param name="tag">Tag dans lequel vas être effectué la recherche</param>
        /// <param name="name">Nom du tag recherché</param>
        /// <returns>Renvoi le tag qui correspond au critère de recherche sinon null</returns>
        public Tag FindTagByName(Tag tag, string name)
        {
            Tag result = null;
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
        /// <returns>Renvoi le nom du tag correspondant sinon null</returns>
        public string FindTagNameById(Tag tag, string id)
        {
            string result = null;
            if (tag.Id.Equals(id))
            {
                return tag.Name;
            }

            if (tag.Child.Count > 0)
            {
                foreach (Tag tagChild in tag.Child)
                {
                    result = FindTagNameById(tagChild, id);
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
        /// <returns>Renvoi le nom du tag correspondant sinon null</returns>
        public Tag FindTagById(Tag tag, string id)
        {
            Tag result = null;
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
        /// Recherche d'un tag spécifique dans tout les tag enfant de celui passer en paramètre
        /// </summary>
        /// <param name="tag">Tag dans lequel vas être effectué la recherche</param>
        /// <param name="id">Id du tag recherché</param>
        /// <returns>Renvoi le nom du tag correspondant sinon null</returns>
        public Tag FindRefTagById(Tag tag, string id)
        {
            Tag result = null;
            if (tag.Id.Equals(id))
            {
                return tag;
            }

            if (tag.Child.Count > 0)
            {
                foreach (Tag tagChild in tag.Child)
                {
                    result = FindRefTagById(tagChild, id);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Initialise et lance la génération des paths
        /// </summary>
        /// <param name="tag">Noeud racine depuis lequel nous allons générer le path, doit venir de la méthode CreateSpecifiqueTag</param>
        /// <returns>La liste des paths générés</returns>
        public List<string> GenererPath(Tag tag, string urlMachine, bool isOrActivated)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag", "Le tag ne peut pas être null");
            }
            while (urlMachine.EndsWith("/"))
            {
                urlMachine = urlMachine.Remove(urlMachine.Length - 1);
            }
            StringBuilder urlParente = new StringBuilder();
            urlParente.Append(urlMachine);
            urlParente.Append("/current?path=");
            paths = new List<string>();
            if (isOrActivated)
            {
                GenererPathAvecOr(tag, urlParente.ToString());
            }
            else
            {
                GenererPathSansOr(tag, urlParente.ToString());
            }
            return paths;
        }

        /// <summary>
        /// Génére les paths de façon récursive si l'option or est désactivée
        /// </summary>
        /// <param name="tag">Le tag courant de la génération du path courant</param>
        /// <param name="urlParente">Url courante de la génération du path courant</param>
        private void GenererPathSansOr(Tag tag, string urlParente)
        {
            StringBuilder urlCourante = new StringBuilder();
            // Ajout du nom du tag au path courant
            urlCourante.Append(urlParente);
            urlCourante.Append("//");
            urlCourante.Append(tag.Name);
            // Ajout de l'id s'il existe
            if (!tag.Id.Equals(""))
            {
                urlCourante.Append("[@id=\"");
                urlCourante.Append(tag.Id);
                urlCourante.Append("\"]");
            }
            // Si il n'y a pas d'enfant on ajoute la path courant à la liste des paths
            if (!tag.HasChild())
            {
                paths.Add(urlCourante.ToString());
            }
            else
            {
                foreach (Tag tagEnfant in tag.Child)
                {
                    GenererPathSansOr(tagEnfant, urlCourante.ToString());
                }
            }
            
        }

        /// <summary>
        /// Génére les paths de façon récursive si l'option or est activée
        /// </summary>
        /// <param name="tag">Le tag courant de la génération du path courant</param>
        /// <param name="urlParente">Url courante de la génération du path courant</param>
        private void GenererPathAvecOr(Tag tag, string urlParente)
        {
            StringBuilder urlCourante = new StringBuilder();
            // Ajout du nom du tag au path courant
            urlCourante.Append(urlParente.ToString());
            urlCourante.Append("//");
            urlCourante.Append(tag.Name);
            bool orPossible = true;
            // Ajout de l'id s'il existe
            if (!tag.Id.Equals(""))
            {
                urlCourante.Append("[@id=\"");
                urlCourante.Append(tag.Id);
                urlCourante.Append("\"]");
            }
            // Si il n'y a pas d'enfant, on ajout le path
            if (!tag.HasChild())
            {
                paths.Add(urlCourante.ToString());
            }
            else
            {
                // On vérifie si l'utilisation du or est possible
                foreach (Tag tagEnfant in tag.Child)
                {
                    if (tagEnfant.HasChild() || tagEnfant.Id.Equals(""))
                    {
                        orPossible = false;
                    }
                }
                if (orPossible)
                {
                    // On ajoute toute les id au même path
                    urlCourante.Append("//");
                    urlCourante.Append(tag.Child[0].Name);
                    urlCourante.Append("[@id=");
                    bool isFirst = true;
                    foreach (Tag tagEnfant in tag.Child)
                    {
                        if (!isFirst)
                        {
                            urlCourante.Append(" or @id=");
                        }
                        urlCourante.Append("\"");
                        urlCourante.Append(tagEnfant.Id);
                        urlCourante.Append("\"");
                        isFirst = false;
                    }
                    urlCourante.Append("]");
                    paths.Add(urlCourante.ToString());
                }
                else
                { 
                    foreach (Tag tagEnfant in tag.Child)
                    {
                        GenererPathAvecOr(tagEnfant, urlCourante.ToString());
                    }
                }
            }
        }
    }
}