using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatabaseManagement.UI
{
    public partial class DataTableViewer : Form
    {
        private Dictionary<string, bool> _columnVisibilityState = new Dictionary<string, bool>();

        public DataTableViewer(DataTable dataSource, string label, string title)
        {
            InitializeComponent();

            lblInitial.Text = label;
            txtInitial.Text = title;
            dataGridView.DataSource = dataSource;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void ShowColumnSelector()
        {
            using (var form = new Form())
            {
                form.Text = "Manage Columns";
                form.Width = 500;
                form.Height = 450;
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var mainPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    Padding = new Padding(10),
                    AutoScroll = true,
                    WrapContents = false
                };

                // Button panel
                var buttonPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    Padding = new Padding(0, 0, 0, 10)
                };

                var selectAllBtn = new Button
                {
                    Text = "Select All",
                    AutoSize = true,
                    Padding = new Padding(10, 5, 10, 5),
                };

                var deselectAllBtn = new Button
                {
                    Text = "Deselect All",
                    AutoSize = true,
                    Padding = new Padding(10, 5, 10, 5)
                };

                buttonPanel.Controls.Add(selectAllBtn);
                buttonPanel.Controls.Add(deselectAllBtn);
                mainPanel.Controls.Add(buttonPanel);

                // Column checkboxes
                var columnPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Width = 310,
                    WrapContents = false
                };

                var checkBoxes = new List<CheckBox>();

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    var checkBox = new CheckBox
                    {
                        Text = column.HeaderText,
                        Checked = column.Visible,
                        AutoSize = true,
                        Margin = new Padding(5),
                        Tag = column
                    };

                    checkBox.CheckedChanged += (s, e) =>
                    {
                        var col = (DataGridViewColumn)((CheckBox)s!)!.Tag!;
                        UpdateColumnVisibility(col, ((CheckBox)s).Checked);
                    };

                    columnPanel.Controls.Add(checkBox);
                    checkBoxes.Add(checkBox);
                }

                selectAllBtn.Click += (s, e) =>
                {
                    SetAllColumnsVisibility(true);
                    foreach (var cb in checkBoxes)
                        cb.Checked = true;
                };

                deselectAllBtn.Click += (s, e) =>
                {
                    SetAllColumnsVisibility(false);
                    foreach (var cb in checkBoxes)
                        cb.Checked = false;
                };

                var lbl = new Label
                {
                    Text = string.Empty,
                    Visible = true,
                    AutoSize = true,
                    Margin = new Padding(5)
                };
                columnPanel.Controls.Add(lbl);

                mainPanel.Controls.Add(columnPanel);

                form.Controls.Add(mainPanel);
                form.ShowDialog(this);
            }
        }

        private void UpdateColumnVisibility(DataGridViewColumn column, bool isVisible)
        {
            column.Visible = isVisible;
            _columnVisibilityState[column.Name] = isVisible;
        }

        private void SetAllColumnsVisibility(bool isVisible)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                UpdateColumnVisibility(column, isVisible);
            }
        }

        private void btnManageColumns_Click(object sender, EventArgs e)
        {
            ShowColumnSelector();
        }
    }
}
