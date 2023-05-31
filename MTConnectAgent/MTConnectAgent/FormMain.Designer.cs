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
            this.tabPath = new System.Windows.Forms.TabPage();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewClientMachine
            // 
            this.treeViewClientMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewClientMachine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.treeViewClientMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewClientMachine.Location = new System.Drawing.Point(12, 99);
            this.treeViewClientMachine.Name = "treeViewClientMachine";
            this.treeViewClientMachine.Size = new System.Drawing.Size(149, 335);
            this.treeViewClientMachine.TabIndex = 0;
            this.treeViewClientMachine.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewClientMachine_AfterSelect);
            // 
            // buttonAjouterClient
            // 
            this.buttonAjouterClient.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.buttonAjouterClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAjouterClient.Location = new System.Drawing.Point(12, 13);
            this.buttonAjouterClient.Name = "buttonAjouterClient";
            this.buttonAjouterClient.Size = new System.Drawing.Size(149, 37);
            this.buttonAjouterClient.TabIndex = 1;
            this.buttonAjouterClient.Text = "Ajouter un client";
            this.buttonAjouterClient.UseVisualStyleBackColor = false;
            this.buttonAjouterClient.Click += new System.EventHandler(this.buttonAjouterClient_Click);
            // 
            // buttonAjouterMachine
            // 
            this.buttonAjouterMachine.BackColor = System.Drawing.Color.LightSkyBlue;
            this.buttonAjouterMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAjouterMachine.Location = new System.Drawing.Point(12, 56);
            this.buttonAjouterMachine.Name = "buttonAjouterMachine";
            this.buttonAjouterMachine.Size = new System.Drawing.Size(149, 37);
            this.buttonAjouterMachine.TabIndex = 2;
            this.buttonAjouterMachine.Text = "Ajouter une machine";
            this.buttonAjouterMachine.UseVisualStyleBackColor = false;
            this.buttonAjouterMachine.Click += new System.EventHandler(this.buttonAjouterMachine_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabProbe);
            this.tabs.Controls.Add(this.tabCurrent);
            this.tabs.Controls.Add(this.tabPath);
            this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabs.Location = new System.Drawing.Point(167, 13);
            this.tabs.Name = "tabs";
            this.tabs.Padding = new System.Drawing.Point(3, 3);
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(785, 425);
            this.tabs.TabIndex = 3;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            this.tabs.Resize += new System.EventHandler(this.tabs_Resize);
            // 
            // tabProbe
            // 
            this.tabProbe.Location = new System.Drawing.Point(4, 29);
            this.tabProbe.Name = "tabProbe";
            this.tabProbe.Padding = new System.Windows.Forms.Padding(3);
            this.tabProbe.Size = new System.Drawing.Size(741, 392);
            this.tabProbe.TabIndex = 0;
            this.tabProbe.Text = "Probe";
            this.tabProbe.UseVisualStyleBackColor = true;
            // 
            // tabCurrent
            // 
            this.tabCurrent.Location = new System.Drawing.Point(4, 29);
            this.tabCurrent.Name = "tabCurrent";
            this.tabCurrent.Padding = new System.Windows.Forms.Padding(3);
            this.tabCurrent.Size = new System.Drawing.Size(777, 392);
            this.tabCurrent.TabIndex = 1;
            this.tabCurrent.Text = "Current";
            this.tabCurrent.UseVisualStyleBackColor = true;
            // 
            // tabPath
            // 
            this.tabPath.Location = new System.Drawing.Point(4, 29);
            this.tabPath.Name = "tabPath";
            this.tabPath.Size = new System.Drawing.Size(613, 392);
            this.tabPath.TabIndex = 2;
            this.tabPath.Text = "Path";
            this.tabPath.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(964, 450);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.buttonAjouterMachine);
            this.Controls.Add(this.buttonAjouterClient);
            this.Controls.Add(this.treeViewClientMachine);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FormMain";
            this.Text = "MTConnect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Resize += new System.EventHandler(this.tabs_Resize);
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewClientMachine;
        private System.Windows.Forms.Button buttonAjouterClient;
        private System.Windows.Forms.Button buttonAjouterMachine;
        private System.Windows.Forms.TabPage tabProbe;
        private System.Windows.Forms.TabPage tabCurrent;
        private UserControlDisplayTab userControlProbe1;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabPath;
    }
}

