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
        //public ITag CreateSpecifiqueTag(ITag root, Queue<string> identifiantTagQueue)
        //{
        //    if (root == null)
        //    {
        //        return null;
        //    }
        //    if (identifiantTagQueue.Count > 0)
        //    {
        //        if (!identifiantTagQueue.Peek().Equals("") && (root.Id.Equals(identifiantTagQueue.Peek()) || root.Name.Equals(identifiantTagQueue.Peek())))
        //        {
        //            identifiantTagQueue.Dequeue();
        //            foreach (ITag tagChild in root.Child)
        //            {
        //                var result = CreateSpecifiqueTag(tagChild, identifiantTagQueue);
        //                if (result != null)
        //                {
        //                    root.ClearChild();
        //                    root.AddChild(result);
        //                    return root;
        //                }
        //            }

        //            root.ClearChild();
        //            return root;
        //        }

        //        foreach (ITag tagChild in root.Child)
        //        {
        //            var result = CreateSpecifiqueTag(tagChild, identifiantTagQueue);
        //            if (result != null)
        //            {
        //                root.ClearChild();
        //                root.AddChild(result);
        //                return root;
        //            }
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// Création d'un tag qui posède une forme spécifique pour la génération des path
        /// </summary>
        /// <param name="root">Tag racine qui est le point de départ du path (Souvent un Device)</param>
        /// <param name="identifiantTagQueue">File d'id et de nom de tag</param>
        /// <returns></returns>
        public ITag CreateSpecifiqueTag(ITag root, Queue<string> identifiantTagQueue)
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
                        var result = CreateSpecifiqueTag(tagChild, identifiantTagQueue);
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
                    var result = CreateSpecifiqueTag(tagChild, identifiantTagQueue);
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
        /// Initialise et lance la génération des paths
        /// </summary>
        /// <param name="tag">Noeud racine depuis lequel nous allons générer le path, doit venir de la méthode CreateSpecifiqueTag</param>
        /// <returns>La liste des paths générés</returns>
        public List<string> GenererPath(ITag tag, string urlMachine, bool isOrActivated)
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
        private void GenererPathSansOr(ITag tag, string urlParente)
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
                foreach (ITag tagEnfant in tag.Child)
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
        private void GenererPathAvecOr(ITag tag, string urlParente)
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
                foreach (ITag tagEnfant in tag.Child)
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
                    foreach (ITag tagEnfant in tag.Child)
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
                    foreach (ITag tagEnfant in tag.Child)
                    {
                        GenererPathAvecOr(tagEnfant, urlCourante.ToString());
                    }
                }
            }
        }
    }
}