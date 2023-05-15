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
    public partial class UserControlProbe : UserControl
    {
        private ITag tagMachine;

        public UserControlProbe()
        {
            InitializeComponent();
            updateView();
        }

        public void updateView()
        {
            Thread threadCalcul = new Thread(() => { this.tagMachine = ThreadParse(); });
            threadCalcul.Start();
            threadCalcul.Join();
            generate(tagMachine.Child);
        }

        private readonly AnchorStyles TopLeftAnchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left));
        private readonly AnchorStyles AllSideAnchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right));

        private int generate(IList<ITag> tags, GroupBox root = null)
        {
            int totalHeight = 0;
            foreach (ITag tag in tags)
            {
                GroupBox container = new GroupBox();
                container.Anchor = TopLeftAnchor;
                container.Location = new Point(10, 20);
                container.Name = "container" + tag.Name;
                container.Size = new Size(500, 0);
                container.Text = tag.Name;
                if (root == null)
                {
                    this.flowContent.Controls.Add(container);
                }
                else
                {
                    root.Controls.Add(container);
                }

                FlowLayoutPanel containerFlow = new FlowLayoutPanel();
                containerFlow.Anchor = AllSideAnchor;
                containerFlow.FlowDirection = FlowDirection.TopDown;
                containerFlow.Location = new Point(10, 20);
                containerFlow.Name = "containerFlow" + tag.Name;
                containerFlow.AutoSize = true;
                containerFlow.Width = container.Width;
                container.Controls.Add(containerFlow);

                if (tag.HasAttributs())
                {
                    TableLayoutPanel attributTable = new TableLayoutPanel();
                    attributTable.Anchor = AllSideAnchor;
                    attributTable.ColumnCount = 2;
                    attributTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    attributTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    attributTable.Location = new Point(0, 0);
                    attributTable.Name = "attributTable" + tag.Name;
                    attributTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                    attributTable.AutoSize = true;
                    attributTable.Width = containerFlow.Width - 40;
                    containerFlow.Controls.Add(attributTable);

                    foreach (KeyValuePair<string, string> attribut in tag.Attributs)
                    {
                        int currentRow = attributTable.RowCount;
                        attributTable.RowCount += 1;
                        attributTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                        Label textKey = new Label();
                        textKey.AutoSize = true;
                        textKey.Name = "textKey" + attribut.Key;
                        textKey.TabIndex = 0;
                        textKey.Text = attribut.Key;
                        attributTable.Controls.Add(textKey, 0, currentRow);


                        Label textValue = new Label();
                        textValue.AutoSize = true;
                        textValue.Name = "textValue" + attribut.Value;
                        textValue.TabIndex = 0;
                        textValue.Text = attribut.Value;
                        attributTable.Controls.Add(textValue, 1, currentRow);
                    }

                    container.Height += 40 + attributTable.Height;
                }

                if (tag.HasChild())
                {
                    container.Height += 40;
                    List<ITag> c = new List<ITag>();
                    c.Add(tag.Child[0]);
                    container.Height += generate(c, container);
                }

                totalHeight += container.Height;
            }

            return totalHeight;
        }

        private static ITag ThreadParse()
        {
            MTConnectClient mtConnectClient = new MTConnectClient();
            XDocument t = mtConnectClient.getProbeAsync("https://smstestbed.nist.gov/vds/").Result;
            return mtConnectClient.ParseXMLRecursif(t.Root);
        }
    }
}
