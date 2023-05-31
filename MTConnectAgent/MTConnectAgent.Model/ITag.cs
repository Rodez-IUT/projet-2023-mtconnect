using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    /// <summary>
    /// Représente un noeud XML
    /// </summary>
    public interface ITag
    {
        /// <summary>
        /// Nom du noeud XMl
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Attribut id du noeud XML si il existe
        /// </summary>
        string Id { get; set; }
    }
}
