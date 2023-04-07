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
    public class Machine : ISerializable
    {
        private string _name;
        private string _url;

        /// <summary>test</summary>
        /// <exception cref="FormatException">Lorsque l'url passé n'est pas bien formé</exception>
        public Machine(string name, string url)
        {
            this._name = name;
            url = url.Trim();

            if (IsValidURL(url))
            {
                this._url = url;
            }
            else
            {
                throw new FormatException("La valeur passé n'est pas un URL");
            }
        }

        /// <summary> </summary>
        public string Name { get { return this._name; } set { this._name = value; } }

        public string Url
        {
            get { return this._url; }

            /// <summary>Change la valeur de l'url</summary>
            /// <exception>FormatException lorsque l'url passé n'est pas bien formé</exception>
            set
            {
                string url = value.Trim();
                if (IsValidURL(url)) { this._url = url; }
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
        public Machine(SerializationInfo info, StreamingContext ctxt)
        {
            this._name = (String)info.GetValue("machineName", typeof(string));
            this._url = (String)info.GetValue("machineUrl", typeof(string));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("machineName", this._name);
            info.AddValue("machineUrl", this._url);
        }
    }
}
