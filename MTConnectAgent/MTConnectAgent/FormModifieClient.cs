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
    public partial class FormModifieClient : Form
    {
        public Client modifiedClient { get; set; }

        public FormModifieClient(Client modifiedClient)
        {
            this.modifiedClient = modifiedClient;
            InitializeComponent();
            textClientName.Text = modifiedClient.Name;
        }

        private void buttonAnnuler_Click(object sender, EventArgs e)
        {
            this.modifiedClient = null;
            this.Close();
        }

        private void buttonModifier_Click(object sender, EventArgs e)
        {
            modifier();
        }


        private bool IsTextBoxNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(textClientName.Text);
        }

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

        private void textClientName_TextChanged(object sender, EventArgs e)
        {
            toogleButtonModifier();
        }

        private void AjoutClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (buttonModifier.Enabled && e.KeyChar == (char)Keys.Return)
            {
                modifier();
            }
        }

        private void modifier()
        {
            modifiedClient.Name = textClientName.Text.Trim();
            this.Close();
        }
    }
}
