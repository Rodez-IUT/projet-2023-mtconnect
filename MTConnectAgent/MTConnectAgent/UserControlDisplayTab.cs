using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MTConnectAgent.Model;
using MTConnectAgent.BLL;
using System.Xml.Linq;
using System.Threading;
using System;

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

        private IList<ITag> specificTags = new List<ITag>(); 

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
                GeneratePathResults(this);
            }
            foreach (ITag tag in tagMachine.Child)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                TreeNode node = new TreeNode();
                node.Name = "node" + compositeName;
                node.Text = tag.Name;
                node.NodeFont = boldFont;
                node.Tag = new SimpleTag(tag.Name, tag.Id);

                treeAffichage.Nodes.Add(node);

                if (tag.HasChild())
                {
                    genV2(tag.Child, node);
                }
            }
        }

        private readonly AnchorStyles TopLeftAnchor = AnchorStyles.Top | AnchorStyles.Left;
        private readonly AnchorStyles BottomLeftAnchor = AnchorStyles.Bottom | AnchorStyles.Left;
        private readonly AnchorStyles AllSideAnchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        private readonly Font boldFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

        private void genV2(IList<ITag> tags,TreeNode root)
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

                if (tag.HasChild())
                {
                    genV2(tag.Child, node);
                }
            }
        }

        // Fenêtre de génération et obtention des PATHS
        private void GeneratePathResults(Control root)
        {
            GroupBox container = new GroupBox();
            container.Anchor = BottomLeftAnchor;
            container.Dock = DockStyle.Bottom;
            container.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            container.Location = new Point(0, root.Height-200);
            container.Name = "containerPath";
            //container.Size = new Size(500, 200);
            container.Height = 200;
            container.Width = root.Width;
            container.Text = "Résultat des paths";
            root.Controls.Add(container);

            FlowLayoutPanel containerFlow = new FlowLayoutPanel();
            containerFlow.Anchor = TopLeftAnchor;
            containerFlow.FlowDirection = FlowDirection.LeftToRight;
            containerFlow.Location = new Point(10, 20);
            containerFlow.Name = "containerFlow";
            containerFlow.Size = new Size(10, 10);
            containerFlow.AutoSize = true;
            container.Controls.Add(containerFlow);

            CheckBox or = new CheckBox();
            // Utilisation de l'option OR ou non
            or.AutoSize = true;
            or.Name = "checkboxOr";
            or.Text = "Avec option OR";
            or.Anchor = AnchorStyles.None;
            containerFlow.Controls.Add(or);

            ListView resultats = new ListView();
            // Affichage du ou des PATH(S) à chaque checkbox cochée
            resultats.Name = "listResultatsPath";
            resultats.Location = new Point(10, 70);
            resultats.Size = new Size(480, 120);
            resultats.View = View.List;
            resultats.MouseDoubleClick += new MouseEventHandler(CopyUrl);
            container.Controls.Add(resultats);
            
        }

        // Génère le ou les PATHS 
        private void GeneratePaths(object sender, EventArgs e)
        {
            // Récupération de la liste de tags à utiliser pour générer le(s) PATH(S)
            //IList<ITag> tags = GetTagsList(interfaceTags);

            //resultats.Clear();
                
            //// Génération de plusieurs PATHS
            //var instance = new MTConnectClient();

            //foreach (ITag tag in tags)
            //{
            //    string pathUrl = "";
            //    Thread thread = new Thread(() => { pathUrl = ThreadGeneratePath(tag, tagMachine, url, or.Checked, instance); });
            //    thread.Start();
            //    thread.Join();
            //    // Recherche et affichage des urls
            //    resultats.Items.Add(pathUrl + "\r\n");
            //}
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

        private static string ThreadGeneratePath(ITag tag, ITag tagMachine, string url, bool or, MTConnectClient instance)
        {

            Queue<string> queue = new Queue<string>();
            if (tag.Id != null && tag.Id != "")
            {
                queue.Enqueue(tag.Id);
            }
            else
            {
                queue.Enqueue(tag.Name);
            }

            ITag aRechercher = instance.CreateSpecifiqueTag(tagMachine, queue);

            return instance.GenererPath(aRechercher, url, or);
        }

        private void CopyUrl(object o, MouseEventArgs e)
        {
            ListView listView = (ListView)o;
            Clipboard.SetText(listView.FocusedItem.Text);
            copyNotification.ShowBalloonTip(1000, "MTConnect", "Le path a été copié dans le presse-papier.", ToolTipIcon.Info);
        }

        private ITag ParseFullPath(string path)
        {
            string[] splitPath = path.Split('\\');
            IList<string> returnedList = new List<string>();
            Queue<string> queue = new Queue<string>();

            foreach (string todoRenommer in splitPath)
            {
                string todoRenommer2 = todoRenommer.Trim();
                if (todoRenommer.Contains(":"))
                {
                    string[] todoRenommer3 = todoRenommer.Trim().Split(':');
                    todoRenommer2 = todoRenommer3[todoRenommer3.Length - 1];
                    todoRenommer2 = todoRenommer2.Remove(todoRenommer2.Length - 1, 1).Trim();
                }

                if (todoRenommer.Trim().EndsWith(")"))
                {
                    string[] todoRenommer3 = todoRenommer.Trim().Split('(');
                    todoRenommer2 = todoRenommer3[todoRenommer3.Length-1];
                    todoRenommer2 = todoRenommer2.Remove(todoRenommer2.Length - 1, 1).Trim();
                }

                returnedList.Add(todoRenommer2);
                queue.Enqueue(todoRenommer2);
            }

            MTConnectClient instance = new MTConnectClient();

            return instance.CreateSpecifiqueTag(tagMachine, queue);
        }

        private void treeAffichage_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //MTConnectClient instance = new MTConnectClient();

            specificTags.Add(ParseFullPath(e.Node.FullPath));
            //instance.GenererPath(aRechercher, url, or);
        }
    }
}
