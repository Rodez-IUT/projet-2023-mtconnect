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
    /// Gere les interactions du formulaire de modification d'une machine
    /// </summary>
    public partial class FormModifieMachine : Form
    {
        /// <summary>
        /// La machine modifiée
        /// </summary>
        public Machine ModifiedMachine { get; set; }

        /// <summary>
        /// L'index du client qui possède la machine
        /// </summary>
        public int IndexClient { get; set; }

        /// <summary>
        /// La liste de tous les clients
        /// </summary>
        private List<Client> clients;

        /// <summary>
        /// initialise une nouvelle fenêtre de modification de machine
        /// </summary>
        /// <param name="modifiedMachine"></param>
        /// <param name="clients"></param>
        /// <param name="indexClients"></param>
        public FormModifieMachine(Machine modifiedMachine, List<Client>clients, int indexClients)
        {
            this.ModifiedMachine = modifiedMachine;
            this.IndexClient = indexClients;
            this.clients = clients;
            InitializeComponent();
            InitializePage();
        }

        /// <summary>
        /// Initialise l'interface
        /// </summary>
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
                comboClients.SelectedIndex = this.IndexClient;
                comboClients.Enabled = true;
                textMachineName.Enabled = true;
                textMachineUrl.Enabled = true;
            }
            comboClients.EndUpdate();
            textMachineName.Text = this.ModifiedMachine.Name;
            textMachineUrl.Text = this.ModifiedMachine.Url;
        }

        /// <summary>
        /// Gere le click sur le bouton annuler
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void ButtonAnnuler_Click(object sender, EventArgs e)
        {
            this.ModifiedMachine = null;
            this.Close();
        }

        /// <summary>
        /// Gere le click sur le bouton d'ajout de client
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void ButtonAjouterClient_Click(object sender, EventArgs e)
        {
            FormAjoutClient addingClient = new FormAjoutClient();
            addingClient.ShowDialog();

            Client newClient = addingClient.NewClient;
            if (newClient != null)
            {
                clients.Add(newClient);
                InitializePage();
            }
        }

        /// <summary>
        /// Gere le click sur le bouton de validation
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void ButtonAjouter_Click(object sender, EventArgs e)
        {
            Modifier();
        }

        /// <summary>
        /// Verifie si les boites de dialogues sont vides
        /// </summary>
        /// <returns>true si aucune n'est vide, false sinon</returns>
        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textMachineName.Text) && !string.IsNullOrWhiteSpace(textMachineUrl.Text);
        }


        /// <summary>
        /// Active le bouton de validation si les boites de dialogues ne sont pas vides
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
        /// Active ou désactive le bouton de validation lors du changement du texte dans les boites de dialogues
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void TextMachineName_TextChanged(object sender, EventArgs e)
        {
            ToogleButtonAjout();
        }

        /// <summary>
        /// Active ou désactive le bouton de validation lors du changement du texte dans les boites de dialogues
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void TextMachineUrl_TextChanged(object sender, EventArgs e)
        {
            ToogleButtonAjout();
        }
        
        /// <summary>
        /// Lance la modification du client lors de l'appui sur la touche entrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonAjouter.Enabled && e.KeyChar == (char)Keys.Return)
            {
                Modifier();
            }
        }

        /// <summary>
        /// Modifie le client
        /// </summary>
        private void Modifier()
        {
            this.ModifiedMachine.Name = textMachineName.Text.Trim();
            string machineUrl = textMachineUrl.Text.Trim();
            if (!Machine.IsValidURL(machineUrl))
            {
                MessageBox.Show("L'url spécifié n'est pas valide", "Création d'une Machine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.ModifiedMachine.setUrl(machineUrl);
            this.IndexClient = comboClients.SelectedIndex;
            this.Close();
        }
    }
}
