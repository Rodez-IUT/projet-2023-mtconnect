﻿using System;
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
                case functions.current:
                default:
                    threadCalcul = new Thread(() => { this.tagMachine = ThreadParseCurrent(this.url); });
                    break;
            }
            threadCalcul.Start();
            threadCalcul.Join();

            Generate(tagMachine.Child, this.flowContent);
        }

        private readonly AnchorStyles TopLeftAnchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left));
        private readonly AnchorStyles AllSideAnchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right));

        private int Generate(IList<ITag> tags, Control root)
        {
            int totalHeight = 0;
            foreach (ITag tag in tags)
            {
                string compositeName = tag.Name + tag.GetHashCode();
                if (!tag.HasAttributs() && !tag.HasChild() && tag.Value != null && tag.Value.Trim() != "")
                {
                    Label nameValue = new Label();
                    nameValue.AutoSize = true;
                    nameValue.Name = "nameValue" + compositeName + tag.Value;
                    nameValue.TabIndex = 0;
                    nameValue.Text = tag.Name + " : " + tag.Value;
                    root.Controls.Add(nameValue);

                    totalHeight += nameValue.Height;
                } else
                {
                    int coord = 20;
                    Panel container = new Panel();
                    container.Anchor = TopLeftAnchor;
                    container.Location = new Point(0, 0);
                    container.Name = "container" + compositeName;
                    container.Height = 10;
                    container.Text = tag.Name;
                    container.Width = root.Width - coord * 2;
                    //container.BorderStyle = BorderStyle.FixedSingle;
                    root.Controls.Add(container);


                    FlowLayoutPanel containerFlow = new FlowLayoutPanel();
                    containerFlow.Anchor = TopLeftAnchor;
                    containerFlow.FlowDirection = FlowDirection.TopDown;
                    containerFlow.Location = new Point(coord, 0);
                    containerFlow.Name = "containerFlow" + compositeName;
                    containerFlow.AutoSize = true;
                    containerFlow.Width = container.Width;
                    container.Controls.Add(containerFlow);

                    Label name = new Label();
                    name.AutoSize = true;
                    name.Name = "name" + compositeName;
                    name.Text = tag.Name;
                    name.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    //name.BorderStyle = BorderStyle.FixedSingle;
                    containerFlow.Controls.Add(name);

                    container.Height += name.Height;

                    if (tag.Value != null && tag.Value.Trim() != "")
                    {
                        name.Text += " : " + tag.Value;
                    }

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
    }
}