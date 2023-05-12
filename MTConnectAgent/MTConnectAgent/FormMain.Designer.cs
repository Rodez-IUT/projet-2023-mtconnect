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
            this.treeViewClientMachine = new System.Windows.Forms.TreeView();
            this.buttonAjouterClient = new System.Windows.Forms.Button();
            this.buttonAjouterMachine = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabProbe = new System.Windows.Forms.TabPage();
            this.tabCurrent = new System.Windows.Forms.TabPage();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewClientMachine
            // 
            this.treeViewClientMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewClientMachine.Location = new System.Drawing.Point(12, 71);
            this.treeViewClientMachine.Name = "treeViewClientMachine";
            this.treeViewClientMachine.Size = new System.Drawing.Size(149, 363);
            this.treeViewClientMachine.TabIndex = 0;
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
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabProbe);
            this.tabs.Controls.Add(this.tabCurrent);
            this.tabs.Location = new System.Drawing.Point(167, 13);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(621, 425);
            this.tabs.TabIndex = 3;
            // 
            // tabProbe
            // 
            this.tabProbe.Location = new System.Drawing.Point(4, 22);
            this.tabProbe.Name = "tabProbe";
            this.tabProbe.Padding = new System.Windows.Forms.Padding(3);
            this.tabProbe.Size = new System.Drawing.Size(613, 399);
            this.tabProbe.TabIndex = 0;
            this.tabProbe.Text = "Probe";
            this.tabProbe.UseVisualStyleBackColor = true;
            // 
            // tabCurrent
            // 
            this.tabCurrent.Location = new System.Drawing.Point(4, 22);
            this.tabCurrent.Name = "tabCurrent";
            this.tabCurrent.Padding = new System.Windows.Forms.Padding(3);
            this.tabCurrent.Size = new System.Drawing.Size(613, 399);
            this.tabCurrent.TabIndex = 1;
            this.tabCurrent.Text = "Current";
            this.tabCurrent.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.buttonAjouterMachine);
            this.Controls.Add(this.buttonAjouterClient);
            this.Controls.Add(this.treeViewClientMachine);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FormMain";
            this.Text = "MTConnect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewClientMachine;
        private System.Windows.Forms.Button buttonAjouterClient;
        private System.Windows.Forms.Button buttonAjouterMachine;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabProbe;
        private System.Windows.Forms.TabPage tabCurrent;
    }
}

