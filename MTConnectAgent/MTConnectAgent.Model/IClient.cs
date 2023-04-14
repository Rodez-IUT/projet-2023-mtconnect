using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    public interface IClient : ISerializable
    {
        string Name { get; set; }
        IList<IMachine> Machines { get; }
        
        IClient AddMachine(IMachine newMachine);
        IClient DeepClone();
        void GetObjectData(SerializationInfo info, StreamingContext ctxt);
    }
}
