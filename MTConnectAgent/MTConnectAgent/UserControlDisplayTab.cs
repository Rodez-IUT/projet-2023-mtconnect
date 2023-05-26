﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MTConnectAgent.Model;
using MTConnectAgent.BLL;
using System.Xml.Linq;
using System.Threading;
using System;
using System.Linq;

namespace MTConnectAgent
{
    public partial class UserControlDisplayTab : UserControl
    {
        private ITag tagMachine;
        private string url;
        private functions fx;
        public enum functions
        {
            probe,
            current,
            path
        }
       
        private CheckBox or = new CheckBox();

        private ListView resultats = new ListView();

        private MTConnectClient instance = new MTConnectClient();

        private IList<ITag> specificTags = new List<ITag>();

        private IList<TreeNode> nodes = new List<TreeNode>();

        public UserControlDisplayTab(string url, functions fx)
        {
            this.url = url;
            this.fx = fx;

            this.Anchor = AllSideAnchor;
            this.Name = "userControl"+fx;
            this.TabIndex = 0;
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            Thread threadCalcul;
            switch (this.fx)
            {
                case functions.probe: // Probe
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                case functions.path: // Path
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                default: // Default => functions.current
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseCurrent(this.url); });
                    break;
            }
            threadCalcul.Start();
            threadCalcul.Join();
           
            if (this.fx.Equals(functions.path))
            {
                treeAffichage.Height = this.Height - 200;
                treeAffichage.CheckBoxes = true;
                GeneratePathResults();
            }

            btnExpandNodes.MouseClick += new MouseEventHandler(ExpandNodes);
            btnCollapseNodes.MouseClick += new MouseEventHandler(CollapseNodes);
            rechercheBox.TextChanged += new EventHandler(SearchItem);

            foreach (ITag tag in tagMachine.Child)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                TreeNode node = new TreeNode();
                node.Name = "node" + compositeName;
                node.Text = tag.Name;
                node.NodeFont = boldFont;
                node.Tag = new SimpleTag(tag.Name, tag.Id);

                treeAffichage.Nodes.Add(node);
                nodes.Add(node);

                if (tag.HasChild())
                {
                    Generer(tag.Child, node);
                }
            }
        }

        private readonly AnchorStyles AllSideAnchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        private readonly Font boldFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);

        // Affichage sous la forme d'un arbre des tags avec leurs attributs, id et nom
        private void Generer(IList<ITag> tags,TreeNode root)
        {
            foreach (ITag tag in tags)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                TreeNode node = new TreeNode();
                node.Name = "node" + compositeName;
                node.NodeFont = boldFont;
                node.Tag = new SimpleTag(tag.Name, tag.Id);

                /* 
                 * Prefixe de la ligne
                 * si la balise contient l'attribut type met l'attribut type en tant que prefixe
                 * sinon met le nom de la balise (<Devices>,...)
                 */
                if (!tag.Attributs.TryGetValue("type", out string prefixe))
                {
                    prefixe = tag.Name;
                }

                /*
                 * Corps de la ligne
                 * si la balise content une valeur (un contenu), l'insert dans le corps
                 * sinon si la balise contient l'attribut name met l'attribut name dans le corps
                 * sinon laisse la variable vide
                 */
                if (!tag.Attributs.TryGetValue("name", out string corps))
                {
                    corps = "";
                } else
                {
                    // ajoute les : avant le corps pour rendre l'affiche plus user friendly
                    corps = " : " + corps;
                }
                if (tag.Value != null && !tag.Value.Equals(""))
                {
                    corps = " : " + tag.Value;
                }

                /*
                 * Suffixe de la ligne
                 * si la balise possède l'attribut id, met l'attribut id dans le suffixe
                 * sinon laisse le suffixe vide
                 */
                string suffixe = "";
                if (tag.Id != null && !tag.Id.Equals(""))
                {
                    suffixe = " ( " + tag.Id + " ) ";
                }

                node.Text = prefixe + corps + suffixe;

                root.Nodes.Add(node);
                nodes.Add(node);

                if (tag.HasChild())
                {
                    Generer(tag.Child, node);
                }
            }
        }

        // Fenêtre de génération et obtention des PATHS
        private void GeneratePathResults()
        {
            // Equilibre l'affichage entre l'arbre des tags et le container des boutons et des résultats des paths
            treeAffichage.Height = this.Height - 200;
            treeAffichage.CheckBoxes = true;
            container.Height = 200;
            container.Location = new Point(0, treeAffichage.Height);
            container.Text = "Résultat des paths";
            containerFlow.Location = new Point(0,containerFlow.Location.Y + 5);

            // Utilisation de l'option OR ou non
            or.AutoSize = true;
            or.Name = "checkboxOr";
            or.Text = "Avec option OR";
            or.Anchor = AnchorStyles.None;
            containerFlow.Controls.Add(or);

            // Affichage du ou des PATH(S) à chaque checkbox cochée
            resultats.Name = "listResultatsPath";
            resultats.Location = new Point(5, containerFlow.Location.Y + containerFlow.Height);
            resultats.Size = new Size(containerFlow.Width - 10, 155);
            resultats.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultats.View = View.List;
            resultats.TabIndex = 2;
            resultats.MouseDoubleClick += new MouseEventHandler(CopyUrl);
            container.Controls.Add(resultats);
        }

        private static ITag ThreadParseProbe(string url)
        {
            MTConnectClient mtConnectClient = new MTConnectClient();
            XDocument t = mtConnectClient.getProbeAsync(url).Result;
            return mtConnectClient.ParseXMLRecursif(t.Root);
        }

        private static ITag ThreadParseCurrent(string url)
        {
            MTConnectClient mtConnectClient = new MTConnectClient();
            XDocument t = mtConnectClient.getCurrentAsync(url).Result;
            return mtConnectClient.ParseXMLRecursif(t.Root);
        }

        private void CopyUrl(object o, MouseEventArgs e)
        {
            ListView listView = (ListView)o;
            Clipboard.SetText(listView.FocusedItem.Text);
            copyNotification.ShowBalloonTip(1000, "MTConnect", "Le path a été copié dans le presse-papier.", ToolTipIcon.Info);
        }

        // Réduire l'affichage de l'arbre
        private void CollapseNodes(object sender, MouseEventArgs e)
        {
            treeAffichage.CollapseAll();
        }

        // Développe l'affichage de l'arbre
        private void ExpandNodes(object sender, MouseEventArgs e)
        {
            treeAffichage.ExpandAll();
        }

        private ITag ParseFullPath(string path)
        {

            ITag newTag = new Tag();
                string todoRenommer2 = path.Trim();
                if (path.Contains(":"))
                {
                    string[] todoRenommer3 = path.Trim().Split(':');
                    todoRenommer2 = todoRenommer3[todoRenommer3.Length - 1];
                    todoRenommer2 = todoRenommer2.Remove(todoRenommer2.Length - 1, 1).Trim();

                    newTag = new Tag(todoRenommer2);
                }

                if (path.Trim().EndsWith(")"))
                {
                    string[] todoRenommer3 = path.Trim().Split('(');
                    todoRenommer2 = todoRenommer3[todoRenommer3.Length - 1];
                    todoRenommer2 = todoRenommer2.Remove(todoRenommer2.Length - 1, 1).Trim();

                    newTag = new Tag("", todoRenommer2);
                    newTag.Name = instance.FindTagNameById(tagMachine, newTag.Id);
                }
            return newTag;
        }

        private void treeAffichage_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ITag tagTmp = ParseFullPath(e.Node.FullPath);
            specificTags.Add(tagTmp);


        //    foreach (ITag aRechercher in tmp)
        //    {
        //        IList<string> todoAREnommer = instance.GenererPath(aRechercher, url, or.Checked);

        //        resultats.Items.Clear();

        //        foreach (string todoAREnommer2 in todoAREnommer)
        //        {
        //            resultats.Items.Add(todoAREnommer2 + "\r\n");
        //        }
        //    }
        }
    }
}
