using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTConnectAgent.Model;
using MTConnectAgent.BLL;
using System.Xml.Linq;
using System.Threading;

namespace MTConnectAgent
{
    public partial class UserControlProbeCurrent : UserControl
    {
        private ITag tagMachine;
        private string url;
        private functions fx;
        public enum functions
        {
            probe,
            current
        }

        public UserControlProbeCurrent(string url, functions fx)
        {
            this.url = url;
            this.fx = fx;

            this.Anchor = AllSideAnchor;
            this.Location = new Point(0, 0);
            this.Name = "userControl"+fx;
            this.Size = new Size(613, 399);
            this.TabIndex = 0;
            InitializeComponent();
            updateView();
        }

        public void updateView()
        {
            Thread threadCalcul;
            switch (this.fx)
            {
                case functions.probe:
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseProbe(this.url); });
                    break;
                case functions.current:
                default:
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseCurrent(this.url); });
                    break;
            }
            threadCalcul.Start();
            threadCalcul.Join();

            generate(tagMachine.Child, this.flowContent);
        }

        private readonly AnchorStyles TopLeftAnchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left));
        private readonly AnchorStyles AllSideAnchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right));

        private int generate(IList<ITag> tags, Control root)
        {
            int totalHeight = 0;
            foreach (ITag tag in tags)
            {

                //ListBox listBox = new ListBox();
                //root.Controls.Add(listBox);

                //// 
                //// listBox1
                //// 
                //listBox.BorderStyle = BorderStyle.None;
                //listBox.FormattingEnabled = true;
                //listBox.Location = new Point(3, 3);
                //listBox.Name = "listBox1";
                //listBox.Size = new Size(530, 91);
                //listBox.TabIndex = 0;

                GroupBox container = new GroupBox();
                container.Anchor = TopLeftAnchor;
                container.Location = new Point(10, 20);
                string compositeName = tag.Name + tag.GetHashCode();
                container.Name = "container" + compositeName;
                container.Size = new Size(500, 40);
                container.Text = tag.Name;
                root.Controls.Add(container);

                FlowLayoutPanel containerFlow = new FlowLayoutPanel();
                containerFlow.Anchor = AllSideAnchor;
                containerFlow.FlowDirection = FlowDirection.TopDown;
                containerFlow.Location = new Point(10, 20);
                containerFlow.Name = "containerFlow" + compositeName;
                containerFlow.AutoSize = true;
                containerFlow.Width = container.Width;
                container.Controls.Add(containerFlow);

                if (tag.Value != null)
                {
                    Label textValue = new Label();
                    textValue.AutoSize = true;
                    textValue.Location = new Point(10, 10);
                    textValue.Name = "value" + compositeName;
                    textValue.TabIndex = 0;
                    textValue.Text = tag.Value;
                    containerFlow.Controls.Add(textValue);
                }

                if (tag.HasAttributs())
                {
                    TableLayoutPanel attributTable = new TableLayoutPanel();
                    attributTable.Anchor = AllSideAnchor;
                    attributTable.ColumnCount = 2;
                    attributTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    attributTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    attributTable.Location = new Point(0, 0);
                    attributTable.Name = "attributTable" + compositeName;
                    attributTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                    attributTable.AutoSize = true;
                    attributTable.Width = containerFlow.Width - 40;
                    containerFlow.Controls.Add(attributTable);

                    foreach (KeyValuePair<string, string> attribut in tag.Attributs)
                    {
                        int currentRow = attributTable.RowCount;
                        attributTable.RowCount += 1;
                        attributTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

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
                    List<ITag> c = new List<ITag>();
                    c.Add(tag.Child[0]);
                    container.Height += generate(tag.Child, containerFlow);

                    //Label textValue = new Label();
                    //textValue.AutoSize = true;
                    //textValue.Location = new Point(10, 10);
                    //textValue.Name = "textValue";
                    //textValue.TabIndex = 0;
                    //textValue.Text = "textValue";
                    //container.Controls.Add(textValue);
                }

                totalHeight += container.Height;
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

        private static void ThreadAffichage()
        {

        }

    }
}
