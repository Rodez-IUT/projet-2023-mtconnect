using System;
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
        public Client NewClient { get; set; }

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
        private void ButtonAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Ajoute le client lors du click sur le bouton de validation
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void ButtonAjouter_Click(object sender, EventArgs e)
        {
            Ajouter();
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
        /// Lance l'activation du bouton de validation lors du changement de texte dans la boite de dialogue
        /// </summary>
        /// <param name="sender">objet appelant</param>
        /// <param name="e">évenement provoqué</param>
        private void TextClientName_TextChanged(object sender, EventArgs e)
        {
            ToogleButtonAjout();
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
                Ajouter();
            }
        }

        /// <summary>
        /// Ajoute le client
        /// </summary>
        private void Ajouter()
        {
            string name = textClientName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Le nom du client ne peut être vide", "Création d'un Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            NewClient = new Client(name);
            this.Close();
        }
    }
}
