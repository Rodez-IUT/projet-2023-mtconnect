﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTConnectAgent.Model;

namespace MTConnectAgent
{
    public partial class FormMain : Form
    {
        private List<Client> clients;

        private int tabIndex = 0;

        private Machine selectedMachine;

        public FormMain()
        {
            InitializeComponent();
        }

        // Populates a TreeView control with example nodes. 
        private void InitializeTreeView()
        {
            treeViewClientMachine.BeginUpdate();
            treeViewClientMachine.Nodes.Clear();

            for (int i = 0; i < clients.Count; i++)
            {
                treeViewClientMachine.Nodes.Add(clients[i].Name);
                foreach (Machine machine in clients[i].Machines)
                {
                    ContextMenuStrip treeViewMachineContext = new ContextMenuStrip();

                    ToolStripMenuItem modifyMachineLabel = new ToolStripMenuItem("Modifier", null, modifyMachineLabel_Click);
                    ToolStripMenuItem deleteMachineLabel = new ToolStripMenuItem("Supprimer", null, deleteMachineLabel_Click);
                    List<object> tags = new List<object>(2)
                    {
                        machine,
                        i
                    };
                    modifyMachineLabel.Tag = tags;
                    deleteMachineLabel.Tag = tags;
                    
                    TreeNode noeudMachine = new TreeNode(machine.Name);
                    noeudMachine.Tag = machine;

                    treeViewMachineContext.Items.AddRange(new ToolStripMenuItem[] { modifyMachineLabel, deleteMachineLabel });
                    noeudMachine.ContextMenuStrip = treeViewMachineContext;
                    noeudMachine.Tag = machine;

                    treeViewClientMachine.Nodes[i].Nodes.Add(noeudMachine);
                }
                
                ContextMenuStrip treeViewClientContext = new ContextMenuStrip();

                ToolStripMenuItem modifyClientLabel = new ToolStripMenuItem("Modifier", null, modifyClientLabel_Click);
                ToolStripMenuItem deleteClientLabel = new ToolStripMenuItem("Supprimer", null, deleteClientLabel_Click);
                modifyClientLabel.Tag = clients[i];
                deleteClientLabel.Tag = clients[i];

                treeViewClientContext.Items.AddRange(new ToolStripMenuItem[] { modifyClientLabel, deleteClientLabel });
                treeViewClientMachine.Nodes[i].ContextMenuStrip = treeViewClientContext;
            }

            treeViewClientMachine.EndUpdate();

            this.Controls.Add(treeViewClientMachine);
            WriteToBinaryFile<List<Client>>(".\\clients", clients);
        }

        private void buttonAjouterClient_Click(object sender, EventArgs e)
        {
            FormAjoutClient addingClient = new FormAjoutClient();
            addingClient.ShowDialog();

            Client newClient = addingClient.newClient;
            if (newClient != null)
            {
                clients.Add(newClient);
                InitializeTreeView();
            }
        }

        private void buttonAjouterMachine_Click(object sender, EventArgs e)
        {
            FormAjoutMachine addingMachine = new FormAjoutMachine(clients, treeViewClientMachine.SelectedNode);
            addingMachine.ShowDialog();
            
            if (addingMachine.updated)
            {
                InitializeTreeView();
            }
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the binary file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the binary file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
            }
            catch (FileNotFoundException e)
            {
                return default(T);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteToBinaryFile<List<Client>>(".\\clients", clients);
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            clients = ReadFromBinaryFile<List<Client>>(".\\clients");
            if (clients == null)
            {
                clients = new List<Client>();
            }
            InitializeTreeView();
        }

        private void modifyClientLabel_Click(object sender, EventArgs e)
        {
            Client target = (Client)((ToolStripMenuItem)sender).Tag;
            FormModifieClient modifyingClient = new FormModifieClient(target);
            modifyingClient.ShowDialog();

            Client modifiedClient = modifyingClient.modifiedClient;
            if (modifiedClient != null)
            {
                target = modifiedClient;
                InitializeTreeView();
            }
        }

        private void deleteClientLabel_Click(object sender, EventArgs e)
        {
            Client target = (Client)((ToolStripMenuItem)sender).Tag;
            clients.Remove(target);
            InitializeTreeView();
        }

        private void modifyMachineLabel_Click(object sender, EventArgs e)
        {
            var target = (List<object>)(((ToolStripMenuItem)sender).Tag);
            Machine machine = (Machine)target[0];
            int indexClient = (int)target[1];
            int indexMachine = clients[indexClient].Machines.IndexOf(machine);

            FormModifieMachine modifyingMachine = new FormModifieMachine(machine, clients, indexClient);
            modifyingMachine.ShowDialog();

            Machine modifiedMachine = modifyingMachine.modifiedMachine;
            if (indexClient != modifyingMachine.indexClient)
            {
                clients[indexClient].Machines.RemoveAt(indexMachine);
                clients[modifyingMachine.indexClient].AddMachine(modifiedMachine);
            }
            if (modifiedMachine != null)
            {
                InitializeTreeView();
            }
        }

        private void deleteMachineLabel_Click(object sender, EventArgs e)
        {
            var target = (List<object>)(((ToolStripMenuItem)sender).Tag);
            Machine machine = (Machine)target[0];
            int indexClient = (int)target[1];
            int indexMachine = clients[indexClient].Machines.IndexOf(machine);
            clients[indexClient].Machines.RemoveAt(indexMachine);
            InitializeTreeView();
        }

        private void treeViewClientMachine_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                selectedMachine = (Machine)e.Node.Tag;

                displayTab();
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabIndex = ((TabControl)sender).SelectedIndex;

            displayTab();
        }

        private void displayTab()
        {
            if (selectedMachine == null)
            {
                return;  // early return pour bloquer l'affichage si aucune machine sélectionnée
            }

            this.tabCurrent.Controls.Clear();
            this.tabProbe.Controls.Clear();
            this.tabPath.Controls.Clear();

            switch (tabIndex)
            {
                case 1: // Current tab
                    UserControlDisplayTab userCtrlCurrent = new UserControlDisplayTab(selectedMachine.Url, UserControlDisplayTab.functions.current);
                    this.tabCurrent.Controls.Add(userCtrlCurrent);
                    userCtrlCurrent.Width = tabCurrent.Width;
                    userCtrlCurrent.Height = tabCurrent.Height;
                    break;

                case 2: // Path tab
                    UserControlDisplayTab userCtrlPath = new UserControlDisplayTab(selectedMachine.Url, UserControlDisplayTab.functions.path);
                    this.tabPath.Controls.Add(userCtrlPath);
                    userCtrlPath.Width = tabPath.Width;
                    userCtrlPath.Height = tabPath.Height;
                    break;
                   
                case 0: // Probe tab
                default:
                    UserControlDisplayTab userCtrlProbe = new UserControlDisplayTab(selectedMachine.Url, UserControlDisplayTab.functions.probe);
                    this.tabProbe.Controls.Add(userCtrlProbe);
                    userCtrlProbe.Width = tabProbe.Width;
                    userCtrlProbe.Height = tabProbe.Height;
                    break;
            }
        }

        private void tabs_Resize(object sender, EventArgs e)
        {
            UserControlDisplayTab userControl;
            switch (tabIndex)
            {
                case 1: // Current tab
                    if (tabCurrent.Controls.Count <= 0)
                    {
                        return;
                    }
                    userControl =  (UserControlDisplayTab)this.tabCurrent.Controls[0];
                    userControl.Width = tabCurrent.Width;
                    userControl.Height = tabCurrent.Height;
                    break;

                case 2: // Path tab
                    if (tabPath.Controls.Count <= 0)
                    {
                        return;
                    }
                    userControl = (UserControlDisplayTab)this.tabPath.Controls[0];
                    userControl.Width = tabPath.Width;
                    userControl.Height = tabPath.Height;
                    break;

                case 0: // Probe tab
                default:
                    if (tabProbe.Controls.Count <= 0)
                    {
                        return;
                    }
                    userControl = (UserControlDisplayTab)this.tabProbe.Controls[0];
                    userControl.Width = tabProbe.Width;
                    userControl.Height = tabProbe.Height;
                    break;
            }
        }
    }
}
