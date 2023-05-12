using System;
using System.Runtime.Serialization;

namespace MTConnectAgent.Model
{
    [Serializable()]
    public class Machine
    {

        /// <summary>
        /// Accesseur du nom de la machine
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Accesseur de l'url de la machine
        /// </summary>
        public string Url { get; private set; }

        public void setUrl(string url)
        {
            if (IsValidURL(url)) { this.Url = url; }
            else { throw new FormatException("La valeur passé n'est pas un URL"); }
        }

        /// <summary>Valide la cohérence de l'url</summary>
        /// <param name="url">Url à valider</param>
        /// <returns>True si l'url est valide False sinon</returns>
        public static bool IsValidURL(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        //Deserialization constructor.
        public Machine(SerializationInfo info, StreamingContext ctx)
        {
            this.Name = (String)info.GetValue("machineName", typeof(string));
            string url = (String)info.GetValue("machineUrl", typeof(string));

            if (IsValidURL(url)) { this.Url = url; }
            else { throw new FormatException("La valeur passé n'est pas un URL"); }
        }

        /// <summary></summary>
        /// <exception cref="FormatException">Lorsque l'url passé n'est pas bien formé</exception>
        public Machine(string name, string url)
        {
            this.Name = name;
            if (IsValidURL(url.Trim())) { this.Url = url.Trim(); }
            else { throw new FormatException("La valeur passé n'est pas un URL"); }
        }


        /// <summary>
        /// Serialise l'objet Machine
        /// </summary>
        /// <param name="info">Donné de sérialisation</param>
        /// <param name="ctx">Contexte de destination de la serialisation</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("machineName", this.Name);
            info.AddValue("machineUrl", this.Url);
        }
    }
}
