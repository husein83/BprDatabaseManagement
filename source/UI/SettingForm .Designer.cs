namespace DatabaseManagement.UI
{
    partial class SettingForm
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
            gbxServerList = new GroupBox();
            btnSetDefault = new Button();
            btnRemoveServer = new Button();
            btnAddServer = new Button();
            lstServers = new ListBox();
            gbxServerDetails = new GroupBox();
            cbxAutoCreateStoredProcedures = new CheckBox();
            numCommandTimeout = new NumericUpDown();
            lblCommandTimeout = new Label();
            cbxSavePassword = new CheckBox();
            btnTestConnection = new Button();
            lblConnectionTimeout = new Label();
            numConnectionTimeout = new NumericUpDown();
            cbxShowPassword = new CheckBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            rbSqlAuth = new RadioButton();
            rbWindowsAuth = new RadioButton();
            lblWindowsAuth = new Label();
            lblServerAlias = new Label();
            lblServerName = new Label();
            txtServerAlias = new TextBox();
            txtServerName = new TextBox();
            panelFooter = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            gbxServerList.SuspendLayout();
            gbxServerDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCommandTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numConnectionTimeout).BeginInit();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // gbxServerList
            // 
            gbxServerList.Controls.Add(btnSetDefault);
            gbxServerList.Controls.Add(btnRemoveServer);
            gbxServerList.Controls.Add(btnAddServer);
            gbxServerList.Controls.Add(lstServers);
            gbxServerList.Location = new Point(23, 27);
            gbxServerList.Name = "gbxServerList";
            gbxServerList.Size = new Size(429, 538);
            gbxServerList.TabIndex = 0;
            gbxServerList.TabStop = false;
            gbxServerList.Text = "Server List";
            // 
            // btnSetDefault
            // 
            btnSetDefault.BackColor = Color.CadetBlue;
            btnSetDefault.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSetDefault.ForeColor = SystemColors.WindowText;
            btnSetDefault.Location = new Point(90, 475);
            btnSetDefault.Name = "btnSetDefault";
            btnSetDefault.Size = new Size(242, 57);
            btnSetDefault.TabIndex = 3;
            btnSetDefault.Text = "Set as Default";
            btnSetDefault.UseVisualStyleBackColor = false;
            btnSetDefault.Click += btnSetDefault_Click;
            // 
            // btnRemoveServer
            // 
            btnRemoveServer.BackColor = Color.Maroon;
            btnRemoveServer.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnRemoveServer.ForeColor = SystemColors.Control;
            btnRemoveServer.Location = new Point(90, 425);
            btnRemoveServer.Name = "btnRemoveServer";
            btnRemoveServer.Size = new Size(242, 44);
            btnRemoveServer.TabIndex = 2;
            btnRemoveServer.Text = "Remove Server";
            btnRemoveServer.UseVisualStyleBackColor = false;
            btnRemoveServer.Click += btnRemoveServer_Click;
            // 
            // btnAddServer
            // 
            btnAddServer.BackColor = SystemColors.HighlightText;
            btnAddServer.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnAddServer.ForeColor = SystemColors.InfoText;
            btnAddServer.Location = new Point(90, 377);
            btnAddServer.Name = "btnAddServer";
            btnAddServer.Size = new Size(242, 42);
            btnAddServer.TabIndex = 1;
            btnAddServer.Text = "Add Server";
            btnAddServer.UseVisualStyleBackColor = false;
            btnAddServer.Click += btnAddServer_Click;
            // 
            // lstServers
            // 
            lstServers.FormattingEnabled = true;
            lstServers.Location = new Point(24, 50);
            lstServers.Name = "lstServers";
            lstServers.Size = new Size(386, 312);
            lstServers.TabIndex = 0;
            lstServers.SelectedIndexChanged += lstServers_SelectedIndexChanged;
            // 
            // gbxServerDetails
            // 
            gbxServerDetails.Controls.Add(cbxAutoCreateStoredProcedures);
            gbxServerDetails.Controls.Add(numCommandTimeout);
            gbxServerDetails.Controls.Add(lblCommandTimeout);
            gbxServerDetails.Controls.Add(cbxSavePassword);
            gbxServerDetails.Controls.Add(btnTestConnection);
            gbxServerDetails.Controls.Add(lblConnectionTimeout);
            gbxServerDetails.Controls.Add(numConnectionTimeout);
            gbxServerDetails.Controls.Add(cbxShowPassword);
            gbxServerDetails.Controls.Add(txtPassword);
            gbxServerDetails.Controls.Add(lblPassword);
            gbxServerDetails.Controls.Add(txtUsername);
            gbxServerDetails.Controls.Add(lblUsername);
            gbxServerDetails.Controls.Add(rbSqlAuth);
            gbxServerDetails.Controls.Add(rbWindowsAuth);
            gbxServerDetails.Controls.Add(lblWindowsAuth);
            gbxServerDetails.Controls.Add(lblServerAlias);
            gbxServerDetails.Controls.Add(lblServerName);
            gbxServerDetails.Controls.Add(txtServerAlias);
            gbxServerDetails.Controls.Add(txtServerName);
            gbxServerDetails.Location = new Point(469, 27);
            gbxServerDetails.Name = "gbxServerDetails";
            gbxServerDetails.Size = new Size(852, 538);
            gbxServerDetails.TabIndex = 1;
            gbxServerDetails.TabStop = false;
            gbxServerDetails.Text = "Server Details";
            // 
            // cbxAutoCreateStoredProcedures
            // 
            cbxAutoCreateStoredProcedures.AutoSize = true;
            cbxAutoCreateStoredProcedures.Location = new Point(33, 423);
            cbxAutoCreateStoredProcedures.Name = "cbxAutoCreateStoredProcedures";
            cbxAutoCreateStoredProcedures.Size = new Size(319, 32);
            cbxAutoCreateStoredProcedures.TabIndex = 21;
            cbxAutoCreateStoredProcedures.Text = "Auto Create Stored Procedures";
            cbxAutoCreateStoredProcedures.UseVisualStyleBackColor = true;
            // 
            // numCommandTimeout
            // 
            numCommandTimeout.Location = new Point(244, 371);
            numCommandTimeout.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numCommandTimeout.Name = "numCommandTimeout";
            numCommandTimeout.Size = new Size(121, 34);
            numCommandTimeout.TabIndex = 20;
            // 
            // lblCommandTimeout
            // 
            lblCommandTimeout.AutoSize = true;
            lblCommandTimeout.Location = new Point(33, 372);
            lblCommandTimeout.Name = "lblCommandTimeout";
            lblCommandTimeout.Size = new Size(199, 28);
            lblCommandTimeout.TabIndex = 19;
            lblCommandTimeout.Text = "Command Timeout :";
            // 
            // cbxSavePassword
            // 
            cbxSavePassword.AutoSize = true;
            cbxSavePassword.Location = new Point(137, 316);
            cbxSavePassword.Name = "cbxSavePassword";
            cbxSavePassword.Size = new Size(171, 32);
            cbxSavePassword.TabIndex = 15;
            cbxSavePassword.Text = "Save Password";
            cbxSavePassword.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            btnTestConnection.BackColor = Color.DimGray;
            btnTestConnection.ForeColor = SystemColors.ControlLightLight;
            btnTestConnection.Location = new Point(205, 465);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(418, 57);
            btnTestConnection.TabIndex = 14;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = false;
            btnTestConnection.Click += btnTestConnection_Click;
            // 
            // lblConnectionTimeout
            // 
            lblConnectionTimeout.AutoSize = true;
            lblConnectionTimeout.Location = new Point(411, 371);
            lblConnectionTimeout.Name = "lblConnectionTimeout";
            lblConnectionTimeout.Size = new Size(209, 28);
            lblConnectionTimeout.TabIndex = 13;
            lblConnectionTimeout.Text = "Connection Timeout :";
            // 
            // numConnectionTimeout
            // 
            numConnectionTimeout.Location = new Point(626, 375);
            numConnectionTimeout.Name = "numConnectionTimeout";
            numConnectionTimeout.Size = new Size(127, 34);
            numConnectionTimeout.TabIndex = 12;
            // 
            // cbxShowPassword
            // 
            cbxShowPassword.AutoSize = true;
            cbxShowPassword.Location = new Point(619, 280);
            cbxShowPassword.Name = "cbxShowPassword";
            cbxShowPassword.Size = new Size(88, 32);
            cbxShowPassword.TabIndex = 11;
            cbxShowPassword.Text = "Show";
            cbxShowPassword.UseVisualStyleBackColor = true;
            cbxShowPassword.CheckedChanged += cbxShowPassword_CheckedChanged;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(137, 278);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(461, 34);
            txtPassword.TabIndex = 10;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(23, 281);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(108, 28);
            lblPassword.TabIndex = 9;
            lblPassword.Text = "Password :";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(155, 233);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(465, 34);
            txtUsername.TabIndex = 8;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(23, 234);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(115, 28);
            lblUsername.TabIndex = 7;
            lblUsername.Text = "Username :";
            // 
            // rbSqlAuth
            // 
            rbSqlAuth.AutoSize = true;
            rbSqlAuth.Location = new Point(514, 173);
            rbSqlAuth.Name = "rbSqlAuth";
            rbSqlAuth.Size = new Size(179, 32);
            rbSqlAuth.TabIndex = 6;
            rbSqlAuth.TabStop = true;
            rbSqlAuth.Text = "Sql Server Auth";
            rbSqlAuth.UseVisualStyleBackColor = true;
            rbSqlAuth.CheckedChanged += rbAuth_CheckedChanged;
            // 
            // rbWindowsAuth
            // 
            rbWindowsAuth.AutoSize = true;
            rbWindowsAuth.Location = new Point(306, 173);
            rbWindowsAuth.Name = "rbWindowsAuth";
            rbWindowsAuth.Size = new Size(171, 32);
            rbWindowsAuth.TabIndex = 5;
            rbWindowsAuth.TabStop = true;
            rbWindowsAuth.Text = "Windows Auth";
            rbWindowsAuth.UseVisualStyleBackColor = true;
            rbWindowsAuth.CheckedChanged += rbAuth_CheckedChanged;
            // 
            // lblWindowsAuth
            // 
            lblWindowsAuth.AutoSize = true;
            lblWindowsAuth.Location = new Point(23, 173);
            lblWindowsAuth.Name = "lblWindowsAuth";
            lblWindowsAuth.Size = new Size(221, 28);
            lblWindowsAuth.TabIndex = 4;
            lblWindowsAuth.Text = "Server Authentication :";
            // 
            // lblServerAlias
            // 
            lblServerAlias.AutoSize = true;
            lblServerAlias.Location = new Point(23, 110);
            lblServerAlias.Name = "lblServerAlias";
            lblServerAlias.Size = new Size(129, 28);
            lblServerAlias.TabIndex = 3;
            lblServerAlias.Text = "Server Alias :";
            // 
            // lblServerName
            // 
            lblServerName.AutoSize = true;
            lblServerName.Location = new Point(23, 59);
            lblServerName.Name = "lblServerName";
            lblServerName.Size = new Size(141, 28);
            lblServerName.TabIndex = 2;
            lblServerName.Text = "Server Name :";
            // 
            // txtServerAlias
            // 
            txtServerAlias.Location = new Point(170, 110);
            txtServerAlias.Name = "txtServerAlias";
            txtServerAlias.Size = new Size(440, 34);
            txtServerAlias.TabIndex = 1;
            txtServerAlias.Leave += txtServerAlias_Leave;
            // 
            // txtServerName
            // 
            txtServerName.Location = new Point(170, 59);
            txtServerName.Name = "txtServerName";
            txtServerName.Size = new Size(662, 34);
            txtServerName.TabIndex = 0;
            txtServerName.Leave += txtServerName_Leave;
            // 
            // panelFooter
            // 
            panelFooter.Controls.Add(btnCancel);
            panelFooter.Controls.Add(btnSave);
            panelFooter.Location = new Point(23, 571);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(1298, 87);
            panelFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Firebrick;
            btnCancel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnCancel.ForeColor = SystemColors.ButtonFace;
            btnCancel.Location = new Point(766, 13);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(439, 61);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.DarkSlateGray;
            btnSave.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnSave.ForeColor = Color.LightGreen;
            btnSave.Location = new Point(103, 13);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(639, 61);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save Changes";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1344, 672);
            Controls.Add(panelFooter);
            Controls.Add(gbxServerDetails);
            Controls.Add(gbxServerList);
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "SettingForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SettingForm";
            Load += SettingForm_Load;
            gbxServerList.ResumeLayout(false);
            gbxServerDetails.ResumeLayout(false);
            gbxServerDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCommandTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numConnectionTimeout).EndInit();
            panelFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbxServerList;
        private GroupBox gbxServerDetails;
        private Panel panelFooter;
        private Button btnSetDefault;
        private Button btnRemoveServer;
        private Button btnAddServer;
        private ListBox lstServers;
        private Button btnTestConnection;
        private Label lblConnectionTimeout;
        private NumericUpDown numConnectionTimeout;
        private CheckBox cbxShowPassword;
        private TextBox txtPassword;
        private Label lblPassword;
        private TextBox txtUsername;
        private Label lblUsername;
        private RadioButton rbSqlAuth;
        private RadioButton rbWindowsAuth;
        private Label lblWindowsAuth;
        private Label lblServerAlias;
        private Label lblServerName;
        private TextBox txtServerAlias;
        private TextBox txtServerName;
        private CheckBox cbxSavePassword;
        private Button btnCancel;
        private Button btnSave;
        private NumericUpDown numCommandTimeout;
        private Label lblCommandTimeout;
        private CheckBox cbxAutoCreateStoredProcedures;
    }
}