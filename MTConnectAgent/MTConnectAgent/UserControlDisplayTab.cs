using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MTConnectAgent.Model;
using MTConnectAgent.BLL;
using System.Xml.Linq;
using System.Threading;
using System;
using static System.Windows.Forms.ListView;

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

        // Utilisation de l'option OR ou non
        private CheckBox or = new CheckBox();

        // Affichage des paths
        private ListView resultats = new ListView();

        // Checkboxs des items
        private IList<CheckBox> interfaceTags = new List<CheckBox>();

        public UserControlDisplayTab(string url, functions fx)
        {
            this.url = url;
            this.fx = fx;

            this.Anchor = AllSideAnchor;
            this.Location = new Point(0, 0);
            this.Name = "userControl"+fx;
            this.Size = new Size(613, 399);
            this.TabIndex = 0;
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            Thread threadCalcul;
            switch (this.fx)
            {
                case functions.probe:
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                case functions.path:
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                default: // Default => functions.current
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseCurrent(this.url); });
                    break;
            }
            threadCalcul.Start();
            threadCalcul.Join();

            // Génération de l'affichage des requêtes
            Generate(tagMachine.Child, this.flowContent);

            // Si la fenêtre PATH est sélectionnée, on génère également l'interface de résultat des paths
            if (this.fx.Equals(functions.path))
            {
                GeneratePathResults(this.flowContent);
            }
        }

        private readonly AnchorStyles TopLeftAnchor = AnchorStyles.Top | AnchorStyles.Left;
        private readonly AnchorStyles AllSideAnchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        // Fenêtre de génération et obtention des PATHS
        private void GeneratePathResults(Control root)
        {
            GroupBox container = new GroupBox();
            container.Anchor = TopLeftAnchor;
            container.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            container.Location = new Point(0, 0);
            container.Name = "containerPath";
            container.Size = new Size(500, 200);
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

            // Utilisation de l'option OR ou non
            or.AutoSize = true;
            or.Name = "checkboxOr";
            or.Text = "Avec option OR";
            or.Anchor = AnchorStyles.None;
            containerFlow.Controls.Add(or);

            // Affichage du ou des PATH(S) à chaque checkbox cochée
            resultats.Name = "listResultatsPath";
            resultats.Location = new Point(10, 70);
            resultats.Size = new Size(480, 120);
            resultats.View = View.List;
            resultats.MouseDoubleClick += new MouseEventHandler(CopyUrl);
            container.Controls.Add(resultats);
        }

        // Récupère la liste des tags sélectionnés pour générer le(s) PATH(S)
        private IList<ITag> GetTagsList(IList<CheckBox> interfaceTags)
        {
            IList<ITag> tags = new List<ITag>();
            
            foreach(CheckBox checkBox in interfaceTags)
            {
                if (checkBox.Checked)
                {
                    tags.Add((SimpleTag)checkBox.Tag);
                }
            }
            return tags;
        }

        // Génère le ou les PATHS 
        private void GeneratePaths(object sender, EventArgs e)
        {
            // Récupération de la liste de tags à utiliser pour générer le(s) PATH(S)
            IList<ITag> tags = GetTagsList(interfaceTags);

            resultats.Clear();
                
            // Génération de plusieurs PATHS
            var instance = new MTConnectClient();

            foreach (ITag tag in tags)
            {
                string pathUrl = "";
                Thread thread = new Thread(() => { pathUrl = ThreadGeneratePath(tag, tagMachine, url, or.Checked, instance); });
                thread.Start();
                thread.Join();
                // Recherche et affichage des urls
                resultats.Items.Add(pathUrl + "\r\n");
            }
        }

        // Affichage du résultat de la requête Probe ou Current
        private int Generate(IList<ITag> tags, Control root)
        {            
            int totalHeight = 0;
            foreach (ITag tag in tags)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                if (!tag.HasAttributs() && !tag.HasChild() && tag.Value != null && tag.Value.Trim() != "")
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.AutoSize = true;
                    checkBox.Name = "nameValue" + compositeName + tag.Value;
                    checkBox.TabIndex = 0;
                    checkBox.Tag = new SimpleTag(tag.Name, tag.Id);
                    checkBox.Text = tag.Name + " : " + tag.Value;
                    root.Controls.Add(checkBox);

                    interfaceTags.Add(checkBox);

                    totalHeight += checkBox.Height;
                } else
                {
                    int coord = 20;
                    
                    // Affichage pour la fenêtre PATH : checkboxs pour préparer la génération du ou des PATH(S)
                    if (this.fx == functions.path)
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Name = "name" + compositeName;
                        checkBox.AutoSize = true;
                        checkBox.Height = 10;
                        checkBox.Text = tag.Name;
                        checkBox.Tag = new SimpleTag(tag.Name, tag.Id);
                        checkBox.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                        checkBox.Width = root.Width - coord * 2;
                        checkBox.CheckedChanged += new EventHandler(this.GeneratePaths);
                        root.Controls.Add(checkBox);

                        // Ajout à la liste de toutes les checkboxs pour la génération du ou des PATH(S)
                        interfaceTags.Add(checkBox);

                        if (tag.Value != null && tag.Value.Trim() != "")
                        {
                            checkBox.Text += " : " + tag.Value;
                        }
                    }
                    else
                    {
                        // Affichage pour les fenêtres Probe et Current
                        Label name = new Label();
                        name.AutoSize = true;
                        name.Name = "name" + compositeName;
                        name.Text = tag.Name;
                        name.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

                        if (tag.Value != null && tag.Value.Trim() != "")
                        {
                            name.Text += " : " + tag.Value;
                        }
                    }

                    Panel container = new Panel();
                    container.Anchor = TopLeftAnchor;
                    container.Location = new Point(0, 0);
                    container.Name = "container" + compositeName;
                    container.Height = 10;
                    container.Text = tag.Name;
                    container.Width = root.Width - coord * 2;
                    root.Controls.Add(container);
                    
                    FlowLayoutPanel containerFlow = new FlowLayoutPanel();
                    containerFlow.Anchor = TopLeftAnchor;
                    containerFlow.FlowDirection = FlowDirection.TopDown;
                    containerFlow.Location = new Point(coord, 0);
                    containerFlow.Name = "containerFlow" + compositeName;
                    containerFlow.AutoSize = true;
                    containerFlow.Width = container.Width;
                    container.Controls.Add(containerFlow);
                    
                    if (tag.HasAttributs())
                    {
                        TableLayoutPanel attributTable = new TableLayoutPanel();
                        attributTable.ColumnCount = 2;
                        attributTable.Location = new Point(0, 0);
                        attributTable.Name = "attributTable" + compositeName;
                        attributTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                        attributTable.AutoSize = true;
                        containerFlow.Controls.Add(attributTable);
                        
                        foreach (KeyValuePair<string, string> attribut in tag.Attributs)
                        {
                            int currentRow = attributTable.RowCount;
                            attributTable.RowCount += 1;

                            Label attributKey = new Label();
                            attributKey.AutoSize = true;
                            attributKey.Name = "attributKey" + compositeName + attribut.Key;
                            attributKey.TabIndex = 0;
                            attributKey.Text = attribut.Key;
                            attributTable.Controls.Add(attributKey, 0, currentRow);

                            Label attributValue = new Label();
                            attributValue.AutoSize = true;
                            attributValue.Name = "attributValue" + compositeName + attribut.Value;
                            attributValue.TabIndex = 0;
                            attributValue.Text = attribut.Value;
                            attributTable.Controls.Add(attributValue, 1, currentRow);
                        }

                        container.Height += attributTable.Height;
                    }

                    if (tag.HasChild())
                    {
                        container.Height += Generate(tag.Child, containerFlow) + 10;
                    }

                    containerFlow.Height = container.Height;
                    totalHeight += container.Height;
                }
            }

            return totalHeight;
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
    }
}
