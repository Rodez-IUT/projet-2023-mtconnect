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
    public partial class FormAjoutClient : Form
    {
        public Client newClient { get; set; }

        public FormAjoutClient()
        {
            InitializeComponent();
        }

        private void buttonAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAjouter_Click(object sender, EventArgs e)
        {
            ajouter();
        }

        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textClientName.Text);
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

        private void textClientName_TextChanged(object sender, EventArgs e)
        {
            toogleButtonAjout();
        }

        private void AjoutClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonAjouter.Enabled && e.KeyChar == (char)Keys.Return)
            {
                ajouter();
            }
        }

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
