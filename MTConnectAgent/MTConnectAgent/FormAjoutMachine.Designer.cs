﻿namespace MTConnectAgent
{
    partial class FormAjoutMachine
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
            this.comboClients = new System.Windows.Forms.ComboBox();
            this.buttonAjouter = new System.Windows.Forms.Button();
            this.buttonAnnuler = new System.Windows.Forms.Button();
            this.labelClient = new System.Windows.Forms.Label();
            this.labelMachineName = new System.Windows.Forms.Label();
            this.textMachineName = new System.Windows.Forms.TextBox();
            this.labelMachineUrl = new System.Windows.Forms.Label();
            this.textMachineUrl = new System.Windows.Forms.TextBox();
            this.buttonAjouterClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboClients
            // 
            this.comboClients.Location = new System.Drawing.Point(120, 12);
            this.comboClients.Name = "comboClients";
            this.comboClients.Size = new System.Drawing.Size(260, 21);
            this.comboClients.TabIndex = 1;
            this.comboClients.Text = "Aucun client n\'est présent (il faut d\'abord le créer)";
            // 
            // buttonAjouter
            // 
            this.buttonAjouter.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouter.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouter.FlatAppearance.BorderSize = 0;
            this.buttonAjouter.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouter.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouter.Location = new System.Drawing.Point(16, 92);
            this.buttonAjouter.Name = "buttonAjouter";
            this.buttonAjouter.Size = new System.Drawing.Size(98, 23);
            this.buttonAjouter.TabIndex = 6;
            this.buttonAjouter.Text = "Ajouter";
            this.buttonAjouter.UseVisualStyleBackColor = false;
            this.buttonAjouter.Click += new System.EventHandler(this.buttonAjouter_Click);
            // 
            // buttonAnnuler
            // 
            this.buttonAnnuler.Location = new System.Drawing.Point(282, 92);
            this.buttonAnnuler.Name = "buttonAnnuler";
            this.buttonAnnuler.Size = new System.Drawing.Size(98, 23);
            this.buttonAnnuler.TabIndex = 5;
            this.buttonAnnuler.Text = "Annuler";
            this.buttonAnnuler.UseVisualStyleBackColor = true;
            this.buttonAnnuler.Click += new System.EventHandler(this.buttonAnnuler_Click);
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.Location = new System.Drawing.Point(14, 15);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(100, 13);
            this.labelClient.TabIndex = 7;
            this.labelClient.Text = "Sélection du client :";
            // 
            // labelMachineName
            // 
            this.labelMachineName.AutoSize = true;
            this.labelMachineName.Location = new System.Drawing.Point(14, 43);
            this.labelMachineName.Name = "labelMachineName";
            this.labelMachineName.Size = new System.Drawing.Size(104, 13);
            this.labelMachineName.TabIndex = 8;
            this.labelMachineName.Text = "Nom de la machine :";
            // 
            // textMachineName
            // 
            this.textMachineName.Location = new System.Drawing.Point(120, 40);
            this.textMachineName.Name = "textMachineName";
            this.textMachineName.Size = new System.Drawing.Size(260, 20);
            this.textMachineName.TabIndex = 9;
            this.textMachineName.TextChanged += new System.EventHandler(this.textMachineName_TextChanged);
            // 
            // labelMachineUrl
            // 
            this.labelMachineUrl.AutoSize = true;
            this.labelMachineUrl.Location = new System.Drawing.Point(13, 69);
            this.labelMachineUrl.Name = "labelMachineUrl";
            this.labelMachineUrl.Size = new System.Drawing.Size(104, 13);
            this.labelMachineUrl.TabIndex = 10;
            this.labelMachineUrl.Text = "URL de la machine :";
            // 
            // textMachineUrl
            // 
            this.textMachineUrl.Location = new System.Drawing.Point(120, 66);
            this.textMachineUrl.Name = "textMachineUrl";
            this.textMachineUrl.Size = new System.Drawing.Size(260, 20);
            this.textMachineUrl.TabIndex = 11;
            this.textMachineUrl.TextChanged += new System.EventHandler(this.textMachineUrl_TextChanged);
            // 
            // buttonAjouterClient
            // 
            this.buttonAjouterClient.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouterClient.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouterClient.FlatAppearance.BorderSize = 0;
            this.buttonAjouterClient.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouterClient.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAjouterClient.Location = new System.Drawing.Point(153, 92);
            this.buttonAjouterClient.Name = "buttonAjouterClient";
            this.buttonAjouterClient.Size = new System.Drawing.Size(98, 23);
            this.buttonAjouterClient.TabIndex = 12;
            this.buttonAjouterClient.Text = "Ajouter un client";
            this.buttonAjouterClient.UseVisualStyleBackColor = false;
            this.buttonAjouterClient.Click += new System.EventHandler(this.buttonAjouterClient_Click);
            // 
            // FormAjoutMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 125);
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
            this.MaximizeBox = false;
            this.Name = "FormAjoutMachine";
            this.Text = "Ajouter une Machine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboClients;
        private System.Windows.Forms.Button buttonAjouter;
        private System.Windows.Forms.Button buttonAnnuler;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelMachineName;
        private System.Windows.Forms.TextBox textMachineName;
        private System.Windows.Forms.Label labelMachineUrl;
        private System.Windows.Forms.TextBox textMachineUrl;
        private System.Windows.Forms.Button buttonAjouterClient;
    }
}