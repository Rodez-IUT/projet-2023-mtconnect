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
    /// Gere les interations du formulaire d'ajout d'un client
    /// </summary>
    public partial class FormAjoutClient : Form
    {
        /// <summary>
        /// Le client à ajouter
        /// </summary>
        public Client newClient { get; set; }

        /// <summary>
        /// Initialise le formulaire
        /// </summary>
        public FormAjoutClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gere le click sur le bouton annuler
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void buttonAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Ajoute le client lors du click sur le bouton de validation
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void buttonAjouter_Click(object sender, EventArgs e)
        {
            ajouter();
        }

        /// <summary>
        /// Vérifie si la boite de dialogue est vide ou non
        /// </summary>
        /// <returns>True si elle n'est pas vide, false sinon</returns>
        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textClientName.Text);
        }

        /// <summary>
        /// Active le bouton de validation si la boite de dialogue n'est pas vide, le désactive sinon
        /// </summary>
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

        /// <summary>
        /// Lance l'activation du bouton de validation lors du changement de texte dans la boite de dialogue
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void textClientName_TextChanged(object sender, EventArgs e)
        {
            toogleButtonAjout();
        }

        /// <summary>
        /// Lance l'ajout du client lors de l'appui sur la touche entrée du clavier
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void AjoutClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonAjouter.Enabled && e.KeyChar == (char)Keys.Return)
            {
                ajouter();
            }
        }

        /// <summary>
        /// Ajoute le client
        /// </summary>
        private void ajouter()
        {
            string name = textClientName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Le nom du client ne peut être vide", "Création d'un Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            newClient = new Client(name);
            this.Close();
        }
    }
}
