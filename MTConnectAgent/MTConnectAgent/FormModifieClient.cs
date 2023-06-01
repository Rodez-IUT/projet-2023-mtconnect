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
    /// Gere les interactions du formulaire de modification d'un client
    /// </summary>
    public partial class FormModifieClient : Form
    {
        /// <summary>
        /// Le client modifié
        /// </summary>
        public Client modifiedClient { get; set; }

        /// <summary>
        /// Initialise une nouvelle fenêtre de modification d'un client
        /// </summary>
        /// <param name="modifiedClient"></param>
        public FormModifieClient(Client modifiedClient)
        {
            this.modifiedClient = modifiedClient;
            InitializeComponent();
            textClientName.Text = modifiedClient.Name;
        }

        /// <summary>
        /// Gere le click sur le bouton d'annulation
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void buttonAnnuler_Click(object sender, EventArgs e)
        {
            this.modifiedClient = null;
            this.Close();
        }

        /// <summary>
        /// Gere le click sur le bouton de validation
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void buttonModifier_Click(object sender, EventArgs e)
        {
            modifier();
        }

        /// <summary>
        /// Verifie si la boite de dialogue est vide
        /// </summary>
        /// <returns>true si la boite de dialogue est vide, false sinon</returns>
        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textClientName.Text);
        }

        /// <summary>
        /// Active le bouton modifier si les boites de dialogues ne sont pas vides, sinon le désactive
        /// </summary>
        private void toogleButtonModifier()
        {
            if (IsTextBoxNotEmpty())
            {
                buttonModifier.Enabled = true;
            }
            else
            {
                buttonModifier.Enabled = false;
            }
        }

        /// <summary>
        /// Lance l'activation ou la désactivation du bouton de validation lors du changement de texte dans les boites de dialogues
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void textClientName_TextChanged(object sender, EventArgs e)
        {
            toogleButtonModifier();
        }

        /// <summary>
        /// Lance la modification du client lors de l'appui sur la touche entrée
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void AjoutClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonModifier.Enabled && e.KeyChar == (char)Keys.Return)
            {
                modifier();
            }
        }

        /// <summary>
        /// Modifie le client courant
        /// </summary>
        private void modifier()
        {
            modifiedClient.Name = textClientName.Text.Trim();
            this.Close();
        }
    }
}
