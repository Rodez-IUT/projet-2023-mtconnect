namespace MTConnectAgent
{
    partial class FormModifieMachine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAjouterClient = new System.Windows.Forms.Button();
            this.textMachineUrl = new System.Windows.Forms.TextBox();
            this.labelMachineUrl = new System.Windows.Forms.Label();
            this.textMachineName = new System.Windows.Forms.TextBox();
            this.labelMachineName = new System.Windows.Forms.Label();
            this.labelClient = new System.Windows.Forms.Label();
            this.buttonAjouter = new System.Windows.Forms.Button();
            this.buttonAnnuler = new System.Windows.Forms.Button();
            this.comboClients = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonAjouterClient
            // 
            this.buttonAjouterClient.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.buttonAjouterClient.Location = new System.Drawing.Point(386, 10);
            this.buttonAjouterClient.Name = "buttonAjouterClient";
            this.buttonAjouterClient.Size = new System.Drawing.Size(98, 23);
            this.buttonAjouterClient.TabIndex = 21;
            this.buttonAjouterClient.Text = "Ajouter un client";
            this.buttonAjouterClient.UseVisualStyleBackColor = false;
            this.buttonAjouterClient.Click += new System.EventHandler(this.buttonAjouterClient_Click);
            // 
            // textMachineUrl
            // 
            this.textMachineUrl.Location = new System.Drawing.Point(120, 66);
            this.textMachineUrl.Name = "textMachineUrl";
            this.textMachineUrl.Size = new System.Drawing.Size(260, 20);
            this.textMachineUrl.TabIndex = 20;
            this.textMachineUrl.TextChanged += new System.EventHandler(this.textMachineUrl_TextChanged);
            this.textMachineUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AjoutClient_KeyPress);
            // 
            // labelMachineUrl
            // 
            this.labelMachineUrl.AutoSize = true;
            this.labelMachineUrl.Location = new System.Drawing.Point(13, 69);
            this.labelMachineUrl.Name = "labelMachineUrl";
            this.labelMachineUrl.Size = new System.Drawing.Size(104, 13);
            this.labelMachineUrl.TabIndex = 19;
            this.labelMachineUrl.Text = "URL de la machine :";
            // 
            // textMachineName
            // 
            this.textMachineName.Location = new System.Drawing.Point(120, 40);
            this.textMachineName.Name = "textMachineName";
            this.textMachineName.Size = new System.Drawing.Size(260, 20);
            this.textMachineName.TabIndex = 18;
            this.textMachineName.TextChanged += new System.EventHandler(this.textMachineName_TextChanged);
            this.textMachineName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AjoutClient_KeyPress);
            // 
            // labelMachineName
            // 
            this.labelMachineName.AutoSize = true;
            this.labelMachineName.Location = new System.Drawing.Point(14, 43);
            this.labelMachineName.Name = "labelMachineName";
            this.labelMachineName.Size = new System.Drawing.Size(104, 13);
            this.labelMachineName.TabIndex = 17;
            this.labelMachineName.Text = "Nom de la machine :";
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.Location = new System.Drawing.Point(14, 15);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(100, 13);
            this.labelClient.TabIndex = 16;
            this.labelClient.Text = "Sélection du client :";
            // 
            // buttonAjouter
            // 
            this.buttonAjouter.BackColor = System.Drawing.Color.PapayaWhip;
            this.buttonAjouter.Location = new System.Drawing.Point(386, 120);
            this.buttonAjouter.Name = "buttonAjouter";
            this.buttonAjouter.Size = new System.Drawing.Size(98, 23);
            this.buttonAjouter.TabIndex = 15;
            this.buttonAjouter.Text = "Modifier";
            this.buttonAjouter.UseVisualStyleBackColor = false;
            this.buttonAjouter.Click += new System.EventHandler(this.buttonAjouter_Click);
            // 
            // buttonAnnuler
            // 
            this.buttonAnnuler.Location = new System.Drawing.Point(12, 120);
            this.buttonAnnuler.Name = "buttonAnnuler";
            this.buttonAnnuler.Size = new System.Drawing.Size(98, 23);
            this.buttonAnnuler.TabIndex = 14;
            this.buttonAnnuler.Text = "Annuler";
            this.buttonAnnuler.UseVisualStyleBackColor = true;
            this.buttonAnnuler.Click += new System.EventHandler(this.buttonAnnuler_Click);
            // 
            // comboClients
            // 
            this.comboClients.Enabled = false;
            this.comboClients.Location = new System.Drawing.Point(120, 12);
            this.comboClients.Name = "comboClients";
            this.comboClients.Size = new System.Drawing.Size(260, 21);
            this.comboClients.TabIndex = 13;
            this.comboClients.Text = "Aucun client n\'est présent (il faut d\'abord le créer)";
            this.comboClients.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AjoutClient_KeyPress);
            // 
            // FormModifieMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(491, 155);
            this.Controls.Add(this.buttonAjouterClient);
            this.Controls.Add(this.textMachineUrl);
            this.Controls.Add(this.labelMachineUrl);
            this.Controls.Add(this.textMachineName);
            this.Controls.Add(this.labelMachineName);
            this.Controls.Add(this.labelClient);
            this.Controls.Add(this.buttonAjouter);
            this.Controls.Add(this.buttonAnnuler);
            this.Controls.Add(this.comboClients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormModifieMachine";
            this.Text = "Modifier une machine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAjouterClient;
        private System.Windows.Forms.TextBox textMachineUrl;
        private System.Windows.Forms.Label labelMachineUrl;
        private System.Windows.Forms.TextBox textMachineName;
        private System.Windows.Forms.Label labelMachineName;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Button buttonAjouter;
        private System.Windows.Forms.Button buttonAnnuler;
        private System.Windows.Forms.ComboBox comboClients;
    }
}