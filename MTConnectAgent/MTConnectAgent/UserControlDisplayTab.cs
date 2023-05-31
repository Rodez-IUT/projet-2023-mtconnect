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
        private Tag tagMachine;
        private string url;
        private functions fx;
        public enum functions
        {
            probe,
            current,
            path
        }

        /// <summary>
        /// Checkbox pour choisir l'option or 
        /// </summary>
        private CheckBox or = new CheckBox();

        private RadioButton currentRadio = new RadioButton();

        private RadioButton sampleRadio = new RadioButton();

        /// <summary>
        /// Déselectionne toutes les checkboxs cochées
        /// </summary>
        private Button decocher = new Button();

        /// <summary>
        /// Boutons radio pour le choix du protocole
        /// </summary>
        private GroupBox choixProtocole = new GroupBox();

        private RadioButton sampleRadio = new RadioButton();

        /// <summary>
        /// Liste des paths générés
        /// </summary>
        private ListView resultats = new ListView();

        /// <summary>
        /// Instance courante du client MTConnect
        /// </summary>
        private MTConnectClient instance = new MTConnectClient();

        /// <summary>
        /// Liste des paths bruts récupérés sous forme de string via le treeview
        /// </summary>
        private List<string> pathsFromTree = new List<string>();

        /// <summary>
        /// Liste des noeuds du treeview
        /// </summary>
        private IList<TreeNode> nodes = new List<TreeNode>();

        private MTConnectClient.Protocol protocole;

        private Dictionary<string, MTConnectClient.Protocol> dictionnaireProtocoles = new Dictionary<string, MTConnectClient.Protocol>()
        {
            {"Probe",MTConnectClient.Protocol.probe },
            {"Current", MTConnectClient.Protocol.current },
            {"Sample", MTConnectClient.Protocol.sample }
        };


        /// <summary>
        /// Initialise l'interface actuelle
        /// </summary>
        /// <param name="url">url de la machine choisie</param>
        /// <param name="fx">type de requete (probe, current, path)</param>
        public UserControlDisplayTab(string url, functions fx)
        {
            this.url = url;
            this.fx = fx;

            this.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
            imageList1.Images.Add(Properties.Resources.component);
            imageList1.Images.Add(Properties.Resources.condition);
            imageList1.Images.Add(Properties.Resources.dataitem);
            imageList1.Images.Add(Properties.Resources._default);
            imageList1.Images.Add(Properties.Resources.description);
            imageList1.Images.Add(Properties.Resources.device);
            imageList1.Images.Add(Properties.Resources._event);
            imageList1.Images.Add(Properties.Resources.header);
            imageList1.Images.Add(Properties.Resources.sample);
            treeAffichage.ImageList = imageList1;

            foreach (Tag tag in tagMachine.Child)
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

        /// <summary>
        /// Affichage sous la forme d'un arbre des tags avec leurs attributs, id et nom
        /// </summary>
        /// <param name="tags">Les tags à afficher</param>
        /// <param name="root">Le noeud racine du treeview</param>
        private void Generer(IList<Tag> tags,TreeNode root)
        {
            foreach (Tag tag in tags)
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

        /// <summary>
        /// Fenêtre de génération et obtention des PATHS
        /// </summary>
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
            or.Checked = true;
            or.CheckedChanged += new EventHandler(ActualiserPaths);
            containerFlow.Controls.Add(or);

            // Décocher toutes les checkboxs sélectionnées
            decocher.Name = "decocher";
            decocher.Text = "Décocher";
            decocher.Anchor = AnchorStyles.None;
            decocher.MouseClick += new MouseEventHandler(Deselect);
            containerFlow.Controls.Add(decocher);

            // Choix du protocole
            currentRadio.Name = "current";
            currentRadio.Text = "Current";
            currentRadio.AutoSize = true;
            sampleRadio.Name = "sample";
            sampleRadio.Text = "Sample";
            sampleRadio.AutoSize = true;
            containerFlow.Controls.Add(currentRadio);
            containerFlow.Controls.Add(sampleRadio);
            currentRadio.Checked = true;
            protocole = MTConnectClient.Protocol.current;
            currentRadio.CheckedChanged += new EventHandler(ChangementProtocole);
            sampleRadio.CheckedChanged += new EventHandler(ChangementProtocole);

            // Affichage du ou des PATH(S) à chaque checkbox cochée
            resultats.Name = "listResultatsPath";
            resultats.Location = new Point(5, containerFlow.Location.Y + containerFlow.Height);
            resultats.Size = new Size(containerFlow.Width - 10, 155);
            resultats.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultats.View = View.List;
            resultats.TabIndex = 2;
            resultats.MouseDoubleClick += new MouseEventHandler(OpenBrowser);
            resultats.MouseDown += new MouseEventHandler(CopyAllUrl);
            resultats.MouseClick += new MouseEventHandler(CopyUrl);
            container.Controls.Add(resultats);
        }

        private void ChangementProtocole(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (!dictionnaireProtocoles.TryGetValue(radioButton.Text, out protocole))
            {
                throw new ArgumentException("Un erreur est survenue lors du choix du protocole");
            }
            GenerationPaths();
        }

        private void ChangementProtocole(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (!dictionnaireProtocoles.TryGetValue(radioButton.Text, out protocole))
            {
                throw new ArgumentException("Un erreur est survenue lors du choix du protocole");
            }
            GenerationPaths();
        }

        /// <summary>
        /// Actualise les paths lors du clic sur le bouton "or"
        /// </summary>
        /// <param name="sender">Objet appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void ActualiserPaths(object sender, EventArgs e)
        {
            GenerationPaths();
        }

        /// <summary>
        /// Appelle l'url du probe de façon asynchrone
        /// </summary>
        /// <param name="url">url de la machine courante</param>
        /// <returns> Tag contenant la totalité des information du probe</returns>
        private static Tag ThreadParseProbe(string url)
        {
            MTConnectClient mtConnectClient = new MTConnectClient();
            XDocument t = mtConnectClient.getProbeAsync(url).Result;
            return mtConnectClient.ParseXMLRecursif(t.Root);
        }

        /// <summary>
        /// Appelle l'url du current de façon asynchrone
        /// </summary>
        /// <param name="url">url de la machine courante</param>
        /// <returns>Tag contenant la totalité des information du current</returns>
        private static Tag ThreadParseCurrent(string url)
        {
            MTConnectClient mtConnectClient = new MTConnectClient();
            XDocument t = mtConnectClient.getCurrentAsync(url).Result;
            return mtConnectClient.ParseXMLRecursif(t.Root);
        }

        /// <summary>
        /// Copie le path sélectionné dans le presse-papier lors d'un double clic de la souris
        /// </summary>
        /// <param name="o">Object appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void CopyUrl(object o, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListView listView = (ListView)o;
                Clipboard.SetText(listView.FocusedItem.Text);
                new ToastContentBuilder().AddText("Path copié dans le presse papier").Show();
            }
        }

        /// <summary> 
        /// Copie tous les paths générés lors d'un clic en dehors d'un item de la ListView
        /// </summary>
        /// <param name="o">Object appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void CopyAllUrl(object o, MouseEventArgs e)
        {
            string url = "";

            ListView listView = (ListView)o;
            ListViewHitTestInfo info = listView.HitTest(e.Location);

            // Clic dans une zone ne contenant pas d'item
            if (info.Location == ListViewHitTestLocations.None)
            {
                // Récupération de tous les paths
                foreach (ListViewItem item in listView.Items)
                {
                    url = url + item.Text + "\n";
                }

                Clipboard.SetText(url);
                new ToastContentBuilder().AddText("Tous les paths ont été copiés dans le presse papier").Show();
            }
        } 

        /// <summary>
        /// Ouvre le path sélectionné dans un navigateur lors d'un double clic de la souris
        /// </summary>
        /// <param name="o">Object appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void OpenBrowser(object o, MouseEventArgs e)
        {
            ListView listView = (ListView)o;
            System.Diagnostics.Process.Start(listView.FocusedItem.Text);
        }

        /// <summary>
        /// Réduire l'affichage de l'arbre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseNodes(object sender, MouseEventArgs e)
        {
            treeAffichage.CollapseAll();
        }

        /// <summary>
        /// Développe l'affichage de l'arbre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandNodes(object sender, MouseEventArgs e)
        {
            treeAffichage.ExpandAll();
        }

        /// <summary>
        /// Décoche toutes les checkbox sélectionnées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Deselect(object sender, MouseEventArgs e)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
            }
        }

        /// <summary>
        /// Recherche au sein de la TreeView le texte écrit par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchItem(object sender, EventArgs e)
        {
            string item = ((TextBox)sender).Text;

            foreach (TreeNode node in nodes)
            {
                node.ForeColor = Color.Empty;
                node.BackColor = Color.Empty;

                if (node.Text.IndexOf(item, 0, StringComparison.OrdinalIgnoreCase) != -1 && item != "")
                {
                    node.BackColor = Color.Yellow;
                }
            }
        }
        
        /// <summary>
        /// Transforme un item sous forme de string en tag, selon son id et son nom
        /// </summary>
        /// <param name="path">L'item à transformer</param>
        /// <returns>Un tag constitué uniquement d'un nom et d'un id</returns>
        private Tag ParseFullPath(string path)
        {
            Tag newTag = new Tag();
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


        /// <summary>
        /// Méthode appelée dès que le treeview est coché ou décocher, initialise et lance la génération du path
        /// </summary>
        /// <param name="sender">Object appelant (la treeview)</param>
        /// <param name="e">Evenement provoqué</param>
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
            GenerationPaths();
        }

        /// <summary>
        /// Génére les paths des items demandés et les ajoute à la fenêtre path
        /// </summary>
        private void GenerationPaths()
        {
            if (pathsFromTree.Count != 0)
            {
                Tag tagGeneration = CreateSpecifiqueTag(pathsFromTree);

                List<string> paths = instance.GenererPath(tagGeneration, url, or.Checked, protocole);
                resultats.Items.Clear();
                foreach (string path in paths)
                {
                    resultats.Items.Add(path + "\n");
                }
            }
            else
            {
                resultats.Items.Clear();
            }
        }

        /// <summary>
        /// Créer un tag spécifique pour la génération des paths
        /// </summary>
        /// <param name="chemins">Les chemins donnés par le treeview</param>
        /// <returns>Un tag spécial pour la génération des paths</returns>
        public Tag CreateSpecifiqueTag(List<string> chemins)
        {
            Tag root = new Tag(tagMachine.Name);
            foreach (string chemin in chemins)
            {
                string[] tagActuelString = chemin.Split('\\');
                root = CreateTagRecursive(root, tagActuelString, 0);
            }
            return root;
        }

        /// <summary>
        /// Créer un tag correspondant à un chemin envoyé (ex : Devices/Device), et si certains items du chemin existe déjà dans le tag parent donné,
        /// alors ils ne sont pas dédoublés
        /// </summary>
        /// <param name="parent">Le tag parent dans lequel on ajoute les enfant si il y en a</param>
        /// <param name="items">Le chemin demandé séparé par item</param>
        /// <param name="index">L'index courant</param>
        /// <returns> Un tag contenant tout les items demandés</returns>
        private Tag CreateTagRecursive(Tag parent, string[] items, int index)
        {
            Tag tagCourant;
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

        /// <summary>
        /// Vérifie si le tag enfant est présent en tant qu'enfant direct du parent
        /// </summary>
        /// <param name="parent">Le tag parent</param>
        /// <param name="enfant">Le tag enfant recherché</param>
        /// <returns>true si il est trouvé, false sinon</returns>
        private bool EnfantExiste(Tag parent, Tag enfant)
        {
            if (enfant.Id.Equals(""))
            {
                return instance.FindTagByName(parent, enfant.Name) != null;
            }

            return instance.FindTagById(parent, enfant.Id) != null;
        }
    }
}
