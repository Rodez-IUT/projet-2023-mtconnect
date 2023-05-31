namespace MTConnectAgent
{
    partial class FormModifieClient
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
            this.buttonModifier = new System.Windows.Forms.Button();
            this.buttonAnnuler = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.textClientName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonModifier
            // 
            this.buttonModifier.BackColor = System.Drawing.Color.PapayaWhip;
            this.buttonModifier.Location = new System.Drawing.Point(240, 32);
            this.buttonModifier.Name = "buttonModifier";
            this.buttonModifier.Size = new System.Drawing.Size(98, 23);
            this.buttonModifier.TabIndex = 8;
            this.buttonModifier.Text = "Modifier";
            this.buttonModifier.UseVisualStyleBackColor = false;
            this.buttonModifier.Click += new System.EventHandler(this.buttonModifier_Click);
            // 
            // buttonAnnuler
            // 
            this.buttonAnnuler.Location = new System.Drawing.Point(15, 32);
            this.buttonAnnuler.Name = "buttonAnnuler";
            this.buttonAnnuler.Size = new System.Drawing.Size(98, 23);
            this.buttonAnnuler.TabIndex = 7;
            this.buttonAnnuler.Text = "Annuler";
            this.buttonAnnuler.UseVisualStyleBackColor = true;
            this.buttonAnnuler.Click += new System.EventHandler(this.buttonAnnuler_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(82, 13);
            this.labelName.TabIndex = 6;
            this.labelName.Text = "Nom du Client : ";
            // 
            // textClientName
            // 
            this.textClientName.Location = new System.Drawing.Point(100, 6);
            this.textClientName.Name = "textClientName";
            this.textClientName.Size = new System.Drawing.Size(238, 20);
            this.textClientName.TabIndex = 5;
            this.textClientName.TextChanged += new System.EventHandler(this.textClientName_TextChanged);
            this.textClientName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AjoutClient_KeyPress);
            // 
            // FormModifieClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.ClientSize = new System.Drawing.Size(350, 62);
            this.Controls.Add(this.buttonModifier);
            this.Controls.Add(this.buttonAnnuler);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textClientName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormModifieClient";
            this.Text = "Modifier un client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonModifier;
        private System.Windows.Forms.Button buttonAnnuler;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textClientName;
    }
}