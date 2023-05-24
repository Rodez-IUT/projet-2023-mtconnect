using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    public class SimpleTag : ITag
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public SimpleTag(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }

        public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<string, string> Attributs => throw new NotImplementedException();
        public IList<ITag> Child => throw new NotImplementedException();

        public ITag AddAttribut(string key, string value)
        {
            throw new NotImplementedException();
        }

        public ITag AddChild(ITag childTag)
        {
            throw new NotImplementedException();
        }

        public void ClearChild()
        {
            throw new NotImplementedException();
        }

        public bool HasAttributs()
        {
            throw new NotImplementedException();
        }

        public bool HasChild()
        {
            throw new NotImplementedException();
        }

        public void SetChild(List<ITag> child)
        {
            throw new NotImplementedException();
        }
    }
}
