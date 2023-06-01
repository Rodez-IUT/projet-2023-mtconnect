using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    /// <summary>
    /// Représente un tag simplifié avec juste un nom et un id
    /// </summary>
    public class SimpleTag : ITag
    {
        /// <summary>
        /// Nom du noeud
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribut id du noeud
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Intialise un nouveau simpleTag
        /// </summary>
        /// <param name="name">le nom à affecter</param>
        /// <param name="id">l'id à affecter</param>
        public SimpleTag(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
