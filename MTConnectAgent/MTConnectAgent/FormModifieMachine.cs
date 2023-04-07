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
    public partial class FormModifieMachine : Form
    {
        public Machine modifiedMachine { get; set; }
        public int indexClient { get; set; }
        private List<Client> clients;

        public FormModifieMachine(Machine modifiedMachine, List<Client>clients, int indexClients)
        {
            this.modifiedMachine = modifiedMachine;
            this.indexClient = indexClients;
            this.clients = clients;
            InitializeComponent();
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
                comboClients.SelectedIndex = this.indexClient;
                comboClients.Enabled = true;
                textMachineName.Enabled = true;
                textMachineUrl.Enabled = true;
            }
            comboClients.EndUpdate();
            textMachineName.Text = this.modifiedMachine.Name;
            textMachineUrl.Text = this.modifiedMachine.Url;
        }

        private void buttonAnnuler_Click(object sender, EventArgs e)
        {
            this.modifiedMachine = null;
            this.Close();
        }

        private void buttonAjouterClient_Click(object sender, EventArgs e)
        {
            FormAjoutClient addingClient = new FormAjoutClient();
            addingClient.ShowDialog();

            Client newClient = addingClient.newClient;
            if (newClient != null)
            {
                clients.Add(newClient);
                InitializePage();
            }
        }

        private void buttonAjouter_Click(object sender, EventArgs e)
        {
            Client selectedClient = (Client)comboClients.SelectedItem;
            this.modifiedMachine.Name = textMachineName.Text.Trim();
            string machineUrl = textMachineUrl.Text.Trim();
            if (!Machine.IsValidURL(machineUrl))
            {
                MessageBox.Show("L'url spécifié n'est pas valide", "Création d'une Machine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.modifiedMachine.Url = machineUrl;
            this.indexClient = comboClients.SelectedIndex;
            this.Close();
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
    }
}
