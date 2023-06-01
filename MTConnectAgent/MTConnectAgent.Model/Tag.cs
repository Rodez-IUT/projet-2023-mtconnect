using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    /// <summary>
    /// Représente un noeud XML MTConnect
    /// </summary>
    public class Tag : ITag
    {
        /// <summary>
        /// Nom du noeud xml
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribut id du noeud xml
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// liste des attributs qui ne sont pas l'id du noeud xml
        /// </summary>
        public IDictionary<string, string> Attributs { get; }

        /// <summary>
        /// Liste des noeuds enfant du noeud actuel
        /// </summary>
        public IList<Tag> Child { get; private set; }

        /// <summary>
        /// Valeur du noeud xml
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// Initialise un Tag avec son nom et son id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public Tag(string name, string id)
        {
            Name = name;
            Id = id;
            this.Attributs = new Dictionary<string, string>();
            this.Child = new List<Tag>();
        }

        /// <summary>
        /// Initialise un Tag avec son nom
        /// </summary>
        /// <param name="name"></param>
        public Tag(string name)
        {
            Name = name;
            Id = "";
            this.Attributs = new Dictionary<string, string>();
            this.Child = new List<Tag>();
        }

        public Tag()
        {
            Id = "";
            Name = "";
            Attributs = new Dictionary<string, string>();
            Child = new List<Tag>();
            Value = "";
        }

        /// <summary>
        /// Ajoute un attribut à la liste des attributs du Tag actuel
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>L'élément courant</returns>
        public ITag AddAttribut(string key, string value)
        {
            this.Attributs.Add(key, value);
            return this;
        }

        /// <summary>
        /// Ajoute un Tag à la liste des enfants du Tag actuel
        /// </summary>
        /// <param name="childTag"></param>
        /// <returns>L'élément courant</returns>
        public Tag AddChild(Tag childTag)
        {
            this.Child.Add(childTag);
            return this;
        }

        /// <summary>
        /// Verifie si le Tag actuel a un ou plusieurs attribut(s)
        /// </summary>
        /// <returns>True si il en possède, false sinon</returns>
        public bool HasAttributs()
        {
            return Attributs.Any();
        }

        /// <summary>
        /// Vérifie si le Tag actuel a un ou plusieurs Tag enfants
        /// </summary>
        /// <returns>True si il en possède, false sinon</returns>
        public bool HasChild()
        {
            return this.Child.Count > 0;
        }

        public void ClearChild()
        {
            this.Child = new List<Tag>();
        }

        public void SetChild(List<Tag> child)
        {
            this.Child = child;
        }

        public override bool Equals(object obj)
        {
            return obj is Tag tag &&
                   (GetHashCode() == tag.GetHashCode() ||
                   (Name.Equals(tag.Name) &&
                   Id.Equals(tag.Id)));
        }

        public override int GetHashCode()
        {
            var hashCode = 1460282102;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            return hashCode;
        }
    }
}
