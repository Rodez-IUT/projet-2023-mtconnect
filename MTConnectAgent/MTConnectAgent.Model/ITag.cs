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

        /// <summary>
        /// Contenu du noeud XML qui n'est pas un autre noeud
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Liste des attributs hors id
        /// </summary>
        IDictionary<string, string> Attributs { get;}

        /// <summary>
        /// Liste des enfants du noeud actuel
        /// </summary>
        IList<ITag> Child { get; }

        /// <summary>
        /// Ajoute un attribut à la liste des attributs du noeud actuel
        /// </summary>
        /// <param name="key">identifiant de l'attribut</param>
        /// <param name="value">valeur de l'attribut</param>
        /// <returns>l'élément courant</returns>
        ITag AddAttribut(string key, string value);

        /// <summary>
        /// Ajoute un noeud enfant à la liste des enfants du noeud actuel
        /// </summary>
        /// <param name="childTag">Noeud ajouté</param>
        /// <returns>L'élément courant</returns>
        ITag AddChild(ITag childTag);

        /// <summary>
        /// Vérifie si un noeud possède des attributs
        /// </summary>
        /// <returns>True si il en possède, false sinon</returns>
        bool HasAttributs();

        /// <summary>
        /// Vérifie si un noeud possède des noeuds enfants
        /// </summary>
        /// <returns>True si il en possède, false sinon</returns>
        bool HasChild();
        void ClearChild();

        void SetChild(List<ITag> child);
    }
}
