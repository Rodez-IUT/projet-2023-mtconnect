using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    public class Tag : ITag
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public IDictionary<string, string> Attributs { get;}
        public IList<ITag> Child { get; }
        public string Value { get; set; }

        public Tag(string name, string id)
        {
            Name = name; 
            Id = id;
            this.Attributs = new Dictionary<string,string>();
            this.Child = new List<ITag>();
        }

        public Tag(string name)
        {
            Name = name;
            Id = "";
            this.Attributs = new Dictionary<string, string>();
            this.Child = new List<ITag>();
        }

        public ITag AddAttribut(string key, string value)
        {
            this.Attributs.Add(key, value);
            return this;
        }

        public ITag AddChild(ITag childTag)
        {
            this.Child.Add(childTag);
            return this;
        }

        public bool HasAttributs()
        {
            return this.Attributs.Count() == 0;
        }

        public bool HasChild()
        {
            return this.Child.Count == 0;
        }
    }
}
