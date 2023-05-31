using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTConnectAgent.Model;

namespace MTConnectAgent
{
    public partial class FormAjoutMachine : Form
    {
        public bool updated { get; set; }
        private List<Client> clients { get; set; }
        private string selectedClient = null;

        public FormAjoutMachine(List<Client> clients, TreeNode selected)
        {
            InitializeComponent();
            this.updated = false;
            this.clients = clients;

            if (selected != null)
            {
                if (selected.Parent != null)
                {
                    this.selectedClient = selected.Parent.Text;
                }
                else
                {
                    this.selectedClient = selected.Text;
                }
            }
            InitializePage();
        }

        private void InitializePage()
        {
            comboClients.BeginUpdate();
            comboClients.DataSource = null;
            comboClients.DataSource = clients;
            comboClients.DisplayMember = "name";
            buttonAjouter.Enabled = false;
            if (comboClients.Items.Count == 0)
            {
                comboClients.Enabled = false;
                textMachineName.Enabled = false;
                textMachineUrl.Enabled = false;
            }
            else
            {
                comboClients.SelectedIndex = 0;
                comboClients.Enabled = true;
                textMachineName.Enabled = true;
                textMachineUrl.Enabled = true;
            }
            if (selectedClient != null)
            {
                comboClients.SelectedIndex = comboClients.FindStringExact(selectedClient);
            }
            comboClients.EndUpdate();
        }

        private void buttonAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAjouter_Click(object sender, EventArgs e)
        {
            ajouter();
        }

        private void buttonAjouterClient_Click(object sender, EventArgs e)
        {
            FormAjoutClient addingClient = new FormAjoutClient();
            addingClient.ShowDialog();

            Client newClient = addingClient.newClient;
            if (newClient != null)
            {
                clients.Add(newClient);
                this.updated = true;
                InitializePage();
            }
        }

        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textMachineName.Text) && !string.IsNullOrWhiteSpace(textMachineUrl.Text);
        }

        private void toogleButtonAjout()
        {
            if (IsTextBoxNotEmpty())
            {
                buttonAjouter.Enabled = true;
            }
            else
            {
                buttonAjouter.Enabled = false;
            }
        }

        private void textMachineName_TextChanged(object sender, EventArgs e)
        {
            toogleButtonAjout();
        }

        private void textMachineUrl_TextChanged(object sender, EventArgs e)
        {
            toogleButtonAjout();
        }

        private void AjoutMachine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonAjouter.Enabled && e.KeyChar == (char)Keys.Return)
            {
                ajouter();
            }
        }

        private void ajouter()
        {
            Client selectedClient = (Client)comboClients.SelectedItem;
            string machineName = textMachineName.Text.Trim();
            string machineUrl = textMachineUrl.Text.Trim();
            if (!Machine.IsValidURL(machineUrl))
            {
                MessageBox.Show("L'url spécifié n'est pas valide", "Création d'une Machine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            selectedClient.AddMachine(new Machine(machineName, machineUrl));
            this.updated = true;
            this.Close();
        }
    }
}
