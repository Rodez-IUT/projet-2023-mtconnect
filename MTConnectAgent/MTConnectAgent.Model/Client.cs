using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model
{
    [Serializable()]
    public class Client : ISerializable
    {
        private string _name;
        private List<Machine> _machines;

        public Client(String name)
        {
            this._name = name.Trim();
            this._machines = new List<Machine>();
        }

        public Client(String name, List<Machine> machines)
        {
            this._name = name.Trim();
            this._machines = machines;
        }

        public string Name { get { return this._name; } set { this._name = value; } }

        public List<Machine> Machines { get { return this._machines; } }

        public Client AddMachine(Machine newMachine)
        {
            this._machines.Add(newMachine);
            return this;
        }

        //Deserialization constructor.
        public Client(SerializationInfo info, StreamingContext ctxt)
        {
            this._name = (String)info.GetValue("clientName", typeof(string));
            this._machines = (List<Machine>)info.GetValue("clientMachines", typeof(List<Machine>));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("clientName", this._name);
            info.AddValue("clientMachines", this._machines);
        }

        // Deep clone
        public Client DeepClone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Client)formatter.Deserialize(stream);
            }
        }
    }
}
