namespace DatabaseManagement.UI
{
    partial class MainForm
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
            panelHeader = new Panel();
            btnShowInformation = new Button();
            btnSetting = new Button();
            btnRefresh = new Button();
            cmbServers = new ComboBox();
            lblSelectServer = new Label();
            tabControl = new TabControl();
            tabDatabase = new TabPage();
            gbx2DatabaseDetails = new GroupBox();
            cbxForceDisconnect = new CheckBox();
            lblDatabases = new Label();
            cmbDatabases = new ComboBox();
            btnBrowseDataFilePath = new Button();
            txtDataFilePath = new TextBox();
            cmbCollations = new ComboBox();
            lblDataFilePath = new Label();
            lblCollation = new Label();
            numDbFileGrowth = new NumericUpDown();
            lblFileGrowth = new Label();
            numDbInitialSize = new NumericUpDown();
            lblInitialSize = new Label();
            txtDbName = new TextBox();
            lblDbName = new Label();
            tabTable = new TabPage();
            gbx3TableDetails = new GroupBox();
            cbxForceDropDependent = new CheckBox();
            cmbTables = new ComboBox();
            cmbSchemas = new ComboBox();
            txtDescription = new TextBox();
            lblDescription = new Label();
            cmbFileGroups = new ComboBox();
            lblFileGroup = new Label();
            txtTableName = new TextBox();
            lblTableName = new Label();
            txtSchemaName = new TextBox();
            lblSchemaName = new Label();
            cmbDatabases2 = new ComboBox();
            lblDatabases2 = new Label();
            tabColumn = new TabPage();
            gbx4ColumnDetails = new GroupBox();
            cbxForceDropDependent2 = new CheckBox();
            txtComputedFormula = new TextBox();
            lblComputedFormula = new Label();
            lblDescription2 = new Label();
            cmbCollations2 = new ComboBox();
            txtDescription2 = new TextBox();
            lblCollation2 = new Label();
            cbxIsPersisted = new CheckBox();
            numIdentityIncrement = new NumericUpDown();
            lblIdentityIncrement = new Label();
            txtDefaultValue = new TextBox();
            txtPkName = new TextBox();
            cmbColumns = new ComboBox();
            numIdentitySeed = new NumericUpDown();
            lblIdentitySeed = new Label();
            cbxIsPrimaryKey = new CheckBox();
            lblDefaultValue = new Label();
            cbxIsIdentity = new CheckBox();
            lblPkName = new Label();
            cbxIsNullable = new CheckBox();
            numPrecision = new NumericUpDown();
            lblPrecision = new Label();
            numScale = new NumericUpDown();
            lblScale = new Label();
            numLength = new NumericUpDown();
            lblLength = new Label();
            cmbDataTypes = new ComboBox();
            txtColumnName = new TextBox();
            lblDataType = new Label();
            lblColumnName = new Label();
            cmbTables2 = new ComboBox();
            lblTables = new Label();
            cmbSchemas2 = new ComboBox();
            lblSchemas = new Label();
            cmbDatabases3 = new ComboBox();
            lblDatabases3 = new Label();
            tabIndex = new TabPage();
            gbx5IndexDetails = new GroupBox();
            lblIndexes = new Label();
            cmbIndexes = new ComboBox();
            cbxIgnoreIfNotExists = new CheckBox();
            txtFilterPredicate = new TextBox();
            lblFilterPredicate = new Label();
            cbxDropExisting = new CheckBox();
            cmbFileGroups2 = new ComboBox();
            lblFileGroup2 = new Label();
            cbxAllowPageLocks = new CheckBox();
            cbxAllowRowLocks = new CheckBox();
            cbxPadIndex = new CheckBox();
            numFillFactor = new NumericUpDown();
            lblFillFactor = new Label();
            cmbIndexType = new ComboBox();
            lblIndexType = new Label();
            cbxIsUnique = new CheckBox();
            txtIndexName = new TextBox();
            lblIndexName = new Label();
            txtIncludeColumns = new TextBox();
            lblIncludeColumns = new Label();
            txtColumns = new TextBox();
            lblColumns = new Label();
            cmbTables3 = new ComboBox();
            lblDatabases4 = new Label();
            lblTables2 = new Label();
            cmbDatabases4 = new ComboBox();
            cmbSchemas3 = new ComboBox();
            lblSchemas2 = new Label();
            tabForeignKey = new TabPage();
            gbx6ForeignKeyDetails = new GroupBox();
            lblFkName = new Label();
            txtFkName = new TextBox();
            cbxIsNotForReplication = new CheckBox();
            cbxEnabled = new CheckBox();
            cmbOnUpdate = new ComboBox();
            lblOnUpdate = new Label();
            cmbOnDelete = new ComboBox();
            lblOnDelete = new Label();
            cmbColumns3 = new ComboBox();
            lblColumns3 = new Label();
            cmbTables5 = new ComboBox();
            lblTables4 = new Label();
            cmbSchemas5 = new ComboBox();
            lblSchemas4 = new Label();
            cmbColumns2 = new ComboBox();
            lblColumns2 = new Label();
            cmbSchemas4 = new ComboBox();
            lblSchemas3 = new Label();
            cmbTables4 = new ComboBox();
            lblTables3 = new Label();
            cmbDatabases5 = new ComboBox();
            lblDatabases5 = new Label();
            tabBackupAndRestore = new TabPage();
            gbx8Restore = new GroupBox();
            btnBrowseRestorePath = new Button();
            txtRestorePath = new TextBox();
            lblRestorePath = new Label();
            cmbDatabases7 = new ComboBox();
            lblDatabases7 = new Label();
            gbx7Backup = new GroupBox();
            btnBrowseBackupPath = new Button();
            txtBackupPath = new TextBox();
            lblBackupPath = new Label();
            cmbDatabases6 = new ComboBox();
            lblDatabases6 = new Label();
            btnExecuteDb = new Button();
            gbx1OperationType = new GroupBox();
            rbDrop = new RadioButton();
            rbAlter = new RadioButton();
            rbCreate = new RadioButton();
            gbx9OperationType = new GroupBox();
            rbRestore = new RadioButton();
            rbBackup = new RadioButton();
            panelFooter = new Panel();
            btnClearLog = new Button();
            rtbLog = new RichTextBox();
            lblOperationLog = new Label();
            panelOperation = new Panel();
            panelHeader.SuspendLayout();
            tabControl.SuspendLayout();
            tabDatabase.SuspendLayout();
            gbx2DatabaseDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDbFileGrowth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDbInitialSize).BeginInit();
            tabTable.SuspendLayout();
            gbx3TableDetails.SuspendLayout();
            tabColumn.SuspendLayout();
            gbx4ColumnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIdentityIncrement).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numIdentitySeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrecision).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLength).BeginInit();
            tabIndex.SuspendLayout();
            gbx5IndexDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFillFactor).BeginInit();
            tabForeignKey.SuspendLayout();
            gbx6ForeignKeyDetails.SuspendLayout();
            tabBackupAndRestore.SuspendLayout();
            gbx8Restore.SuspendLayout();
            gbx7Backup.SuspendLayout();
            gbx1OperationType.SuspendLayout();
            gbx9OperationType.SuspendLayout();
            panelFooter.SuspendLayout();
            panelOperation.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelHeader.Controls.Add(btnShowInformation);
            panelHeader.Controls.Add(btnSetting);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(cmbServers);
            panelHeader.Controls.Add(lblSelectServer);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1318, 95);
            panelHeader.TabIndex = 0;
            // 
            // btnShowInformation
            // 
            btnShowInformation.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnShowInformation.Location = new Point(932, 22);
            btnShowInformation.Name = "btnShowInformation";
            btnShowInformation.Size = new Size(350, 48);
            btnShowInformation.TabIndex = 4;
            btnShowInformation.Text = "Show Information";
            btnShowInformation.UseVisualStyleBackColor = true;
            btnShowInformation.Visible = false;
            btnShowInformation.Click += btnShowInformation_Click;
            // 
            // btnSetting
            // 
            btnSetting.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetting.Location = new Point(762, 22);
            btnSetting.Name = "btnSetting";
            btnSetting.Size = new Size(134, 48);
            btnSetting.TabIndex = 3;
            btnSetting.Text = "Setting";
            btnSetting.UseVisualStyleBackColor = true;
            btnSetting.Click += btnSetting_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.Location = new Point(600, 22);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(134, 48);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // cmbServers
            // 
            cmbServers.FormattingEnabled = true;
            cmbServers.Location = new Point(176, 28);
            cmbServers.Name = "cmbServers";
            cmbServers.Size = new Size(396, 33);
            cmbServers.TabIndex = 1;
            cmbServers.SelectedIndexChanged += cmbServers_SelectedIndexChanged;
            // 
            // lblSelectServer
            // 
            lblSelectServer.AutoSize = true;
            lblSelectServer.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSelectServer.Location = new Point(23, 30);
            lblSelectServer.Name = "lblSelectServer";
            lblSelectServer.Size = new Size(141, 28);
            lblSelectServer.TabIndex = 0;
            lblSelectServer.Text = "Select Server :";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabDatabase);
            tabControl.Controls.Add(tabTable);
            tabControl.Controls.Add(tabColumn);
            tabControl.Controls.Add(tabIndex);
            tabControl.Controls.Add(tabForeignKey);
            tabControl.Controls.Add(tabBackupAndRestore);
            tabControl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabControl.Location = new Point(242, 95);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1076, 532);
            tabControl.TabIndex = 1;
            tabControl.SelectedIndexChanged += btnTabChanged_Click;
            // 
            // tabDatabase
            // 
            tabDatabase.Controls.Add(gbx2DatabaseDetails);
            tabDatabase.Location = new Point(4, 34);
            tabDatabase.Name = "tabDatabase";
            tabDatabase.Padding = new Padding(3);
            tabDatabase.Size = new Size(1068, 494);
            tabDatabase.TabIndex = 0;
            tabDatabase.Text = "Database";
            tabDatabase.UseVisualStyleBackColor = true;
            // 
            // gbx2DatabaseDetails
            // 
            gbx2DatabaseDetails.Controls.Add(cbxForceDisconnect);
            gbx2DatabaseDetails.Controls.Add(lblDatabases);
            gbx2DatabaseDetails.Controls.Add(cmbDatabases);
            gbx2DatabaseDetails.Controls.Add(btnBrowseDataFilePath);
            gbx2DatabaseDetails.Controls.Add(txtDataFilePath);
            gbx2DatabaseDetails.Controls.Add(cmbCollations);
            gbx2DatabaseDetails.Controls.Add(lblDataFilePath);
            gbx2DatabaseDetails.Controls.Add(lblCollation);
            gbx2DatabaseDetails.Controls.Add(numDbFileGrowth);
            gbx2DatabaseDetails.Controls.Add(lblFileGrowth);
            gbx2DatabaseDetails.Controls.Add(numDbInitialSize);
            gbx2DatabaseDetails.Controls.Add(lblInitialSize);
            gbx2DatabaseDetails.Controls.Add(txtDbName);
            gbx2DatabaseDetails.Controls.Add(lblDbName);
            gbx2DatabaseDetails.Location = new Point(14, 23);
            gbx2DatabaseDetails.Name = "gbx2DatabaseDetails";
            gbx2DatabaseDetails.Size = new Size(1038, 455);
            gbx2DatabaseDetails.TabIndex = 2;
            gbx2DatabaseDetails.TabStop = false;
            gbx2DatabaseDetails.Text = "Database Details";
            // 
            // cbxForceDisconnect
            // 
            cbxForceDisconnect.AutoSize = true;
            cbxForceDisconnect.Location = new Point(604, 81);
            cbxForceDisconnect.Name = "cbxForceDisconnect";
            cbxForceDisconnect.Size = new Size(180, 29);
            cbxForceDisconnect.TabIndex = 15;
            cbxForceDisconnect.Text = "Force Disconnect";
            cbxForceDisconnect.UseVisualStyleBackColor = true;
            cbxForceDisconnect.Visible = false;
            // 
            // lblDatabases
            // 
            lblDatabases.AutoSize = true;
            lblDatabases.Location = new Point(44, 81);
            lblDatabases.Name = "lblDatabases";
            lblDatabases.Size = new Size(97, 25);
            lblDatabases.TabIndex = 14;
            lblDatabases.Text = "Database :";
            // 
            // cmbDatabases
            // 
            cmbDatabases.FormattingEnabled = true;
            cmbDatabases.Location = new Point(201, 78);
            cmbDatabases.Name = "cmbDatabases";
            cmbDatabases.Size = new Size(311, 33);
            cmbDatabases.TabIndex = 13;
            cmbDatabases.SelectedIndexChanged += cmbDatabases_SelectedIndexChanged;
            // 
            // btnBrowseDataFilePath
            // 
            btnBrowseDataFilePath.Location = new Point(892, 386);
            btnBrowseDataFilePath.Name = "btnBrowseDataFilePath";
            btnBrowseDataFilePath.Size = new Size(84, 34);
            btnBrowseDataFilePath.TabIndex = 12;
            btnBrowseDataFilePath.Text = "Browse";
            btnBrowseDataFilePath.UseVisualStyleBackColor = true;
            btnBrowseDataFilePath.Click += btnBrowseDataFilePath_Click;
            // 
            // txtDataFilePath
            // 
            txtDataFilePath.Location = new Point(179, 388);
            txtDataFilePath.Name = "txtDataFilePath";
            txtDataFilePath.ReadOnly = true;
            txtDataFilePath.Size = new Size(707, 31);
            txtDataFilePath.TabIndex = 11;
            // 
            // cmbCollations
            // 
            cmbCollations.FormattingEnabled = true;
            cmbCollations.Location = new Point(167, 315);
            cmbCollations.Name = "cmbCollations";
            cmbCollations.Size = new Size(345, 33);
            cmbCollations.TabIndex = 10;
            // 
            // lblDataFilePath
            // 
            lblDataFilePath.AutoSize = true;
            lblDataFilePath.Location = new Point(44, 391);
            lblDataFilePath.Name = "lblDataFilePath";
            lblDataFilePath.Size = new Size(129, 25);
            lblDataFilePath.TabIndex = 8;
            lblDataFilePath.Text = "DataFile Path :";
            // 
            // lblCollation
            // 
            lblCollation.AutoSize = true;
            lblCollation.Location = new Point(44, 318);
            lblCollation.Name = "lblCollation";
            lblCollation.Size = new Size(96, 25);
            lblCollation.TabIndex = 6;
            lblCollation.Text = "Collation :";
            // 
            // numDbFileGrowth
            // 
            numDbFileGrowth.Location = new Point(694, 232);
            numDbFileGrowth.Name = "numDbFileGrowth";
            numDbFileGrowth.Size = new Size(180, 31);
            numDbFileGrowth.TabIndex = 5;
            // 
            // lblFileGrowth
            // 
            lblFileGrowth.AutoSize = true;
            lblFileGrowth.Location = new Point(515, 234);
            lblFileGrowth.Name = "lblFileGrowth";
            lblFileGrowth.Size = new Size(157, 25);
            lblFileGrowth.TabIndex = 4;
            lblFileGrowth.Text = "FileGrowth (MB) :";
            // 
            // numDbInitialSize
            // 
            numDbInitialSize.Location = new Point(223, 232);
            numDbInitialSize.Name = "numDbInitialSize";
            numDbInitialSize.Size = new Size(180, 31);
            numDbInitialSize.TabIndex = 3;
            // 
            // lblInitialSize
            // 
            lblInitialSize.AutoSize = true;
            lblInitialSize.Location = new Point(44, 234);
            lblInitialSize.Name = "lblInitialSize";
            lblInitialSize.Size = new Size(151, 25);
            lblInitialSize.TabIndex = 2;
            lblInitialSize.Text = "Initial Size (MB) :";
            // 
            // txtDbName
            // 
            txtDbName.Location = new Point(201, 152);
            txtDbName.Name = "txtDbName";
            txtDbName.Size = new Size(350, 31);
            txtDbName.TabIndex = 1;
            // 
            // lblDbName
            // 
            lblDbName.AutoSize = true;
            lblDbName.Location = new Point(44, 152);
            lblDbName.Name = "lblDbName";
            lblDbName.Size = new Size(151, 25);
            lblDbName.TabIndex = 0;
            lblDbName.Text = "Database Name :";
            // 
            // tabTable
            // 
            tabTable.Controls.Add(gbx3TableDetails);
            tabTable.Location = new Point(4, 34);
            tabTable.Name = "tabTable";
            tabTable.Padding = new Padding(3);
            tabTable.Size = new Size(1068, 494);
            tabTable.TabIndex = 1;
            tabTable.Text = "Table";
            tabTable.UseVisualStyleBackColor = true;
            // 
            // gbx3TableDetails
            // 
            gbx3TableDetails.Controls.Add(cbxForceDropDependent);
            gbx3TableDetails.Controls.Add(cmbTables);
            gbx3TableDetails.Controls.Add(cmbSchemas);
            gbx3TableDetails.Controls.Add(txtDescription);
            gbx3TableDetails.Controls.Add(lblDescription);
            gbx3TableDetails.Controls.Add(cmbFileGroups);
            gbx3TableDetails.Controls.Add(lblFileGroup);
            gbx3TableDetails.Controls.Add(txtTableName);
            gbx3TableDetails.Controls.Add(lblTableName);
            gbx3TableDetails.Controls.Add(txtSchemaName);
            gbx3TableDetails.Controls.Add(lblSchemaName);
            gbx3TableDetails.Controls.Add(cmbDatabases2);
            gbx3TableDetails.Controls.Add(lblDatabases2);
            gbx3TableDetails.Location = new Point(13, 16);
            gbx3TableDetails.Name = "gbx3TableDetails";
            gbx3TableDetails.Size = new Size(1041, 467);
            gbx3TableDetails.TabIndex = 0;
            gbx3TableDetails.TabStop = false;
            gbx3TableDetails.Text = "Table Details";
            // 
            // cbxForceDropDependent
            // 
            cbxForceDropDependent.AutoSize = true;
            cbxForceDropDependent.Location = new Point(629, 199);
            cbxForceDropDependent.Name = "cbxForceDropDependent";
            cbxForceDropDependent.Size = new Size(229, 29);
            cbxForceDropDependent.TabIndex = 12;
            cbxForceDropDependent.Text = "Force Drop Dependent";
            cbxForceDropDependent.UseVisualStyleBackColor = true;
            // 
            // cmbTables
            // 
            cmbTables.FormattingEnabled = true;
            cmbTables.Location = new Point(184, 197);
            cmbTables.Name = "cmbTables";
            cmbTables.Size = new Size(362, 33);
            cmbTables.TabIndex = 11;
            cmbTables.SelectedIndexChanged += cmbTables_SelectedIndexChanged;
            // 
            // cmbSchemas
            // 
            cmbSchemas.FormattingEnabled = true;
            cmbSchemas.Location = new Point(200, 127);
            cmbSchemas.Name = "cmbSchemas";
            cmbSchemas.Size = new Size(217, 33);
            cmbSchemas.TabIndex = 10;
            cmbSchemas.SelectedIndexChanged += cmbSchemas_SelectedIndexChanged;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(184, 316);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(754, 111);
            txtDescription.TabIndex = 9;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(46, 316);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(117, 25);
            lblDescription.TabIndex = 8;
            lblDescription.Text = "Description :";
            // 
            // cmbFileGroups
            // 
            cmbFileGroups.FormattingEnabled = true;
            cmbFileGroups.Location = new Point(184, 256);
            cmbFileGroups.Name = "cmbFileGroups";
            cmbFileGroups.Size = new Size(310, 33);
            cmbFileGroups.TabIndex = 7;
            // 
            // lblFileGroup
            // 
            lblFileGroup.AutoSize = true;
            lblFileGroup.Location = new Point(46, 259);
            lblFileGroup.Name = "lblFileGroup";
            lblFileGroup.Size = new Size(108, 25);
            lblFileGroup.TabIndex = 6;
            lblFileGroup.Text = "File Group :";
            // 
            // txtTableName
            // 
            txtTableName.Location = new Point(184, 197);
            txtTableName.Name = "txtTableName";
            txtTableName.Size = new Size(362, 31);
            txtTableName.TabIndex = 5;
            // 
            // lblTableName
            // 
            lblTableName.AutoSize = true;
            lblTableName.Location = new Point(46, 197);
            lblTableName.Name = "lblTableName";
            lblTableName.Size = new Size(118, 25);
            lblTableName.TabIndex = 4;
            lblTableName.Text = "Table Name :";
            // 
            // txtSchemaName
            // 
            txtSchemaName.Location = new Point(200, 129);
            txtSchemaName.Name = "txtSchemaName";
            txtSchemaName.Size = new Size(217, 31);
            txtSchemaName.TabIndex = 3;
            // 
            // lblSchemaName
            // 
            lblSchemaName.AutoSize = true;
            lblSchemaName.Location = new Point(46, 132);
            lblSchemaName.Name = "lblSchemaName";
            lblSchemaName.Size = new Size(138, 25);
            lblSchemaName.TabIndex = 2;
            lblSchemaName.Text = "Schema Name :";
            // 
            // cmbDatabases2
            // 
            cmbDatabases2.FormattingEnabled = true;
            cmbDatabases2.Location = new Point(152, 63);
            cmbDatabases2.Name = "cmbDatabases2";
            cmbDatabases2.Size = new Size(278, 33);
            cmbDatabases2.TabIndex = 1;
            cmbDatabases2.SelectedIndexChanged += cmbDatabases_SelectedIndexChanged;
            // 
            // lblDatabases2
            // 
            lblDatabases2.AutoSize = true;
            lblDatabases2.Location = new Point(46, 63);
            lblDatabases2.Name = "lblDatabases2";
            lblDatabases2.Size = new Size(97, 25);
            lblDatabases2.TabIndex = 0;
            lblDatabases2.Text = "Database :";
            // 
            // tabColumn
            // 
            tabColumn.Controls.Add(gbx4ColumnDetails);
            tabColumn.Location = new Point(4, 34);
            tabColumn.Name = "tabColumn";
            tabColumn.Padding = new Padding(3);
            tabColumn.Size = new Size(1068, 494);
            tabColumn.TabIndex = 2;
            tabColumn.Text = "Column";
            tabColumn.UseVisualStyleBackColor = true;
            // 
            // gbx4ColumnDetails
            // 
            gbx4ColumnDetails.Controls.Add(cbxForceDropDependent2);
            gbx4ColumnDetails.Controls.Add(txtComputedFormula);
            gbx4ColumnDetails.Controls.Add(lblComputedFormula);
            gbx4ColumnDetails.Controls.Add(lblDescription2);
            gbx4ColumnDetails.Controls.Add(cmbCollations2);
            gbx4ColumnDetails.Controls.Add(txtDescription2);
            gbx4ColumnDetails.Controls.Add(lblCollation2);
            gbx4ColumnDetails.Controls.Add(cbxIsPersisted);
            gbx4ColumnDetails.Controls.Add(numIdentityIncrement);
            gbx4ColumnDetails.Controls.Add(lblIdentityIncrement);
            gbx4ColumnDetails.Controls.Add(txtDefaultValue);
            gbx4ColumnDetails.Controls.Add(txtPkName);
            gbx4ColumnDetails.Controls.Add(cmbColumns);
            gbx4ColumnDetails.Controls.Add(numIdentitySeed);
            gbx4ColumnDetails.Controls.Add(lblIdentitySeed);
            gbx4ColumnDetails.Controls.Add(cbxIsPrimaryKey);
            gbx4ColumnDetails.Controls.Add(lblDefaultValue);
            gbx4ColumnDetails.Controls.Add(cbxIsIdentity);
            gbx4ColumnDetails.Controls.Add(lblPkName);
            gbx4ColumnDetails.Controls.Add(cbxIsNullable);
            gbx4ColumnDetails.Controls.Add(numPrecision);
            gbx4ColumnDetails.Controls.Add(lblPrecision);
            gbx4ColumnDetails.Controls.Add(numScale);
            gbx4ColumnDetails.Controls.Add(lblScale);
            gbx4ColumnDetails.Controls.Add(numLength);
            gbx4ColumnDetails.Controls.Add(lblLength);
            gbx4ColumnDetails.Controls.Add(cmbDataTypes);
            gbx4ColumnDetails.Controls.Add(txtColumnName);
            gbx4ColumnDetails.Controls.Add(lblDataType);
            gbx4ColumnDetails.Controls.Add(lblColumnName);
            gbx4ColumnDetails.Controls.Add(cmbTables2);
            gbx4ColumnDetails.Controls.Add(lblTables);
            gbx4ColumnDetails.Controls.Add(cmbSchemas2);
            gbx4ColumnDetails.Controls.Add(lblSchemas);
            gbx4ColumnDetails.Controls.Add(cmbDatabases3);
            gbx4ColumnDetails.Controls.Add(lblDatabases3);
            gbx4ColumnDetails.Location = new Point(14, 13);
            gbx4ColumnDetails.Name = "gbx4ColumnDetails";
            gbx4ColumnDetails.Size = new Size(1039, 470);
            gbx4ColumnDetails.TabIndex = 0;
            gbx4ColumnDetails.TabStop = false;
            gbx4ColumnDetails.Text = "Column Details";
            // 
            // cbxForceDropDependent2
            // 
            cbxForceDropDependent2.AutoSize = true;
            cbxForceDropDependent2.Location = new Point(22, 217);
            cbxForceDropDependent2.Name = "cbxForceDropDependent2";
            cbxForceDropDependent2.Size = new Size(229, 29);
            cbxForceDropDependent2.TabIndex = 38;
            cbxForceDropDependent2.Text = "Force Drop Dependent";
            cbxForceDropDependent2.UseVisualStyleBackColor = true;
            // 
            // txtComputedFormula
            // 
            txtComputedFormula.Location = new Point(598, 351);
            txtComputedFormula.Name = "txtComputedFormula";
            txtComputedFormula.Size = new Size(424, 31);
            txtComputedFormula.TabIndex = 37;
            // 
            // lblComputedFormula
            // 
            lblComputedFormula.AutoSize = true;
            lblComputedFormula.Location = new Point(410, 354);
            lblComputedFormula.Name = "lblComputedFormula";
            lblComputedFormula.Size = new Size(182, 25);
            lblComputedFormula.TabIndex = 36;
            lblComputedFormula.Text = "Computed Formula :";
            // 
            // lblDescription2
            // 
            lblDescription2.AutoSize = true;
            lblDescription2.Location = new Point(23, 400);
            lblDescription2.Name = "lblDescription2";
            lblDescription2.Size = new Size(117, 25);
            lblDescription2.TabIndex = 35;
            lblDescription2.Text = "Description :";
            // 
            // cmbCollations2
            // 
            cmbCollations2.FormattingEnabled = true;
            cmbCollations2.Location = new Point(125, 351);
            cmbCollations2.Name = "cmbCollations2";
            cmbCollations2.Size = new Size(270, 33);
            cmbCollations2.TabIndex = 34;
            // 
            // txtDescription2
            // 
            txtDescription2.Location = new Point(146, 397);
            txtDescription2.Multiline = true;
            txtDescription2.Name = "txtDescription2";
            txtDescription2.Size = new Size(876, 60);
            txtDescription2.TabIndex = 33;
            // 
            // lblCollation2
            // 
            lblCollation2.AutoSize = true;
            lblCollation2.Location = new Point(23, 354);
            lblCollation2.Name = "lblCollation2";
            lblCollation2.Size = new Size(96, 25);
            lblCollation2.TabIndex = 32;
            lblCollation2.Text = "Collation :";
            // 
            // cbxIsPersisted
            // 
            cbxIsPersisted.AutoSize = true;
            cbxIsPersisted.Location = new Point(168, 241);
            cbxIsPersisted.Name = "cbxIsPersisted";
            cbxIsPersisted.Size = new Size(127, 29);
            cbxIsPersisted.TabIndex = 31;
            cbxIsPersisted.Text = "IsPersisted";
            cbxIsPersisted.UseVisualStyleBackColor = true;
            // 
            // numIdentityIncrement
            // 
            numIdentityIncrement.Location = new Point(743, 241);
            numIdentityIncrement.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numIdentityIncrement.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numIdentityIncrement.Name = "numIdentityIncrement";
            numIdentityIncrement.Size = new Size(118, 31);
            numIdentityIncrement.TabIndex = 30;
            numIdentityIncrement.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblIdentityIncrement
            // 
            lblIdentityIncrement.AutoSize = true;
            lblIdentityIncrement.Location = new Point(625, 243);
            lblIdentityIncrement.Name = "lblIdentityIncrement";
            lblIdentityIncrement.Size = new Size(106, 25);
            lblIdentityIncrement.TabIndex = 29;
            lblIdentityIncrement.Text = "Increment :";
            // 
            // txtDefaultValue
            // 
            txtDefaultValue.Location = new Point(162, 294);
            txtDefaultValue.Name = "txtDefaultValue";
            txtDefaultValue.Size = new Size(233, 31);
            txtDefaultValue.TabIndex = 28;
            // 
            // txtPkName
            // 
            txtPkName.Location = new Point(649, 297);
            txtPkName.Name = "txtPkName";
            txtPkName.Size = new Size(373, 31);
            txtPkName.TabIndex = 27;
            // 
            // cmbColumns
            // 
            cmbColumns.FormattingEnabled = true;
            cmbColumns.Location = new Point(181, 113);
            cmbColumns.Name = "cmbColumns";
            cmbColumns.Size = new Size(322, 33);
            cmbColumns.TabIndex = 26;
            // 
            // numIdentitySeed
            // 
            numIdentitySeed.Location = new Point(498, 241);
            numIdentitySeed.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numIdentitySeed.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numIdentitySeed.Name = "numIdentitySeed";
            numIdentitySeed.Size = new Size(110, 31);
            numIdentitySeed.TabIndex = 25;
            numIdentitySeed.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblIdentitySeed
            // 
            lblIdentitySeed.AutoSize = true;
            lblIdentitySeed.Location = new Point(439, 243);
            lblIdentitySeed.Name = "lblIdentitySeed";
            lblIdentitySeed.Size = new Size(62, 25);
            lblIdentitySeed.TabIndex = 24;
            lblIdentitySeed.Text = "Seed :";
            // 
            // cbxIsPrimaryKey
            // 
            cbxIsPrimaryKey.AutoSize = true;
            cbxIsPrimaryKey.Location = new Point(410, 296);
            cbxIsPrimaryKey.Name = "cbxIsPrimaryKey";
            cbxIsPrimaryKey.Size = new Size(146, 29);
            cbxIsPrimaryKey.TabIndex = 21;
            cbxIsPrimaryKey.Text = "IsPrimaryKey";
            cbxIsPrimaryKey.UseVisualStyleBackColor = true;
            cbxIsPrimaryKey.CheckedChanged += cbxIsPrimaryKey_CheckedChanged;
            // 
            // lblDefaultValue
            // 
            lblDefaultValue.AutoSize = true;
            lblDefaultValue.Location = new Point(23, 295);
            lblDefaultValue.Name = "lblDefaultValue";
            lblDefaultValue.Size = new Size(133, 25);
            lblDefaultValue.TabIndex = 20;
            lblDefaultValue.Text = "Default Value :";
            // 
            // cbxIsIdentity
            // 
            cbxIsIdentity.AutoSize = true;
            cbxIsIdentity.Location = new Point(315, 241);
            cbxIsIdentity.Name = "cbxIsIdentity";
            cbxIsIdentity.Size = new Size(116, 29);
            cbxIsIdentity.TabIndex = 19;
            cbxIsIdentity.Text = "IsIdentity";
            cbxIsIdentity.UseVisualStyleBackColor = true;
            cbxIsIdentity.CheckedChanged += cbxIsIdentity_CheckedChanged;
            // 
            // lblPkName
            // 
            lblPkName.AutoSize = true;
            lblPkName.Location = new Point(554, 297);
            lblPkName.Name = "lblPkName";
            lblPkName.Size = new Size(97, 25);
            lblPkName.TabIndex = 18;
            lblPkName.Text = "PK Name :";
            // 
            // cbxIsNullable
            // 
            cbxIsNullable.AutoSize = true;
            cbxIsNullable.Location = new Point(23, 241);
            cbxIsNullable.Name = "cbxIsNullable";
            cbxIsNullable.Size = new Size(121, 29);
            cbxIsNullable.TabIndex = 17;
            cbxIsNullable.Text = "IsNullable";
            cbxIsNullable.UseVisualStyleBackColor = true;
            // 
            // numPrecision
            // 
            numPrecision.Location = new Point(372, 174);
            numPrecision.Name = "numPrecision";
            numPrecision.Size = new Size(142, 31);
            numPrecision.TabIndex = 15;
            // 
            // lblPrecision
            // 
            lblPrecision.AutoSize = true;
            lblPrecision.Location = new Point(264, 176);
            lblPrecision.Name = "lblPrecision";
            lblPrecision.Size = new Size(97, 25);
            lblPrecision.TabIndex = 14;
            lblPrecision.Text = "Precision :";
            // 
            // numScale
            // 
            numScale.Location = new Point(607, 174);
            numScale.Name = "numScale";
            numScale.Size = new Size(135, 31);
            numScale.TabIndex = 13;
            // 
            // lblScale
            // 
            lblScale.AutoSize = true;
            lblScale.Location = new Point(536, 176);
            lblScale.Name = "lblScale";
            lblScale.Size = new Size(63, 25);
            lblScale.TabIndex = 12;
            lblScale.Text = "Scale :";
            // 
            // numLength
            // 
            numLength.Location = new Point(112, 174);
            numLength.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numLength.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            numLength.Name = "numLength";
            numLength.Size = new Size(135, 31);
            numLength.TabIndex = 11;
            // 
            // lblLength
            // 
            lblLength.AutoSize = true;
            lblLength.Location = new Point(23, 176);
            lblLength.Name = "lblLength";
            lblLength.Size = new Size(79, 25);
            lblLength.TabIndex = 10;
            lblLength.Text = "Length :";
            // 
            // cmbDataTypes
            // 
            cmbDataTypes.FormattingEnabled = true;
            cmbDataTypes.Location = new Point(674, 113);
            cmbDataTypes.Name = "cmbDataTypes";
            cmbDataTypes.Size = new Size(285, 33);
            cmbDataTypes.TabIndex = 9;
            // 
            // txtColumnName
            // 
            txtColumnName.Location = new Point(181, 113);
            txtColumnName.Name = "txtColumnName";
            txtColumnName.Size = new Size(322, 31);
            txtColumnName.TabIndex = 8;
            // 
            // lblDataType
            // 
            lblDataType.AutoSize = true;
            lblDataType.Location = new Point(549, 116);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new Size(103, 25);
            lblDataType.TabIndex = 7;
            lblDataType.Text = "Data Type :";
            // 
            // lblColumnName
            // 
            lblColumnName.AutoSize = true;
            lblColumnName.Location = new Point(23, 116);
            lblColumnName.Name = "lblColumnName";
            lblColumnName.Size = new Size(140, 25);
            lblColumnName.TabIndex = 6;
            lblColumnName.Text = "Column Name :";
            // 
            // cmbTables2
            // 
            cmbTables2.FormattingEnabled = true;
            cmbTables2.Location = new Point(702, 49);
            cmbTables2.Name = "cmbTables2";
            cmbTables2.Size = new Size(320, 33);
            cmbTables2.TabIndex = 5;
            cmbTables2.SelectedIndexChanged += cmbTables_SelectedIndexChanged;
            // 
            // lblTables
            // 
            lblTables.AutoSize = true;
            lblTables.Location = new Point(632, 52);
            lblTables.Name = "lblTables";
            lblTables.Size = new Size(64, 25);
            lblTables.TabIndex = 4;
            lblTables.Text = "Table :";
            // 
            // cmbSchemas2
            // 
            cmbSchemas2.FormattingEnabled = true;
            cmbSchemas2.Location = new Point(441, 49);
            cmbSchemas2.Name = "cmbSchemas2";
            cmbSchemas2.Size = new Size(170, 33);
            cmbSchemas2.TabIndex = 3;
            cmbSchemas2.SelectedIndexChanged += cmbSchemas_SelectedIndexChanged;
            // 
            // lblSchemas
            // 
            lblSchemas.AutoSize = true;
            lblSchemas.Location = new Point(351, 52);
            lblSchemas.Name = "lblSchemas";
            lblSchemas.Size = new Size(84, 25);
            lblSchemas.TabIndex = 2;
            lblSchemas.Text = "Schema :";
            // 
            // cmbDatabases3
            // 
            cmbDatabases3.FormattingEnabled = true;
            cmbDatabases3.Location = new Point(126, 49);
            cmbDatabases3.Name = "cmbDatabases3";
            cmbDatabases3.Size = new Size(205, 33);
            cmbDatabases3.TabIndex = 1;
            cmbDatabases3.SelectedIndexChanged += cmbDatabases_SelectedIndexChanged;
            // 
            // lblDatabases3
            // 
            lblDatabases3.AutoSize = true;
            lblDatabases3.Location = new Point(23, 52);
            lblDatabases3.Name = "lblDatabases3";
            lblDatabases3.Size = new Size(97, 25);
            lblDatabases3.TabIndex = 0;
            lblDatabases3.Text = "Database :";
            // 
            // tabIndex
            // 
            tabIndex.Controls.Add(gbx5IndexDetails);
            tabIndex.Location = new Point(4, 34);
            tabIndex.Name = "tabIndex";
            tabIndex.Padding = new Padding(3);
            tabIndex.Size = new Size(1068, 494);
            tabIndex.TabIndex = 3;
            tabIndex.Text = "Index";
            tabIndex.UseVisualStyleBackColor = true;
            // 
            // gbx5IndexDetails
            // 
            gbx5IndexDetails.Controls.Add(lblIndexes);
            gbx5IndexDetails.Controls.Add(cmbIndexes);
            gbx5IndexDetails.Controls.Add(cbxIgnoreIfNotExists);
            gbx5IndexDetails.Controls.Add(txtFilterPredicate);
            gbx5IndexDetails.Controls.Add(lblFilterPredicate);
            gbx5IndexDetails.Controls.Add(cbxDropExisting);
            gbx5IndexDetails.Controls.Add(cmbFileGroups2);
            gbx5IndexDetails.Controls.Add(lblFileGroup2);
            gbx5IndexDetails.Controls.Add(cbxAllowPageLocks);
            gbx5IndexDetails.Controls.Add(cbxAllowRowLocks);
            gbx5IndexDetails.Controls.Add(cbxPadIndex);
            gbx5IndexDetails.Controls.Add(numFillFactor);
            gbx5IndexDetails.Controls.Add(lblFillFactor);
            gbx5IndexDetails.Controls.Add(cmbIndexType);
            gbx5IndexDetails.Controls.Add(lblIndexType);
            gbx5IndexDetails.Controls.Add(cbxIsUnique);
            gbx5IndexDetails.Controls.Add(txtIndexName);
            gbx5IndexDetails.Controls.Add(lblIndexName);
            gbx5IndexDetails.Controls.Add(txtIncludeColumns);
            gbx5IndexDetails.Controls.Add(lblIncludeColumns);
            gbx5IndexDetails.Controls.Add(txtColumns);
            gbx5IndexDetails.Controls.Add(lblColumns);
            gbx5IndexDetails.Controls.Add(cmbTables3);
            gbx5IndexDetails.Controls.Add(lblDatabases4);
            gbx5IndexDetails.Controls.Add(lblTables2);
            gbx5IndexDetails.Controls.Add(cmbDatabases4);
            gbx5IndexDetails.Controls.Add(cmbSchemas3);
            gbx5IndexDetails.Controls.Add(lblSchemas2);
            gbx5IndexDetails.Location = new Point(11, 12);
            gbx5IndexDetails.Name = "gbx5IndexDetails";
            gbx5IndexDetails.Size = new Size(1042, 466);
            gbx5IndexDetails.TabIndex = 12;
            gbx5IndexDetails.TabStop = false;
            gbx5IndexDetails.Text = "Index Details";
            // 
            // lblIndexes
            // 
            lblIndexes.AutoSize = true;
            lblIndexes.Location = new Point(33, 115);
            lblIndexes.Name = "lblIndexes";
            lblIndexes.Size = new Size(67, 25);
            lblIndexes.TabIndex = 33;
            lblIndexes.Text = "Index :";
            // 
            // cmbIndexes
            // 
            cmbIndexes.FormattingEnabled = true;
            cmbIndexes.Location = new Point(126, 112);
            cmbIndexes.Name = "cmbIndexes";
            cmbIndexes.Size = new Size(805, 33);
            cmbIndexes.TabIndex = 32;
            // 
            // cbxIgnoreIfNotExists
            // 
            cbxIgnoreIfNotExists.AutoSize = true;
            cbxIgnoreIfNotExists.Location = new Point(68, 169);
            cbxIgnoreIfNotExists.Name = "cbxIgnoreIfNotExists";
            cbxIgnoreIfNotExists.Size = new Size(197, 29);
            cbxIgnoreIfNotExists.TabIndex = 31;
            cbxIgnoreIfNotExists.Text = "Ignore If Not Exists";
            cbxIgnoreIfNotExists.UseVisualStyleBackColor = true;
            // 
            // txtFilterPredicate
            // 
            txtFilterPredicate.Location = new Point(182, 387);
            txtFilterPredicate.Multiline = true;
            txtFilterPredicate.Name = "txtFilterPredicate";
            txtFilterPredicate.Size = new Size(788, 70);
            txtFilterPredicate.TabIndex = 30;
            // 
            // lblFilterPredicate
            // 
            lblFilterPredicate.AutoSize = true;
            lblFilterPredicate.Location = new Point(23, 387);
            lblFilterPredicate.Name = "lblFilterPredicate";
            lblFilterPredicate.Size = new Size(147, 25);
            lblFilterPredicate.TabIndex = 29;
            lblFilterPredicate.Text = "Filter Predicate :";
            // 
            // cbxDropExisting
            // 
            cbxDropExisting.AutoSize = true;
            cbxDropExisting.Location = new Point(734, 288);
            cbxDropExisting.Name = "cbxDropExisting";
            cbxDropExisting.Size = new Size(150, 29);
            cbxDropExisting.TabIndex = 28;
            cbxDropExisting.Text = "Drop Existing";
            cbxDropExisting.UseVisualStyleBackColor = true;
            // 
            // cmbFileGroups2
            // 
            cmbFileGroups2.FormattingEnabled = true;
            cmbFileGroups2.Location = new Point(133, 335);
            cmbFileGroups2.Name = "cmbFileGroups2";
            cmbFileGroups2.Size = new Size(316, 33);
            cmbFileGroups2.TabIndex = 27;
            // 
            // lblFileGroup2
            // 
            lblFileGroup2.AutoSize = true;
            lblFileGroup2.Location = new Point(23, 338);
            lblFileGroup2.Name = "lblFileGroup2";
            lblFileGroup2.Size = new Size(108, 25);
            lblFileGroup2.TabIndex = 26;
            lblFileGroup2.Text = "File Group :";
            // 
            // cbxAllowPageLocks
            // 
            cbxAllowPageLocks.AutoSize = true;
            cbxAllowPageLocks.Location = new Point(383, 288);
            cbxAllowPageLocks.Name = "cbxAllowPageLocks";
            cbxAllowPageLocks.Size = new Size(180, 29);
            cbxAllowPageLocks.TabIndex = 25;
            cbxAllowPageLocks.Text = "Allow Page Locks";
            cbxAllowPageLocks.UseVisualStyleBackColor = true;
            // 
            // cbxAllowRowLocks
            // 
            cbxAllowRowLocks.AutoSize = true;
            cbxAllowRowLocks.Location = new Point(182, 288);
            cbxAllowRowLocks.Name = "cbxAllowRowLocks";
            cbxAllowRowLocks.Size = new Size(176, 29);
            cbxAllowRowLocks.TabIndex = 24;
            cbxAllowRowLocks.Text = "Allow Row Locks";
            cbxAllowRowLocks.UseVisualStyleBackColor = true;
            // 
            // cbxPadIndex
            // 
            cbxPadIndex.AutoSize = true;
            cbxPadIndex.Location = new Point(590, 288);
            cbxPadIndex.Name = "cbxPadIndex";
            cbxPadIndex.Size = new Size(119, 29);
            cbxPadIndex.TabIndex = 23;
            cbxPadIndex.Text = "Pad Index";
            cbxPadIndex.UseVisualStyleBackColor = true;
            // 
            // numFillFactor
            // 
            numFillFactor.Location = new Point(599, 336);
            numFillFactor.Name = "numFillFactor";
            numFillFactor.Size = new Size(180, 31);
            numFillFactor.TabIndex = 22;
            // 
            // lblFillFactor
            // 
            lblFillFactor.AutoSize = true;
            lblFillFactor.Location = new Point(491, 338);
            lblFillFactor.Name = "lblFillFactor";
            lblFillFactor.Size = new Size(100, 25);
            lblFillFactor.TabIndex = 21;
            lblFillFactor.Text = "Fill Factor :";
            // 
            // cmbIndexType
            // 
            cmbIndexType.FormattingEnabled = true;
            cmbIndexType.Location = new Point(665, 230);
            cmbIndexType.Name = "cmbIndexType";
            cmbIndexType.Size = new Size(305, 33);
            cmbIndexType.TabIndex = 20;
            // 
            // lblIndexType
            // 
            lblIndexType.AutoSize = true;
            lblIndexType.Location = new Point(548, 233);
            lblIndexType.Name = "lblIndexType";
            lblIndexType.Size = new Size(111, 25);
            lblIndexType.TabIndex = 19;
            lblIndexType.Text = "Index Type :";
            // 
            // cbxIsUnique
            // 
            cbxIsUnique.AutoSize = true;
            cbxIsUnique.Location = new Point(47, 288);
            cbxIsUnique.Name = "cbxIsUnique";
            cbxIsUnique.Size = new Size(112, 29);
            cbxIsUnique.TabIndex = 18;
            cbxIsUnique.Text = "IsUnique";
            cbxIsUnique.UseVisualStyleBackColor = true;
            // 
            // txtIndexName
            // 
            txtIndexName.Location = new Point(150, 232);
            txtIndexName.Name = "txtIndexName";
            txtIndexName.Size = new Size(392, 31);
            txtIndexName.TabIndex = 17;
            // 
            // lblIndexName
            // 
            lblIndexName.AutoSize = true;
            lblIndexName.Location = new Point(23, 233);
            lblIndexName.Name = "lblIndexName";
            lblIndexName.Size = new Size(121, 25);
            lblIndexName.TabIndex = 16;
            lblIndexName.Text = "Index Name :";
            // 
            // txtIncludeColumns
            // 
            txtIncludeColumns.Location = new Point(189, 170);
            txtIncludeColumns.Multiline = true;
            txtIncludeColumns.Name = "txtIncludeColumns";
            txtIncludeColumns.Size = new Size(742, 46);
            txtIncludeColumns.TabIndex = 15;
            // 
            // lblIncludeColumns
            // 
            lblIncludeColumns.AutoSize = true;
            lblIncludeColumns.Location = new Point(23, 173);
            lblIncludeColumns.Name = "lblIncludeColumns";
            lblIncludeColumns.Size = new Size(160, 25);
            lblIncludeColumns.TabIndex = 14;
            lblIncludeColumns.Text = "Include Columns :";
            // 
            // txtColumns
            // 
            txtColumns.Location = new Point(126, 115);
            txtColumns.Name = "txtColumns";
            txtColumns.Size = new Size(805, 31);
            txtColumns.TabIndex = 13;
            // 
            // lblColumns
            // 
            lblColumns.AutoSize = true;
            lblColumns.Location = new Point(23, 115);
            lblColumns.Name = "lblColumns";
            lblColumns.Size = new Size(94, 25);
            lblColumns.TabIndex = 12;
            lblColumns.Text = "Columns :";
            // 
            // cmbTables3
            // 
            cmbTables3.FormattingEnabled = true;
            cmbTables3.Location = new Point(702, 57);
            cmbTables3.Name = "cmbTables3";
            cmbTables3.Size = new Size(320, 33);
            cmbTables3.TabIndex = 11;
            cmbTables3.SelectedIndexChanged += cmbTables_SelectedIndexChanged;
            // 
            // lblDatabases4
            // 
            lblDatabases4.AutoSize = true;
            lblDatabases4.Location = new Point(23, 60);
            lblDatabases4.Name = "lblDatabases4";
            lblDatabases4.Size = new Size(97, 25);
            lblDatabases4.TabIndex = 6;
            lblDatabases4.Text = "Database :";
            // 
            // lblTables2
            // 
            lblTables2.AutoSize = true;
            lblTables2.Location = new Point(632, 60);
            lblTables2.Name = "lblTables2";
            lblTables2.Size = new Size(64, 25);
            lblTables2.TabIndex = 10;
            lblTables2.Text = "Table :";
            // 
            // cmbDatabases4
            // 
            cmbDatabases4.FormattingEnabled = true;
            cmbDatabases4.Location = new Point(126, 57);
            cmbDatabases4.Name = "cmbDatabases4";
            cmbDatabases4.Size = new Size(205, 33);
            cmbDatabases4.TabIndex = 7;
            cmbDatabases4.SelectedIndexChanged += cmbDatabases_SelectedIndexChanged;
            // 
            // cmbSchemas3
            // 
            cmbSchemas3.FormattingEnabled = true;
            cmbSchemas3.Location = new Point(441, 57);
            cmbSchemas3.Name = "cmbSchemas3";
            cmbSchemas3.Size = new Size(170, 33);
            cmbSchemas3.TabIndex = 9;
            cmbSchemas3.SelectedIndexChanged += cmbSchemas_SelectedIndexChanged;
            // 
            // lblSchemas2
            // 
            lblSchemas2.AutoSize = true;
            lblSchemas2.Location = new Point(351, 60);
            lblSchemas2.Name = "lblSchemas2";
            lblSchemas2.Size = new Size(84, 25);
            lblSchemas2.TabIndex = 8;
            lblSchemas2.Text = "Schema :";
            // 
            // tabForeignKey
            // 
            tabForeignKey.Controls.Add(gbx6ForeignKeyDetails);
            tabForeignKey.Location = new Point(4, 34);
            tabForeignKey.Name = "tabForeignKey";
            tabForeignKey.Padding = new Padding(3);
            tabForeignKey.Size = new Size(1068, 494);
            tabForeignKey.TabIndex = 4;
            tabForeignKey.Text = "Foreign Key";
            tabForeignKey.UseVisualStyleBackColor = true;
            // 
            // gbx6ForeignKeyDetails
            // 
            gbx6ForeignKeyDetails.Controls.Add(lblFkName);
            gbx6ForeignKeyDetails.Controls.Add(txtFkName);
            gbx6ForeignKeyDetails.Controls.Add(cbxIsNotForReplication);
            gbx6ForeignKeyDetails.Controls.Add(cbxEnabled);
            gbx6ForeignKeyDetails.Controls.Add(cmbOnUpdate);
            gbx6ForeignKeyDetails.Controls.Add(lblOnUpdate);
            gbx6ForeignKeyDetails.Controls.Add(cmbOnDelete);
            gbx6ForeignKeyDetails.Controls.Add(lblOnDelete);
            gbx6ForeignKeyDetails.Controls.Add(cmbColumns3);
            gbx6ForeignKeyDetails.Controls.Add(lblColumns3);
            gbx6ForeignKeyDetails.Controls.Add(cmbTables5);
            gbx6ForeignKeyDetails.Controls.Add(lblTables4);
            gbx6ForeignKeyDetails.Controls.Add(cmbSchemas5);
            gbx6ForeignKeyDetails.Controls.Add(lblSchemas4);
            gbx6ForeignKeyDetails.Controls.Add(cmbColumns2);
            gbx6ForeignKeyDetails.Controls.Add(lblColumns2);
            gbx6ForeignKeyDetails.Controls.Add(cmbSchemas4);
            gbx6ForeignKeyDetails.Controls.Add(lblSchemas3);
            gbx6ForeignKeyDetails.Controls.Add(cmbTables4);
            gbx6ForeignKeyDetails.Controls.Add(lblTables3);
            gbx6ForeignKeyDetails.Controls.Add(cmbDatabases5);
            gbx6ForeignKeyDetails.Controls.Add(lblDatabases5);
            gbx6ForeignKeyDetails.Location = new Point(10, 11);
            gbx6ForeignKeyDetails.Name = "gbx6ForeignKeyDetails";
            gbx6ForeignKeyDetails.Size = new Size(1048, 474);
            gbx6ForeignKeyDetails.TabIndex = 0;
            gbx6ForeignKeyDetails.TabStop = false;
            gbx6ForeignKeyDetails.Text = "Foreign Key Details";
            // 
            // lblFkName
            // 
            lblFkName.AutoSize = true;
            lblFkName.Location = new Point(32, 329);
            lblFkName.Name = "lblFkName";
            lblFkName.Size = new Size(95, 25);
            lblFkName.TabIndex = 21;
            lblFkName.Text = "FK Name :";
            // 
            // txtFkName
            // 
            txtFkName.Location = new Point(136, 326);
            txtFkName.Name = "txtFkName";
            txtFkName.Size = new Size(762, 31);
            txtFkName.TabIndex = 20;
            // 
            // cbxIsNotForReplication
            // 
            cbxIsNotForReplication.AutoSize = true;
            cbxIsNotForReplication.Location = new Point(203, 406);
            cbxIsNotForReplication.Name = "cbxIsNotForReplication";
            cbxIsNotForReplication.Size = new Size(218, 29);
            cbxIsNotForReplication.TabIndex = 19;
            cbxIsNotForReplication.Text = "Is Not For Replication";
            cbxIsNotForReplication.UseVisualStyleBackColor = true;
            // 
            // cbxEnabled
            // 
            cbxEnabled.AutoSize = true;
            cbxEnabled.Location = new Point(32, 406);
            cbxEnabled.Name = "cbxEnabled";
            cbxEnabled.Size = new Size(104, 29);
            cbxEnabled.TabIndex = 18;
            cbxEnabled.Text = "Enabled";
            cbxEnabled.UseVisualStyleBackColor = true;
            // 
            // cmbOnUpdate
            // 
            cmbOnUpdate.FormattingEnabled = true;
            cmbOnUpdate.Location = new Point(694, 339);
            cmbOnUpdate.Name = "cmbOnUpdate";
            cmbOnUpdate.Size = new Size(276, 33);
            cmbOnUpdate.TabIndex = 17;
            // 
            // lblOnUpdate
            // 
            lblOnUpdate.AutoSize = true;
            lblOnUpdate.Location = new Point(517, 342);
            lblOnUpdate.Name = "lblOnUpdate";
            lblOnUpdate.Size = new Size(171, 25);
            lblOnUpdate.TabIndex = 16;
            lblOnUpdate.Text = "On Update Action :";
            // 
            // cmbOnDelete
            // 
            cmbOnDelete.FormattingEnabled = true;
            cmbOnDelete.Location = new Point(203, 339);
            cmbOnDelete.Name = "cmbOnDelete";
            cmbOnDelete.Size = new Size(293, 33);
            cmbOnDelete.TabIndex = 15;
            // 
            // lblOnDelete
            // 
            lblOnDelete.AutoSize = true;
            lblOnDelete.Location = new Point(32, 342);
            lblOnDelete.Name = "lblOnDelete";
            lblOnDelete.Size = new Size(165, 25);
            lblOnDelete.TabIndex = 14;
            lblOnDelete.Text = "On Delete Action :";
            // 
            // cmbColumns3
            // 
            cmbColumns3.FormattingEnabled = true;
            cmbColumns3.Location = new Point(154, 271);
            cmbColumns3.Name = "cmbColumns3";
            cmbColumns3.Size = new Size(744, 33);
            cmbColumns3.TabIndex = 13;
            // 
            // lblColumns3
            // 
            lblColumns3.AutoSize = true;
            lblColumns3.Location = new Point(32, 274);
            lblColumns3.Name = "lblColumns3";
            lblColumns3.Size = new Size(118, 25);
            lblColumns3.TabIndex = 12;
            lblColumns3.Text = "Ref Column :";
            // 
            // cmbTables5
            // 
            cmbTables5.FormattingEnabled = true;
            cmbTables5.Location = new Point(517, 220);
            cmbTables5.Name = "cmbTables5";
            cmbTables5.Size = new Size(381, 33);
            cmbTables5.TabIndex = 11;
            cmbTables5.SelectedIndexChanged += cmbTables_SelectedIndexChanged;
            // 
            // lblTables4
            // 
            lblTables4.AutoSize = true;
            lblTables4.Location = new Point(415, 223);
            lblTables4.Name = "lblTables4";
            lblTables4.Size = new Size(96, 25);
            lblTables4.TabIndex = 10;
            lblTables4.Text = "Ref Table :";
            // 
            // cmbSchemas5
            // 
            cmbSchemas5.FormattingEnabled = true;
            cmbSchemas5.Location = new Point(154, 220);
            cmbSchemas5.Name = "cmbSchemas5";
            cmbSchemas5.Size = new Size(242, 33);
            cmbSchemas5.TabIndex = 9;
            cmbSchemas5.SelectedIndexChanged += cmbSchemas_SelectedIndexChanged;
            // 
            // lblSchemas4
            // 
            lblSchemas4.AutoSize = true;
            lblSchemas4.Location = new Point(32, 223);
            lblSchemas4.Name = "lblSchemas4";
            lblSchemas4.Size = new Size(116, 25);
            lblSchemas4.TabIndex = 8;
            lblSchemas4.Text = "Ref Schema :";
            // 
            // cmbColumns2
            // 
            cmbColumns2.FormattingEnabled = true;
            cmbColumns2.Location = new Point(124, 165);
            cmbColumns2.Name = "cmbColumns2";
            cmbColumns2.Size = new Size(750, 33);
            cmbColumns2.TabIndex = 7;
            // 
            // lblColumns2
            // 
            lblColumns2.AutoSize = true;
            lblColumns2.Location = new Point(32, 168);
            lblColumns2.Name = "lblColumns2";
            lblColumns2.Size = new Size(86, 25);
            lblColumns2.TabIndex = 6;
            lblColumns2.Text = "Column :";
            // 
            // cmbSchemas4
            // 
            cmbSchemas4.FormattingEnabled = true;
            cmbSchemas4.Location = new Point(122, 115);
            cmbSchemas4.Name = "cmbSchemas4";
            cmbSchemas4.Size = new Size(236, 33);
            cmbSchemas4.TabIndex = 5;
            cmbSchemas4.SelectedIndexChanged += cmbSchemas_SelectedIndexChanged;
            // 
            // lblSchemas3
            // 
            lblSchemas3.AutoSize = true;
            lblSchemas3.Location = new Point(32, 118);
            lblSchemas3.Name = "lblSchemas3";
            lblSchemas3.Size = new Size(84, 25);
            lblSchemas3.TabIndex = 4;
            lblSchemas3.Text = "Schema :";
            // 
            // cmbTables4
            // 
            cmbTables4.FormattingEnabled = true;
            cmbTables4.Location = new Point(454, 115);
            cmbTables4.Name = "cmbTables4";
            cmbTables4.Size = new Size(420, 33);
            cmbTables4.TabIndex = 3;
            cmbTables4.SelectedIndexChanged += cmbTables_SelectedIndexChanged;
            // 
            // lblTables3
            // 
            lblTables3.AutoSize = true;
            lblTables3.Location = new Point(384, 118);
            lblTables3.Name = "lblTables3";
            lblTables3.Size = new Size(64, 25);
            lblTables3.TabIndex = 2;
            lblTables3.Text = "Table :";
            // 
            // cmbDatabases5
            // 
            cmbDatabases5.FormattingEnabled = true;
            cmbDatabases5.Location = new Point(136, 54);
            cmbDatabases5.Name = "cmbDatabases5";
            cmbDatabases5.Size = new Size(309, 33);
            cmbDatabases5.TabIndex = 1;
            cmbDatabases5.SelectedIndexChanged += cmbDatabases_SelectedIndexChanged;
            // 
            // lblDatabases5
            // 
            lblDatabases5.AutoSize = true;
            lblDatabases5.Location = new Point(32, 57);
            lblDatabases5.Name = "lblDatabases5";
            lblDatabases5.Size = new Size(97, 25);
            lblDatabases5.TabIndex = 0;
            lblDatabases5.Text = "Database :";
            // 
            // tabBackupAndRestore
            // 
            tabBackupAndRestore.Controls.Add(gbx8Restore);
            tabBackupAndRestore.Controls.Add(gbx7Backup);
            tabBackupAndRestore.Location = new Point(4, 34);
            tabBackupAndRestore.Name = "tabBackupAndRestore";
            tabBackupAndRestore.Padding = new Padding(3);
            tabBackupAndRestore.Size = new Size(1068, 494);
            tabBackupAndRestore.TabIndex = 5;
            tabBackupAndRestore.Text = "Backup/Restore";
            tabBackupAndRestore.UseVisualStyleBackColor = true;
            // 
            // gbx8Restore
            // 
            gbx8Restore.Controls.Add(btnBrowseRestorePath);
            gbx8Restore.Controls.Add(txtRestorePath);
            gbx8Restore.Controls.Add(lblRestorePath);
            gbx8Restore.Controls.Add(cmbDatabases7);
            gbx8Restore.Controls.Add(lblDatabases7);
            gbx8Restore.Location = new Point(48, 253);
            gbx8Restore.Name = "gbx8Restore";
            gbx8Restore.Size = new Size(962, 202);
            gbx8Restore.TabIndex = 1;
            gbx8Restore.TabStop = false;
            gbx8Restore.Text = "Restore";
            // 
            // btnBrowseRestorePath
            // 
            btnBrowseRestorePath.Location = new Point(821, 87);
            btnBrowseRestorePath.Name = "btnBrowseRestorePath";
            btnBrowseRestorePath.Size = new Size(94, 34);
            btnBrowseRestorePath.TabIndex = 9;
            btnBrowseRestorePath.Text = "Browse";
            btnBrowseRestorePath.UseVisualStyleBackColor = true;
            btnBrowseRestorePath.Click += btnBrowseRestorePath_Click;
            // 
            // txtRestorePath
            // 
            txtRestorePath.Location = new Point(200, 127);
            txtRestorePath.Name = "txtRestorePath";
            txtRestorePath.ReadOnly = true;
            txtRestorePath.Size = new Size(715, 31);
            txtRestorePath.TabIndex = 8;
            // 
            // lblRestorePath
            // 
            lblRestorePath.AutoSize = true;
            lblRestorePath.Location = new Point(68, 130);
            lblRestorePath.Name = "lblRestorePath";
            lblRestorePath.Size = new Size(126, 25);
            lblRestorePath.TabIndex = 7;
            lblRestorePath.Text = "Restore Path :";
            // 
            // cmbDatabases7
            // 
            cmbDatabases7.FormattingEnabled = true;
            cmbDatabases7.Location = new Point(187, 65);
            cmbDatabases7.Name = "cmbDatabases7";
            cmbDatabases7.Size = new Size(415, 33);
            cmbDatabases7.TabIndex = 6;
            // 
            // lblDatabases7
            // 
            lblDatabases7.AutoSize = true;
            lblDatabases7.Location = new Point(68, 68);
            lblDatabases7.Name = "lblDatabases7";
            lblDatabases7.Size = new Size(97, 25);
            lblDatabases7.TabIndex = 5;
            lblDatabases7.Text = "Database :";
            // 
            // gbx7Backup
            // 
            gbx7Backup.Controls.Add(btnBrowseBackupPath);
            gbx7Backup.Controls.Add(txtBackupPath);
            gbx7Backup.Controls.Add(lblBackupPath);
            gbx7Backup.Controls.Add(cmbDatabases6);
            gbx7Backup.Controls.Add(lblDatabases6);
            gbx7Backup.Location = new Point(48, 39);
            gbx7Backup.Name = "gbx7Backup";
            gbx7Backup.Size = new Size(962, 197);
            gbx7Backup.TabIndex = 0;
            gbx7Backup.TabStop = false;
            gbx7Backup.Text = "Backup";
            // 
            // btnBrowseBackupPath
            // 
            btnBrowseBackupPath.Location = new Point(821, 77);
            btnBrowseBackupPath.Name = "btnBrowseBackupPath";
            btnBrowseBackupPath.Size = new Size(94, 34);
            btnBrowseBackupPath.TabIndex = 4;
            btnBrowseBackupPath.Text = "Browse";
            btnBrowseBackupPath.UseVisualStyleBackColor = true;
            btnBrowseBackupPath.Click += btnBrowseBackupPath_Click;
            // 
            // txtBackupPath
            // 
            txtBackupPath.Location = new Point(195, 117);
            txtBackupPath.Name = "txtBackupPath";
            txtBackupPath.ReadOnly = true;
            txtBackupPath.Size = new Size(720, 31);
            txtBackupPath.TabIndex = 3;
            // 
            // lblBackupPath
            // 
            lblBackupPath.AutoSize = true;
            lblBackupPath.Location = new Point(68, 120);
            lblBackupPath.Name = "lblBackupPath";
            lblBackupPath.Size = new Size(121, 25);
            lblBackupPath.TabIndex = 2;
            lblBackupPath.Text = "Backup Path :";
            // 
            // cmbDatabases6
            // 
            cmbDatabases6.FormattingEnabled = true;
            cmbDatabases6.Location = new Point(187, 58);
            cmbDatabases6.Name = "cmbDatabases6";
            cmbDatabases6.Size = new Size(415, 33);
            cmbDatabases6.TabIndex = 1;
            // 
            // lblDatabases6
            // 
            lblDatabases6.AutoSize = true;
            lblDatabases6.Location = new Point(68, 57);
            lblDatabases6.Name = "lblDatabases6";
            lblDatabases6.Size = new Size(97, 25);
            lblDatabases6.TabIndex = 0;
            lblDatabases6.Text = "Database :";
            // 
            // btnExecuteDb
            // 
            btnExecuteDb.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnExecuteDb.Location = new Point(41, 405);
            btnExecuteDb.Name = "btnExecuteDb";
            btnExecuteDb.Size = new Size(159, 84);
            btnExecuteDb.TabIndex = 1;
            btnExecuteDb.Text = "Execute";
            btnExecuteDb.UseVisualStyleBackColor = true;
            btnExecuteDb.Visible = false;
            btnExecuteDb.Click += btnExecuteDb_Click;
            // 
            // gbx1OperationType
            // 
            gbx1OperationType.Controls.Add(rbDrop);
            gbx1OperationType.Controls.Add(rbAlter);
            gbx1OperationType.Controls.Add(rbCreate);
            gbx1OperationType.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbx1OperationType.Location = new Point(23, 34);
            gbx1OperationType.Name = "gbx1OperationType";
            gbx1OperationType.Size = new Size(197, 333);
            gbx1OperationType.TabIndex = 0;
            gbx1OperationType.TabStop = false;
            gbx1OperationType.Text = "Operation Type";
            gbx1OperationType.Click += gbx1OperationType_Click;
            // 
            // rbDrop
            // 
            rbDrop.AutoSize = true;
            rbDrop.Location = new Point(15, 206);
            rbDrop.Name = "rbDrop";
            rbDrop.Size = new Size(93, 36);
            rbDrop.TabIndex = 2;
            rbDrop.TabStop = true;
            rbDrop.Text = "Drop";
            rbDrop.UseVisualStyleBackColor = true;
            // 
            // rbAlter
            // 
            rbAlter.AutoSize = true;
            rbAlter.Location = new Point(15, 148);
            rbAlter.Name = "rbAlter";
            rbAlter.Size = new Size(92, 36);
            rbAlter.TabIndex = 1;
            rbAlter.TabStop = true;
            rbAlter.Text = "Alter";
            rbAlter.UseVisualStyleBackColor = true;
            // 
            // rbCreate
            // 
            rbCreate.AutoSize = true;
            rbCreate.Location = new Point(15, 92);
            rbCreate.Name = "rbCreate";
            rbCreate.Size = new Size(111, 36);
            rbCreate.TabIndex = 0;
            rbCreate.TabStop = true;
            rbCreate.Text = "Create";
            rbCreate.UseVisualStyleBackColor = true;
            // 
            // gbx9OperationType
            // 
            gbx9OperationType.Controls.Add(rbRestore);
            gbx9OperationType.Controls.Add(rbBackup);
            gbx9OperationType.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            gbx9OperationType.Location = new Point(16, 83);
            gbx9OperationType.Name = "gbx9OperationType";
            gbx9OperationType.Size = new Size(208, 248);
            gbx9OperationType.TabIndex = 2;
            gbx9OperationType.TabStop = false;
            gbx9OperationType.Text = "Operation Type";
            gbx9OperationType.Click += gbx9OperationType_Click;
            // 
            // rbRestore
            // 
            rbRestore.AutoSize = true;
            rbRestore.Location = new Point(22, 156);
            rbRestore.Name = "rbRestore";
            rbRestore.Size = new Size(113, 34);
            rbRestore.TabIndex = 1;
            rbRestore.TabStop = true;
            rbRestore.Text = "Restore";
            rbRestore.UseVisualStyleBackColor = true;
            // 
            // rbBackup
            // 
            rbBackup.AutoSize = true;
            rbBackup.Location = new Point(22, 85);
            rbBackup.Name = "rbBackup";
            rbBackup.Size = new Size(110, 34);
            rbBackup.TabIndex = 0;
            rbBackup.TabStop = true;
            rbBackup.Text = "Backup";
            rbBackup.UseVisualStyleBackColor = true;
            // 
            // panelFooter
            // 
            panelFooter.Controls.Add(btnClearLog);
            panelFooter.Controls.Add(rtbLog);
            panelFooter.Controls.Add(lblOperationLog);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelFooter.Location = new Point(0, 633);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(1318, 136);
            panelFooter.TabIndex = 2;
            // 
            // btnClearLog
            // 
            btnClearLog.BackColor = Color.Silver;
            btnClearLog.ForeColor = SystemColors.ControlText;
            btnClearLog.Location = new Point(58, 73);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(75, 34);
            btnClearLog.TabIndex = 2;
            btnClearLog.Text = "Clear";
            btnClearLog.UseVisualStyleBackColor = false;
            btnClearLog.Click += btnClearLog_Click;
            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(183, 20);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(1099, 104);
            rtbLog.TabIndex = 1;
            rtbLog.Text = "";
            // 
            // lblOperationLog
            // 
            lblOperationLog.AutoSize = true;
            lblOperationLog.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOperationLog.Location = new Point(23, 33);
            lblOperationLog.Name = "lblOperationLog";
            lblOperationLog.Size = new Size(154, 28);
            lblOperationLog.TabIndex = 0;
            lblOperationLog.Text = "Operation Log :";
            // 
            // panelOperation
            // 
            panelOperation.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelOperation.BackColor = SystemColors.Control;
            panelOperation.BackgroundImageLayout = ImageLayout.Center;
            panelOperation.Controls.Add(gbx9OperationType);
            panelOperation.Controls.Add(gbx1OperationType);
            panelOperation.Controls.Add(btnExecuteDb);
            panelOperation.ForeColor = SystemColors.ControlText;
            panelOperation.Location = new Point(0, 95);
            panelOperation.Name = "panelOperation";
            panelOperation.Size = new Size(240, 528);
            panelOperation.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1318, 769);
            Controls.Add(panelOperation);
            Controls.Add(panelFooter);
            Controls.Add(tabControl);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainForm";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            tabControl.ResumeLayout(false);
            tabDatabase.ResumeLayout(false);
            gbx2DatabaseDetails.ResumeLayout(false);
            gbx2DatabaseDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDbFileGrowth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDbInitialSize).EndInit();
            tabTable.ResumeLayout(false);
            gbx3TableDetails.ResumeLayout(false);
            gbx3TableDetails.PerformLayout();
            tabColumn.ResumeLayout(false);
            gbx4ColumnDetails.ResumeLayout(false);
            gbx4ColumnDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIdentityIncrement).EndInit();
            ((System.ComponentModel.ISupportInitialize)numIdentitySeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrecision).EndInit();
            ((System.ComponentModel.ISupportInitialize)numScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLength).EndInit();
            tabIndex.ResumeLayout(false);
            gbx5IndexDetails.ResumeLayout(false);
            gbx5IndexDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFillFactor).EndInit();
            tabForeignKey.ResumeLayout(false);
            gbx6ForeignKeyDetails.ResumeLayout(false);
            gbx6ForeignKeyDetails.PerformLayout();
            tabBackupAndRestore.ResumeLayout(false);
            gbx8Restore.ResumeLayout(false);
            gbx8Restore.PerformLayout();
            gbx7Backup.ResumeLayout(false);
            gbx7Backup.PerformLayout();
            gbx1OperationType.ResumeLayout(false);
            gbx1OperationType.PerformLayout();
            gbx9OperationType.ResumeLayout(false);
            gbx9OperationType.PerformLayout();
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            panelOperation.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private ComboBox cmbServers;
        private Label lblSelectServer;
        private Button btnSetting;
        private Button btnRefresh;
        private TabControl tabControl;
        private TabPage tabDatabase;
        private TabPage tabTable;
        private TabPage tabColumn;
        private TabPage tabIndex;
        private TabPage tabForeignKey;
        private TabPage tabBackupAndRestore;
        private Panel panelFooter;
        private RichTextBox rtbLog;
        private Label lblOperationLog;
        private GroupBox gbx1OperationType;
        private RadioButton rbDrop;
        private RadioButton rbAlter;
        private RadioButton rbCreate;
        private Button btnExecuteDb;
        private GroupBox gbx2DatabaseDetails;
        private Label lblDataFilePath;
        private Label lblCollation;
        private NumericUpDown numDbFileGrowth;
        private Label lblFileGrowth;
        private NumericUpDown numDbInitialSize;
        private Label lblInitialSize;
        private TextBox txtDbName;
        private Label lblDbName;
        private ComboBox cmbCollations;
        private Button btnBrowseDataFilePath;
        private TextBox txtDataFilePath;
        private Label lblDatabases;
        private ComboBox cmbDatabases;
        private CheckBox cbxForceDisconnect;
        private Panel panelOperation;
        private GroupBox gbx3TableDetails;
        private Label lblTableName;
        private TextBox txtSchemaName;
        private Label lblSchemaName;
        private ComboBox cmbDatabases2;
        private Label lblDatabases2;
        private TextBox txtDescription;
        private Label lblDescription;
        private ComboBox cmbFileGroups;
        private Label lblFileGroup;
        private TextBox txtTableName;
        private ComboBox cmbTables;
        private ComboBox cmbSchemas;
        private CheckBox cbxForceDropDependent;
        private GroupBox gbx4ColumnDetails;
        private Label lblPkName;
        private CheckBox cbxIsNullable;
        private NumericUpDown numPrecision;
        private Label lblPrecision;
        private NumericUpDown numScale;
        private Label lblScale;
        private NumericUpDown numLength;
        private Label lblLength;
        private ComboBox cmbDataTypes;
        private TextBox txtColumnName;
        private Label lblDataType;
        private Label lblColumnName;
        private ComboBox cmbTables2;
        private Label lblTables;
        private ComboBox cmbSchemas2;
        private Label lblSchemas;
        private ComboBox cmbDatabases3;
        private Label lblDatabases3;
        private NumericUpDown numIdentitySeed;
        private Label lblIdentitySeed;
        private CheckBox cbxIsPrimaryKey;
        private Label lblDefaultValue;
        private CheckBox cbxIsIdentity;
        private ComboBox cmbColumns;
        private TextBox txtPkName;
        private TextBox txtDefaultValue;
        private Label lblIdentityIncrement;
        private NumericUpDown numIdentityIncrement;
        private CheckBox cbxIsPersisted;
        private TextBox txtComputedFormula;
        private Label lblComputedFormula;
        private Label lblDescription2;
        private ComboBox cmbCollations2;
        private TextBox txtDescription2;
        private Label lblCollation2;
        private CheckBox cbxForceDropDependent2;
        private Button btnClearLog;
        private GroupBox gbx5IndexDetails;
        private ComboBox cmbTables3;
        private Label lblDatabases4;
        private Label lblTables2;
        private ComboBox cmbDatabases4;
        private ComboBox cmbSchemas3;
        private Label lblSchemas2;
        private CheckBox cbxAllowRowLocks;
        private CheckBox cbxPadIndex;
        private NumericUpDown numFillFactor;
        private Label lblFillFactor;
        private ComboBox cmbIndexType;
        private Label lblIndexType;
        private CheckBox cbxIsUnique;
        private TextBox txtIndexName;
        private Label lblIndexName;
        private TextBox txtIncludeColumns;
        private Label lblIncludeColumns;
        private TextBox txtColumns;
        private Label lblColumns;
        private TextBox txtFilterPredicate;
        private Label lblFilterPredicate;
        private CheckBox cbxDropExisting;
        private ComboBox cmbFileGroups2;
        private Label lblFileGroup2;
        private CheckBox cbxAllowPageLocks;
        private Label lblIndexes;
        private ComboBox cmbIndexes;
        private CheckBox cbxIgnoreIfNotExists;
        private GroupBox gbx6ForeignKeyDetails;
        private CheckBox cbxIsNotForReplication;
        private CheckBox cbxEnabled;
        private ComboBox cmbOnUpdate;
        private Label lblOnUpdate;
        private ComboBox cmbOnDelete;
        private Label lblOnDelete;
        private ComboBox cmbColumns3;
        private Label lblColumns3;
        private ComboBox cmbTables5;
        private Label lblTables4;
        private ComboBox cmbSchemas5;
        private Label lblSchemas4;
        private ComboBox cmbColumns2;
        private Label lblColumns2;
        private ComboBox cmbSchemas4;
        private Label lblSchemas3;
        private ComboBox cmbTables4;
        private Label lblTables3;
        private ComboBox cmbDatabases5;
        private Label lblDatabases5;
        private TextBox txtFkName;
        private Label lblFkName;
        private GroupBox gbx8Restore;
        private Button btnBrowseRestorePath;
        private TextBox txtRestorePath;
        private Label lblRestorePath;
        private ComboBox cmbDatabases7;
        private Label lblDatabases7;
        private GroupBox gbx7Backup;
        private Button btnBrowseBackupPath;
        private TextBox txtBackupPath;
        private Label lblBackupPath;
        private ComboBox cmbDatabases6;
        private Label lblDatabases6;
        private GroupBox gbx9OperationType;
        private RadioButton rbRestore;
        private RadioButton rbBackup;
        private Button btnShowInformation;
    }
}