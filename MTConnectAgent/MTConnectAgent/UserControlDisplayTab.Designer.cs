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
            this.treeAffichage = new System.Windows.Forms.TreeView();
            this.container = new System.Windows.Forms.GroupBox();
            this.containerFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExpandNodes = new System.Windows.Forms.Button();
            this.btnCollapseNodes = new System.Windows.Forms.Button();
            this.rechercheBox = new System.Windows.Forms.TextBox();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.container.SuspendLayout();
            this.containerFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeAffichage
            // 
            this.treeAffichage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeAffichage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.treeAffichage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeAffichage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeAffichage.Location = new System.Drawing.Point(0, 0);
            this.treeAffichage.Margin = new System.Windows.Forms.Padding(0);
            this.treeAffichage.Name = "treeAffichage";
            this.treeAffichage.ShowNodeToolTips = true;
            this.treeAffichage.Size = new System.Drawing.Size(777, 357);
            this.treeAffichage.TabIndex = 0;
            this.treeAffichage.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeAffichage_AfterCheck);
            // 
            // container
            // 
            this.container.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.container.BackColor = System.Drawing.Color.Transparent;
            this.container.Controls.Add(this.containerFlow);
            this.container.Location = new System.Drawing.Point(0, 351);
            this.container.Margin = new System.Windows.Forms.Padding(0);
            this.container.Name = "container";
            this.container.Padding = new System.Windows.Forms.Padding(0);
            this.container.Size = new System.Drawing.Size(777, 40);
            this.container.TabIndex = 0;
            this.container.TabStop = false;
            // 
            // containerFlow
            // 
            this.containerFlow.AutoSize = true;
            this.containerFlow.Controls.Add(this.btnExpandNodes);
            this.containerFlow.Controls.Add(this.btnCollapseNodes);
            this.containerFlow.Controls.Add(this.rechercheBox);
            this.containerFlow.Location = new System.Drawing.Point(0, 5);
            this.containerFlow.Margin = new System.Windows.Forms.Padding(0);
            this.containerFlow.Name = "containerFlow";
            this.containerFlow.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.containerFlow.Size = new System.Drawing.Size(777, 30);
            this.containerFlow.TabIndex = 1;
            // 
            // btnExpandNodes
            // 
            this.btnExpandNodes.AutoSize = true;
            this.btnExpandNodes.Location = new System.Drawing.Point(3, 4);
            this.btnExpandNodes.Name = "btnExpandNodes";
            this.btnExpandNodes.Size = new System.Drawing.Size(123, 23);
            this.btnExpandNodes.TabIndex = 2;
            this.btnExpandNodes.Text = "Développer l\'affichage";
            this.btnExpandNodes.UseVisualStyleBackColor = true;
            // 
            // btnCollapseNodes
            // 
            this.btnCollapseNodes.AutoSize = true;
            this.btnCollapseNodes.Location = new System.Drawing.Point(132, 4);
            this.btnCollapseNodes.Name = "btnCollapseNodes";
            this.btnCollapseNodes.Size = new System.Drawing.Size(123, 23);
            this.btnCollapseNodes.TabIndex = 2;
            this.btnCollapseNodes.Text = "Réduire l\'affichage";
            this.btnCollapseNodes.UseVisualStyleBackColor = true;
            // 
            // rechercheBox
            // 
            this.rechercheBox.Location = new System.Drawing.Point(261, 4);
            this.rechercheBox.Name = "rechercheBox";
            this.rechercheBox.Size = new System.Drawing.Size(159, 20);
            this.rechercheBox.TabIndex = 3;
            // 
            // imageListIcons
            // 
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(20, 20);
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // UserControlDisplayTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.container);
            this.Controls.Add(this.treeAffichage);
            this.Name = "UserControlDisplayTab";
            this.Size = new System.Drawing.Size(777, 392);
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            this.containerFlow.ResumeLayout(false);
            this.containerFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeAffichage;
        private System.Windows.Forms.GroupBox container;
        private System.Windows.Forms.FlowLayoutPanel containerFlow;
        private System.Windows.Forms.Button btnExpandNodes;
        private System.Windows.Forms.Button btnCollapseNodes;
        private System.Windows.Forms.TextBox rechercheBox;
        private System.Windows.Forms.ImageList imageListIcons;
    }
}
