using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MTConnectAgent.Model;
using MTConnectAgent.BLL;
using System.Xml.Linq;
using System.Threading;
using System;
using Microsoft.Toolkit.Uwp.Notifications;

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

        private List<string> pathsFromTree = new List<string>();

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

        /// <summary>
        /// Charge la fenêtre sélectionnée avec l'affichage qui correspond
        /// </summary>
        public void UpdateView()
        {
            Cursor.Current = Cursors.WaitCursor; // Curseur de souris en mode chargement

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

            Cursor.Current = Cursors.Default;

            if (this.fx.Equals(functions.path))
            {
                treeAffichage.Height = this.Height - 200;
                treeAffichage.CheckBoxes = true;
                GeneratePathResults();
            }

            btnExpandNodes.MouseClick += new MouseEventHandler(ExpandNodes);
            btnCollapseNodes.MouseClick += new MouseEventHandler(CollapseNodes);
            rechercheBox.TextChanged += new EventHandler(SearchItem);

            // Ajout des icones pour les tags dans la TreeView
            imageList1.Images.Add(new Icon(".\\default.ico"));
            imageList1.Images.Add(new Icon(".\\header.ico"));
            imageList1.Images.Add(new Icon(".\\device.ico"));
            imageList1.Images.Add(new Icon(".\\dataitem.ico"));
            imageList1.Images.Add(new Icon(".\\component.ico"));
            imageList1.Images.Add(new Icon(".\\condition.ico"));
            imageList1.Images.Add(new Icon(".\\description.ico"));
            imageList1.Images.Add(new Icon(".\\event.ico"));
            //imageList1.Images.Add(new Icon(".\\sample.ico"));
            treeAffichage.ImageList = imageList1;

            foreach (ITag tag in tagMachine.Child)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                TreeNode node = new TreeNode();
                node.Name = "node" + compositeName;
                node.Text = tag.Name;
                node.NodeFont = boldFont;
                node.Tag = new SimpleTag(tag.Name, tag.Id);
                SelectIcon(node);

                treeAffichage.Nodes.Add(node);
                nodes.Add(node);

                if (tag.HasChild())
                {
                    Generer(tag.Child, node);
                }
            }
        }

        /// <summary>
        /// Sélectionne l'icone correspondant au tag généré
        /// </summary>
        /// <param name="node"></param>
        private void SelectIcon(TreeNode node)
        {
            int iconIndex = 0;
            
            switch (((SimpleTag)node.Tag).Name)
            {
                case "Header":
                    iconIndex = 1; // header.ico
                    break;
                case "Devices":
                case "Device":
                case "Streams":
                case "DeviceStream":
                    iconIndex = 2; // device.ico
                    break;
                case "DataItems":
                case "DataItem":
                    iconIndex = 3; // dataitem.ico
                    break;
                case "Components":
                case "Component":
                case "ComponentStream":
                    iconIndex = 4; // component.ico
                    break;
                case "Condition":
                    iconIndex = 5; // condition.ico
                    break;
                case "Description":
                    iconIndex = 6; // description.ico
                    break;
                case "Events":
                    iconIndex = 7; // event.ico
                    break;
                case "Samples":
                    iconIndex = 8; // sample.ico
                    break;
                default:
                    iconIndex = 0; // default.ico
                    break;
            }
            node.ImageIndex = iconIndex;
            node.SelectedImageIndex = iconIndex;
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
                SelectIcon(node);

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
            or.CheckedChanged += new EventHandler(ActualiserPaths);
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

        private void ActualiserPaths(object sender, EventArgs e)
        {
            if (pathsFromTree.Count != 0)
            {
                GenerationPaths();
            }
            
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

            new ToastContentBuilder().AddText("Path copié dans le presse papier").Show();
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

        // Recherche au sein de la TreeView le texte écrit par l'utilisateur
        private void SearchItem(object sender, EventArgs e)
        {
            string item = ((TextBox)sender).Text;

            foreach (TreeNode node in nodes)
            {
                node.ForeColor = Color.Empty;
                node.BackColor = Color.Empty;

                if (node.Text.IndexOf(item, 0, StringComparison.OrdinalIgnoreCase) != -1 && item != "")
                {
                    //node.ForeColor = Color.Tomato;
                    node.BackColor = Color.Yellow;
                }
            }
        }
        
        private ITag ParseFullPath(string path)
        {
            ITag newTag = new Tag();
            string pathCourant = path.Trim();

            if (pathCourant.EndsWith(")"))
            {
                string[] idCourante = pathCourant.Split('(');
                pathCourant = idCourante[idCourante.Length - 1];
                pathCourant = pathCourant.Remove(pathCourant.Length - 1, 1).Trim();
                newTag.Name = instance.FindTagNameById(tagMachine, pathCourant);
                newTag.Id = pathCourant;
            }
            else
            {
                string[] nomCourant = pathCourant.Split(':');
                newTag = new Tag(nomCourant[0].Trim());
            }
            return newTag;
        }

        private void treeAffichage_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                pathsFromTree.Add(e.Node.FullPath);
            }
            else
            {
                pathsFromTree.Remove(e.Node.FullPath);
            }
            if (pathsFromTree.Count != 0)
            {
                GenerationPaths();
            }
            else
            {
                resultats.Items.Clear();
            }
        }

        private void GenerationPaths()
        {
            ITag tagGeneration = CreateSpecifiqueTag(pathsFromTree);

            List<string> paths = instance.GenererPath(tagGeneration, url, or.Checked);
            resultats.Items.Clear();
            foreach (string path in paths)
            {
                resultats.Items.Add(path + "\n");
            }
        }

        public ITag CreateSpecifiqueTag(List<string> chemins)
        {
            ITag root = new Tag(tagMachine.Name);
            foreach (string chemin in chemins)
            {
                string[] tagActuelString = chemin.Split('\\');
                root = CreateTagRecursive(root, tagActuelString, 0);
            }
            return root;
        }

        private ITag CreateTagRecursive(ITag parent, string[] items, int index)
        {
            ITag tagCourant;
            if (index < items.Length)
            {
                tagCourant = ParseFullPath(items[index]);

                if (EnfantExiste(parent,tagCourant))
                {
                    tagCourant = CreateTagRecursive(parent.Child[parent.Child.IndexOf(tagCourant)], items, index + 1);
                    parent.Child.Remove(tagCourant);
                    parent.AddChild(tagCourant);
                }
                else
                {
                    tagCourant = CreateTagRecursive(tagCourant, items, index + 1);
                    parent.AddChild(tagCourant);
                }           
            }
            return parent;
        }

        private bool EnfantExiste(ITag parent, ITag enfant)
        {
            if (enfant.Id.Equals(""))
            {
                return instance.FindTagByName(parent, enfant.Name) != null;
            }

            return instance.FindTagById(parent, enfant.Id) != null;
        }
    }
}
