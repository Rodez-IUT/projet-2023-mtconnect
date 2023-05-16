using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTConnectAgent.Model
{
    [Serializable()]
    public class Client
    {
        /// <summary>
        /// Accesseur du nom du client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Accesseur de la liste des Machine
        /// </summary>
        public List<Machine> Machines { get; }

        /// <summary></summary>
        /// <param name="name">Nom du client</param>
        public Client(string name)
        {
            this.Name = name.Trim();
            this.Machines = new List<Machine>();
        }

        /// <summary></summary>
        /// <param name="name">Nom du client</param>
        /// <param name="machines">Liste des machines du clients</param>
        public Client(string name, List<Machine> machines)
        {
            this.Name = name.Trim();
            this.Machines = machines;
        }
        
        //Deserialization constructor.
        public Client(SerializationInfo info, StreamingContext ctxt)
        {
            this.Name = (string)info.GetValue("clientName", typeof(string));
            this.Machines = (List<Machine>) info.GetValue("clientMachines", typeof(List<Machine>));
        }

        /// <summary>
        /// Serialise l'objet Client
        /// </summary>
        /// <param name="info">Donné de sérialisation</param>
        /// <param name="ctx">Contexte de destination de la serialisation</param>
        public void SetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("clientName", this.Name);
            info.AddValue("clientMachines", this.Machines);
        }

        /// <summary>
        /// Ajoute une machine au client
        /// </summary>
        /// <param name="newMachine">Machine a ajouter</param>
        /// <returns>Le client lui même (this)</returns>
        public Client AddMachine(Machine newMachine)
        {
            this.Machines.Add(newMachine);
            return this;
        }

        /// <summary>
        /// Fait un copie profonde de l'objet
        /// </summary>
        /// <returns>L'objet copié</returns>
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
