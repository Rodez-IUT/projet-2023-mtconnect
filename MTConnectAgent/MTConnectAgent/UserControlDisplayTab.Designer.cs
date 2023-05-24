namespace MTConnectAgent
{
    partial class UserControlDisplayTab
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.copyNotification = new System.Windows.Forms.NotifyIcon(this.components);
            this.treeAffichage = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // copyNotification
            // 
            this.copyNotification.BalloonTipText = "test";
            this.copyNotification.BalloonTipTitle = "MTConnect";
            this.copyNotification.Text = "notifyIcon1";
            this.copyNotification.Visible = true;
            // 
            // treeAffichage
            // 
            this.treeAffichage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeAffichage.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.treeAffichage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeAffichage.Location = new System.Drawing.Point(0, 0);
            this.treeAffichage.Margin = new System.Windows.Forms.Padding(0);
            this.treeAffichage.Name = "treeAffichage";
            this.treeAffichage.ShowNodeToolTips = true;
            this.treeAffichage.Size = new System.Drawing.Size(613, 399);
            this.treeAffichage.TabIndex = 0;
            this.treeAffichage.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeAffichage_AfterCheck);
            // 
            // UserControlDisplayTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.treeAffichage);
            this.Name = "UserControlDisplayTab";
            this.Size = new System.Drawing.Size(613, 399);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon copyNotification;
        private System.Windows.Forms.TreeView treeAffichage;
    }
}
