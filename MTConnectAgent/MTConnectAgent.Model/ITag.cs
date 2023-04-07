using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    public interface ITag
    {
        string Name { get; set; }
        string Id { get; set; }
        string Value { get; set; }
        IDictionary<string, string> Attributs { get;}
        IList<ITag> Child { get; }

        ITag AddAttribut(string key, string value);
        ITag AddChild(ITag childTag);

        bool HasAttributs();
        bool HasChild();
    }
}
