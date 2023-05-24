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
            this.flowContent = new System.Windows.Forms.FlowLayoutPanel();
            this.copyNotification = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // flowContent
            // 
            this.flowContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowContent.AutoScroll = true;
            this.flowContent.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowContent.Location = new System.Drawing.Point(3, 3);
            this.flowContent.Name = "flowContent";
            this.flowContent.Size = new System.Drawing.Size(607, 393);
            this.flowContent.TabIndex = 1;
            this.flowContent.WrapContents = false;
            // 
            // copyNotification
            // 
            this.copyNotification.BalloonTipText = "test";
            this.copyNotification.BalloonTipTitle = "MTConnect";
            this.copyNotification.Text = "notifyIcon1";
            this.copyNotification.Visible = true;
            // 
            // UserControlProbeCurrent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowContent);
            this.Name = "UserControlProbeCurrent";
            this.Size = new System.Drawing.Size(613, 399);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowContent;
        private System.Windows.Forms.NotifyIcon copyNotification;
    }
}
