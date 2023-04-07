namespace MTConnectAgent
{
    partial class FormMain
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.buttonAjouterClient = new System.Windows.Forms.Button();
            this.buttonAjouterMachine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 71);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(149, 367);
            this.treeView1.TabIndex = 0;
            // 
            // buttonAjouterClient
            // 
            this.buttonAjouterClient.Location = new System.Drawing.Point(12, 13);
            this.buttonAjouterClient.Name = "buttonAjouterClient";
            this.buttonAjouterClient.Size = new System.Drawing.Size(149, 23);
            this.buttonAjouterClient.TabIndex = 1;
            this.buttonAjouterClient.Text = "Ajouter un client";
            this.buttonAjouterClient.UseVisualStyleBackColor = true;
            this.buttonAjouterClient.Click += new System.EventHandler(this.buttonAjouterClient_Click);
            // 
            // buttonAjouterMachine
            // 
            this.buttonAjouterMachine.Location = new System.Drawing.Point(12, 42);
            this.buttonAjouterMachine.Name = "buttonAjouterMachine";
            this.buttonAjouterMachine.Size = new System.Drawing.Size(149, 23);
            this.buttonAjouterMachine.TabIndex = 2;
            this.buttonAjouterMachine.Text = "Ajouter une machine";
            this.buttonAjouterMachine.UseVisualStyleBackColor = true;
            this.buttonAjouterMachine.Click += new System.EventHandler(this.buttonAjouterMachine_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonAjouterMachine);
            this.Controls.Add(this.buttonAjouterClient);
            this.Controls.Add(this.treeView1);
            this.Name = "FormMain";
            this.Text = "MTConnect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button buttonAjouterClient;
        private System.Windows.Forms.Button buttonAjouterMachine;
    }
}

