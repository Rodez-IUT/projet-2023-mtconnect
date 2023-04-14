using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    [Serializable()]
    public class Machine : IMachine
    {

        /// <summary>
        /// Accesseur du nom de la machine
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Accesseur de l'url de la machine
        /// </summary>
        public string Url
        {
            get { return this.Url; }
            set
            {
                string url = value.Trim();
                if (IsValidURL(url)) { this.Url = url; }
                else { throw new FormatException("La valeur passé n'est pas un URL"); }
            }
        }

        /// <summary>Valide la cohérence de l'url</summary>
        /// <param name="url">Url à valider</param>
        /// <returns>True si l'url est valide False sinon</returns>
        public static bool IsValidURL(string url)
        {
            Regex validateDateRegex = new Regex("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");
            return !string.IsNullOrWhiteSpace(url) && validateDateRegex.IsMatch(url);
        }

        //Deserialization constructor.
        public Machine(SerializationInfo info, StreamingContext ctx)
        {
            this.Name = (String)info.GetValue("machineName", typeof(string));
            this.Url = (String)info.GetValue("machineUrl", typeof(string));
        }

        /// <summary></summary>
        /// <exception cref="FormatException">Lorsque l'url passé n'est pas bien formé</exception>
        public Machine(string name, string url)
        {
            this.Name = name;
            url = url.Trim();

            if (IsValidURL(url))
            {
                this.Url = url;
            }
            else
            {
                throw new FormatException("La valeur passé n'est pas un URL");
            }
        }


        /// <summary>
        /// Serialise l'objet Machine
        /// </summary>
        /// <param name="info">Donné de sérialisation</param>
        /// <param name="ctx">Contexte de destination de la serialisation</param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("machineName", this._name);
            info.AddValue("machineUrl", this._url);
        }
    }
}
