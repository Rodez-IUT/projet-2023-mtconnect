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
    /// <summary>
    /// Gere les interactions du formulaire d'ajout d'une machine
    /// </summary>
    public partial class FormAjoutMachine : Form
    {
        /// <summary>
        /// True lorsque la machine est modfiée, false sinon
        /// </summary>
        public bool Updated { get; set; }
        private string selectedClient = null;

        /// <summary>
        /// Liste des clients
        /// </summary>
        public List<Client> Clients { get; set; }


        /// <summary>
        /// Initialise une fenêtre d'ajout de machine
        /// </summary>
        /// <param name="clients">La liste des clients à initialiser</param>
        public FormAjoutMachine(List<Client> clients, TreeNode selected)
        {
            InitializeComponent();
            this.Updated = false;
            this.Clients = clients;

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

        /// <summary>
        /// Initialise l'interface de la fenêtre
        /// </summary>
        private void InitializePage()
        {
            comboClients.BeginUpdate();
            comboClients.DataSource = null;
            comboClients.DataSource = Clients;
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

        /// <summary>
        /// Gere le click sur le bouton annuler
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void ButtonAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Ajoute la machine lors du click sur le bouton valider
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void ButtonAjouter_Click(object sender, EventArgs e)
        {
            Ajouter();
        }

        /// <summary>
        /// Lance la fenêtre d'ajout d'un client lors du click sur le bouton d'ajout d'un client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAjouterClient_Click(object sender, EventArgs e)
        {
            FormAjoutClient addingClient = new FormAjoutClient();
            addingClient.ShowDialog();

            Client newClient = addingClient.NewClient;
            if (newClient != null)
            {
                Clients.Add(newClient);
                this.Updated = true;
                InitializePage();
            }
        }

        /// <summary>
        /// Vérifie si les boites de dialogues sont vides
        /// </summary>
        /// <returns>true si aucune n'est vide, false sinon</returns>
        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textMachineName.Text) && !string.IsNullOrWhiteSpace(textMachineUrl.Text);
        }

        /// <summary>
        /// Active le bouton d'ajout lorsque les boites de dialogues ne sont pas vides
        /// </summary>
        private void ToogleButtonAjout()
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

        /// <summary>
        /// Lance l'activation du bouton de validation lors du changement du texte dans la boite de dialogue du nom de la machine
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void TextMachineName_TextChanged(object sender, EventArgs e)
        {
            ToogleButtonAjout();
        }

        /// <summary>
        /// Lance l'activation du bouton de validation lors du changement du texte dans la boite de dialogue de l'url de la machine
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void TextMachineUrl_TextChanged(object sender, EventArgs e)
        {
            ToogleButtonAjout();
        }

        /// <summary>
        /// Lance l'ajout de la machine lors de l'appui sur la touche entrée du clavier
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void AjoutMachine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonAjouter.Enabled && e.KeyChar == (char)Keys.Return)
            {
                Ajouter();
            }
        }

        /// <summary>
        /// Ajout de la machine
        /// </summary>
        private void Ajouter()
        {
            Client clientActuel = (Client)comboClients.SelectedItem;
            string machineName = textMachineName.Text.Trim();
            string machineUrl = textMachineUrl.Text.Trim();
            if (!Machine.IsValidURL(machineUrl))
            {
                MessageBox.Show("L'url spécifié n'est pas valide", "Création d'une Machine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clientActuel.AddMachine(new Machine(machineName, machineUrl));
            Updated = true;
            Close();
        }
    }
}
