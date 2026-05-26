namespace DatabaseManagement.UI
{
    partial class PasswordPromptForm
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
            btnOk = new Button();
            btnCancel = new Button();
            lblPassword = new Label();
            txtPassword = new TextBox();
            cbxShowPassword = new CheckBox();
            lblServer = new Label();
            lblUsername = new Label();
            txtServerName = new TextBox();
            txtUsername = new TextBox();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.DimGray;
            btnOk.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnOk.ForeColor = SystemColors.ButtonFace;
            btnOk.Location = new Point(102, 268);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(204, 72);
            btnOk.TabIndex = 0;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Firebrick;
            btnCancel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnCancel.ForeColor = SystemColors.ButtonFace;
            btnCancel.Location = new Point(365, 268);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(204, 72);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(102, 122);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(161, 28);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Enter Password :";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(102, 165);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(467, 34);
            txtPassword.TabIndex = 3;
            // 
            // cbxShowPassword
            // 
            cbxShowPassword.AutoSize = true;
            cbxShowPassword.Location = new Point(102, 205);
            cbxShowPassword.Name = "cbxShowPassword";
            cbxShowPassword.Size = new Size(88, 32);
            cbxShowPassword.TabIndex = 4;
            cbxShowPassword.Text = "Show";
            cbxShowPassword.UseVisualStyleBackColor = true;
            cbxShowPassword.CheckedChanged += cbxShowPassword_CheckedChanged;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(102, 24);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(81, 28);
            lblServer.TabIndex = 5;
            lblServer.Text = "Server :";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(102, 65);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(115, 28);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "Username :";
            // 
            // txtServerName
            // 
            txtServerName.Location = new Point(189, 25);
            txtServerName.Name = "txtServerName";
            txtServerName.ReadOnly = true;
            txtServerName.Size = new Size(344, 34);
            txtServerName.TabIndex = 7;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(219, 65);
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = true;
            txtUsername.Size = new Size(350, 34);
            txtUsername.TabIndex = 8;
            // 
            // PasswordPromptForm
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(685, 372);
            Controls.Add(txtUsername);
            Controls.Add(txtServerName);
            Controls.Add(lblUsername);
            Controls.Add(lblServer);
            Controls.Add(cbxShowPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PasswordPromptForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PasswordPromptForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox cbxShowPassword;
        private Label lblServer;
        private Label lblUsername;
        private TextBox txtServerName;
        private TextBox txtUsername;
    }
}