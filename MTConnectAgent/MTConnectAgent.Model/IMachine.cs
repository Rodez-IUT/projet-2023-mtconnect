using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    public interface IMachine : ISerializable
    {
        string Name { get; set; }
        string Url { get; set; }
        void GetObjectData(SerializationInfo info, StreamingContext ctxt);
    }
}
