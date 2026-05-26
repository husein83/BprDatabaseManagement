namespace DatabaseManagement.UI
{
    partial class DataTableViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            lblInitial = new Label();
            txtInitial = new TextBox();
            btnManageColumns = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(19, 71);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 62;
            dataGridView.Size = new Size(1771, 445);
            dataGridView.TabIndex = 0;
            // 
            // lblInitial
            // 
            lblInitial.AutoSize = true;
            lblInitial.Location = new Point(98, 25);
            lblInitial.Name = "lblInitial";
            lblInitial.Size = new Size(110, 28);
            lblInitial.TabIndex = 1;
            lblInitial.Text = "InitialLabel";
            // 
            // txtInitial
            // 
            txtInitial.Location = new Point(269, 24);
            txtInitial.Name = "txtInitial";
            txtInitial.ReadOnly = true;
            txtInitial.Size = new Size(942, 34);
            txtInitial.TabIndex = 2;
            // 
            // btnManageColumns
            // 
            btnManageColumns.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnManageColumns.Location = new Point(1244, 24);
            btnManageColumns.Name = "btnManageColumns";
            btnManageColumns.Size = new Size(283, 34);
            btnManageColumns.TabIndex = 3;
            btnManageColumns.Text = "Manage Columns";
            btnManageColumns.UseVisualStyleBackColor = true;
            btnManageColumns.Click += btnManageColumns_Click;
            // 
            // DataTableViewer
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1808, 529);
            Controls.Add(btnManageColumns);
            Controls.Add(txtInitial);
            Controls.Add(lblInitial);
            Controls.Add(dataGridView);
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "DataTableViewer";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Information";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private Label lblInitial;
        private TextBox txtInitial;
        private Button btnManageColumns;
    }
}