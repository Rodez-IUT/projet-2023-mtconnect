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
    }
}
