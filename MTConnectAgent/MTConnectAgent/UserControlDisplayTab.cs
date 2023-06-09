﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MTConnectAgent.Model;
using MTConnectAgent.BLL;
using System.Xml.Linq;
using System.Threading;
using System;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Text;

namespace MTConnectAgent
{
    /// <summary>
    /// Gere les interactions entre l'interface et la logique métier
    /// </summary>
    public partial class UserControlDisplayTab : UserControl
    {
        private Tag tagMachine;
        private string url;
        private Functions fx;
        public enum Functions
        {
            probe,
            current,
            path
        }

        /// <summary>
        /// Checkbox pour choisir l'option or 
        /// </summary>
        private CheckBox or = new CheckBox();

        /// <summary>
        /// Bouton radio permettant de choisir le protocole current, il est coché par défaut
        /// </summary>
        private RadioButton currentRadio = new RadioButton();

        /// <summary>
        /// Bouton radio permettant de choisir le protocole sample.
        /// </summary>
        private RadioButton sampleRadio = new RadioButton();

        /// <summary>
        /// Déselectionne toutes les checkboxs cochées
        /// </summary>
        private Button decocher = new Button();

        private Button copyAll = new Button();
               

        /// <summary>
        /// Liste des paths générés
        /// </summary>
        private ListBox resultats = new ListBox();

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
        public UserControlDisplayTab(string url, Functions fx)
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
                case Functions.probe: // Probe
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                case Functions.path: // Path
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                default: // Default => functions.current
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseCurrent(this.url); });
                    break;
            }
            threadCalcul.Start();
            threadCalcul.Join();

            Cursor.Current = Cursors.Default;

            if (this.fx.Equals(Functions.path))
            {
                treeAffichage.Height = this.Height - 200;
                treeAffichage.CheckBoxes = true;
                GeneratePathResults();
            }

            btnExpandNodes.MouseClick += new MouseEventHandler(ExpandNodes);
            btnCollapseNodes.MouseClick += new MouseEventHandler(CollapseNodes);
            rechercheBox.TextChanged += new EventHandler(SearchItem);

            // Ajout des icones pour les tags dans la TreeView
            imageListIcons.Images.Add(Properties.Resources._default);
            imageListIcons.Images.Add(Properties.Resources.header);
            imageListIcons.Images.Add(Properties.Resources.device);
            imageListIcons.Images.Add(Properties.Resources.dataitem);
            imageListIcons.Images.Add(Properties.Resources.component);
            imageListIcons.Images.Add(Properties.Resources.condition);
            imageListIcons.Images.Add(Properties.Resources.description);
            imageListIcons.Images.Add(Properties.Resources._event);
            imageListIcons.Images.Add(Properties.Resources.sample);
            treeAffichage.ImageList = imageListIcons;

            foreach (Tag tag in tagMachine.Child)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                TreeNode node = new TreeNode
                {
                    Name = "node" + compositeName,
                    Text = tag.Name,
                    NodeFont = boldFont,
                    Tag = new SimpleTag(tag.Name, tag.Id)
                };
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
                TreeNode node = new TreeNode
                {
                    Name = "node" + compositeName,
                    NodeFont = boldFont,
                    Tag = new SimpleTag(tag.Name, tag.Id)
                };

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
            decocher.AutoSize = true;
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

            copyAll.Name = "copyAll";
            copyAll.Text = "Tout copier";
            copyAll.AutoSize = true;
            containerFlow.Controls.Add(copyAll);
            copyAll.MouseClick += new MouseEventHandler(CopyAllUrl);

            // Affichage du ou des PATH(S) à chaque checkbox cochée
            resultats.Name = "listResultatsPath";
            resultats.HorizontalScrollbar = true;

            resultats.Location = new Point(5, containerFlow.Location.Y + containerFlow.Height);
            resultats.Size = new Size(containerFlow.Width - 10, 155);
            resultats.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resultats.TabIndex = 2;
            resultats.MouseDoubleClick += new MouseEventHandler(OpenBrowser);
            resultats.MouseDown += new MouseEventHandler(CopyUrl);
            container.Controls.Add(resultats);
        }

        /// <summary>
        /// Copie le path sélectionné dans le presse-papier lors d'un double clic de la souris
        /// </summary>
        /// <param name="o">Object appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void CopyUrl(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                int index = this.resultats.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    resultats.SelectedItem =  resultats.Items[index];
                    Clipboard.SetText(resultats.SelectedItem.ToString());
                    new ToastContentBuilder().AddText("Path copié dans le presse papier").Show();
                }
            }

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
            try
            {
                XDocument t = mtConnectClient.GetProbeAsync(url).Result;
                return mtConnectClient.ParseXMLRecursif(t.Root);
            }
            catch (System.AggregateException e) // une erreur est levé lors de l'appelle
            {
                new ToastContentBuilder().AddText(e.InnerException.Message).Show();
                return new Tag();
            }
        }

        /// <summary>
        /// Appelle l'url du current de façon asynchrone
        /// </summary>
        /// <param name="url">url de la machine courante</param>
        /// <returns>Tag contenant la totalité des information du current</returns>
        private static Tag ThreadParseCurrent(string url)
        {
            try
            {
                MTConnectClient mtConnectClient = new MTConnectClient();
                XDocument t = mtConnectClient.GetCurrentAsync(url).Result;
                return mtConnectClient.ParseXMLRecursif(t.Root);
            }
            catch (System.AggregateException e) // une erreur est levé lors de l'appelle
            {
                new ToastContentBuilder().AddText(e.InnerException.Message).Show();
                return new Tag();
            }
        }

        /// <summary> 
        /// Copie tous les paths générés lors d'un clic en dehors d'un item de la ListBox
        /// </summary>
        /// <param name="o">Object appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void CopyAllUrl(object o, MouseEventArgs e)
        {
            StringBuilder urlSelectionne = new StringBuilder("");

            if (resultats.Items.Count == 0)
            {
                return;
            }

            // Récupération de tous les paths
            foreach (string item in resultats.Items)
            {
                urlSelectionne.Append(item);
            }

            Clipboard.SetText(urlSelectionne.ToString());
            new ToastContentBuilder().AddText("Tous les paths ont été copiés dans le presse papier").Show();
        }

        /// <summary>
        /// Ouvre le path sélectionné dans un navigateur lors d'un double clic de la souris
        /// </summary>
        /// <param name="o">Object appellant</param>
        /// <param name="e">Evenenement provoqué</param>
        private void OpenBrowser(object o, MouseEventArgs e)
        {
            ListBox listView = (ListBox)o;
            if (listView.SelectedItem != null)
            {
                System.Diagnostics.Process.Start(listView.SelectedItem.ToString());
            }
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
        private void TreeAffichage_AfterCheck(object sender, TreeViewEventArgs e)

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
                    resultats.Items.Add(path);
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
