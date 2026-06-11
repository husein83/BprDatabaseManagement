using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DatabaseManagement.Models;
using DatabaseManagement.Business;
using DatabaseManagement.Utility;

namespace DatabaseManagement.UI
{
    public partial class MainForm : Form
    {
        #region Variables

        private DbService? _DbService = null;
        private string _CurrentServer = string.Empty;
        private bool _Initializing = false;

        #endregion

        #region Constructor
        public MainForm()
        {
            _Initializing = true;
            InitializeComponent();
            InitializeCustomComponents();
            LoadServers();
            _Initializing = false;
        }

        #endregion

        #region Private

        #region Initialize
        private void InitializeCustomComponents()
        {
            this.Text = "Database Manager";
            this.StartPosition = FormStartPosition.CenterScreen;

            gbx9OperationType.Visible = false;
            gbx2DatabaseDetails.Visible = false;
            rtbLog.BackColor = Color.FromArgb(30, 30, 30);
            rtbLog.ForeColor = Color.LightGreen;
            rtbLog.Font = new Font("Consolas", 9);

            rbCreate.CheckedChanged += DatabaseOperation_CheckedChanged;
            rbAlter.CheckedChanged += DatabaseOperation_CheckedChanged;
            rbDrop.CheckedChanged += DatabaseOperation_CheckedChanged;
            rbBackup.CheckedChanged += DatabaseOperation_CheckedChanged;
            rbRestore.CheckedChanged += DatabaseOperation_CheckedChanged;
        }
        private void InitialDbServiceForCurrentServer()
        {
            if (!string.IsNullOrWhiteSpace(_CurrentServer))
            {
                var config = AppSettings.GetServerConfiguration(_CurrentServer);

                if (config != null)
                {
                    DbServiceValidation(out bool canConnect);

                    if (canConnect)
                    {
                        LogInfo($"Connected to server: {_CurrentServer}");
                    }
                    ReloadFrom();
                }
            }
        }

        #endregion

        #region LoadData
        private void LoadServers()
        {
            try
            {
                cmbServers.Items.Clear();
                var servers = AppSettings.GetAllServerNames();

                foreach (var server in servers)
                {
                    cmbServers.Items.Add(server);
                }

                var defaultServer = AppSettings.GetDefaultServerName();
                if (!string.IsNullOrWhiteSpace(defaultServer) && cmbServers.Items.Contains(defaultServer))
                {
                    cmbServers.SelectedItem = defaultServer;
                    _CurrentServer = defaultServer;
                    InitialDbServiceForCurrentServer();
                }
                else if (cmbServers.Items.Count > 0)
                {
                    cmbServers.SelectedIndex = 0;
                }
                else
                {
                    cmbServers.SelectedIndex = -1;
                    cmbServers.ResetText();
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading servers: {ex.Message}");
            }
        }
        private void LoadDatabases(params ComboBox?[] databaseLookups)
        {
            try
            {
                if (_DbService == null || databaseLookups == null)
                    return;

                var result = _DbService.GetDatabases();

                if (result.operationResult.IsSuccess && result.databases != null)
                {
                    foreach (var item in databaseLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var db in result.databases)
                                {
                                    item.Items.Add(db);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load databases: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading databases: {ex.Message}");
            }
        }
        private void LoadSchemas(string? databaseName, params ComboBox?[] schemaLookups)
        {
            try
            {
                if (_DbService == null ||
                    string.IsNullOrWhiteSpace(databaseName) ||
                    schemaLookups == null
                   )
                    return;

                var result = _DbService.GetSchemas(databaseName);

                if (result.operationResult.IsSuccess && result.schemas != null)
                {
                    foreach (var item in schemaLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var db in result.schemas)
                                {
                                    item.Items.Add(db);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load schemas: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading schemas: {ex.Message}");
            }
        }
        private void LoadTables(string? databaseName, string? schemaName, params ComboBox?[] tableLookups)
        {
            try
            {
                if (_DbService == null ||
                    string.IsNullOrWhiteSpace(databaseName) ||
                    string.IsNullOrWhiteSpace(schemaName) ||
                    tableLookups == null
                   )
                    return;

                var result = _DbService.GetTables(databaseName, schemaName);

                if (result.operationResult.IsSuccess && result.tables != null)
                {
                    foreach (var item in tableLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var db in result.tables)
                                {
                                    item.Items.Add(db);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load tables: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading tables: {ex.Message}");
            }
        }
        private void LoadIndexNames(string? databaseName, string? schemaName, string? tableName, params ComboBox?[] indexLookups)
        {
            try
            {
                if (_DbService == null ||
                    string.IsNullOrWhiteSpace(databaseName) ||
                    string.IsNullOrWhiteSpace(schemaName) ||
                    string.IsNullOrWhiteSpace(tableName) ||
                    indexLookups == null
                   )
                    return;

                var result = _DbService.GetIndexNames(databaseName, schemaName, tableName);

                if (result.operationResult.IsSuccess && result.indexNames != null)
                {
                    foreach (var item in indexLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var db in result.indexNames)
                                {
                                    item.Items.Add(db);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load indexNames: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading indexNames: {ex.Message}");
            }
        }
        private void LoadColumns(string? databaseName, string? schemaName, string? tableName, params ComboBox?[] columnLookups)
        {
            try
            {
                if (_DbService == null ||
                    string.IsNullOrWhiteSpace(databaseName) ||
                    string.IsNullOrWhiteSpace(schemaName) ||
                    string.IsNullOrWhiteSpace(tableName) ||
                    columnLookups == null
                   )
                    return;

                var result = _DbService.GetColumns(databaseName, schemaName, tableName);

                if (result.operationResult.IsSuccess && result.columns != null)
                {
                    foreach (var item in columnLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var db in result.columns)
                                {
                                    item.Items.Add(db);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load columns: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading columns: {ex.Message}");
            }
        }
        private void LoadDataTypes(params ComboBox?[] dataTypeLookups)
        {
            try
            {
                if (_DbService == null || dataTypeLookups == null)
                    return;

                var result = _DbService.GetDataTypes();

                if (result.operationResult.IsSuccess && result.dataTypes != null)
                {
                    foreach (var item in dataTypeLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var collation in result.dataTypes)
                                {
                                    item.Items.Add(collation);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load dataTypes: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading dataTypes: {ex.Message}");
            }
        }
        private void LoadCollations(params ComboBox?[] collationLookups)
        {
            try
            {
                if (_DbService == null || collationLookups == null)
                    return;

                var result = _DbService.GetCollations();

                if (result.operationResult.IsSuccess && result.collations != null)
                {
                    foreach (var item in collationLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                item.Items.Add(Utility.Constants.COMBOBOX__EMPTY_VALUE_KEY);

                                foreach (var collation in result.collations)
                                {
                                    item.Items.Add(collation);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load collations: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading collations: {ex.Message}");
            }
        }
        private void LoadFileGroups(params ComboBox?[] fileGroupLookups)
        {
            try
            {
                if (_DbService == null || fileGroupLookups == null)
                    return;

                var result = _DbService.GetFileGroups();

                if (result.operationResult.IsSuccess && result.fileGroups != null)
                {
                    foreach (var item in fileGroupLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                item.Items.Add(Utility.Constants.COMBOBOX__EMPTY_VALUE_KEY);

                                foreach (var collation in result.fileGroups)
                                {
                                    item.Items.Add(collation);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load fileGroups: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading fileGroups: {ex.Message}");
            }
        }
        private void LoadIndexTypes(params ComboBox?[] indexTypeLookups)
        {
            try
            {
                if (_DbService == null || indexTypeLookups == null)
                    return;

                var result = _DbService.GetIndexTypes();

                if (result.operationResult.IsSuccess && result.indexTypes != null)
                {
                    foreach (var item in indexTypeLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var collation in result.indexTypes)
                                {
                                    item.Items.Add(collation);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load indexTypes: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading indexTypes: {ex.Message}");
            }
        }
        private void LoadReferentialActions(params ComboBox?[] referentialActionLookups)
        {
            try
            {
                if (_DbService == null || referentialActionLookups == null)
                    return;

                var result = _DbService.GetReferentialActions();

                if (result.operationResult.IsSuccess && result.referentialActions != null)
                {
                    foreach (var item in referentialActionLookups)
                    {
                        if (item != null)
                        {
                            item.Items.Clear();
                            item.SelectedIndex = -1;
                            item.ResetText();

                            if (item.Visible)
                            {
                                foreach (var collation in result.referentialActions)
                                {
                                    item.Items.Add(collation);
                                }

                                if (item.Items.Count > 0)
                                {
                                    item.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LogError($"Failed to load referential actions: {result.operationResult.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error loading referential actions: {ex.Message}");
            }
        }

        #endregion

        #region ElementEvents

        #region ButtonsEvent
        private void btnExecuteDb_Click(object sender, EventArgs e)
        {
            try
            {
                if (_DbService == null)
                {
                    MessageBox.Show("Please select a server first", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var activeTab = tabControl.SelectedTab;
                if (activeTab == null)
                {
                    MessageBox.Show("Please select a tab", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (activeTab == tabDatabase)
                {
                    if (rbCreate.Checked)
                    {
                        ExecuteCreateDatabase();
                    }
                    else if (rbAlter.Checked)
                    {
                        ExecuteAlterDatabase();
                    }
                    else if (rbDrop.Checked)
                    {
                        ExecuteDropDatabase();
                    }
                }
                else if (activeTab == tabTable)
                {
                    if (rbCreate.Checked)
                    {
                        ExecuteCreateTable();
                    }
                    else if (rbAlter.Checked)
                    {
                        ExecuteAlterTable();
                    }
                    else if (rbDrop.Checked)
                    {
                        ExecuteDropTable();
                    }
                }
                else if (activeTab == tabColumn)
                {
                    if (rbCreate.Checked)
                    {
                        ExecuteCreateColumn();
                    }
                    else if (rbAlter.Checked)
                    {
                        ExecuteAlterColumn();
                    }
                    else if (rbDrop.Checked)
                    {
                        ExecuteDropColumn();
                    }
                }
                else if (activeTab == tabIndex)
                {
                    if (rbCreate.Checked)
                    {
                        ExecuteCreateIndex();
                    }
                    else if (rbAlter.Checked)
                    {
                        ExecuteAlterIndex();
                    }
                    else if (rbDrop.Checked)
                    {
                        ExecuteDropIndex();
                    }
                }
                else if (activeTab == tabForeignKey)
                {
                    if (rbCreate.Checked)
                    {
                        ExecuteCreateForeignKey();
                    }
                    else if (rbAlter.Checked)
                    {
                        ExecuteAlterForeignKey();
                    }
                    else if (rbDrop.Checked)
                    {
                        ExecuteDropForeignKey();
                    }
                }
                else if (activeTab == tabBackupAndRestore)
                {
                    if (rbBackup.Checked)
                    {
                        ExecuteBackup();
                    }
                    else if (rbRestore.Checked)
                    {
                        ExecuteRestore();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            var config = AppSettings.LoadSettings();
            using var form = new SettingForm(config);
            if (form.ShowDialog() == DialogResult.OK)
            {
                AppSettings.SaveSettings(config);
            }
            LoadServers();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReloadFrom();
            LoadServers();
            LogInfo("Databases refreshed");
        }
        private void btnBrowseDataFilePath_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Database File Location";
                dialog.ShowNewFolderButton = true;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (!string.IsNullOrWhiteSpace(txtDataFilePath.Text) && Directory.Exists(txtDataFilePath.Text))
                    dialog.SelectedPath = txtDataFilePath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.Exists(dialog.SelectedPath))
                    {
                        txtDataFilePath.Text = dialog.SelectedPath;
                        LogInfo($"Data file path set to: {dialog.SelectedPath}");
                    }
                    else
                    {
                        MessageBox.Show("Selected path does not exist.", "Invalid Path",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void btnBrowseBackupPath_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Backup Path Location";
                dialog.ShowNewFolderButton = true;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (!string.IsNullOrWhiteSpace(txtBackupPath.Text) && Directory.Exists(txtBackupPath.Text))
                    dialog.SelectedPath = txtBackupPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.Exists(dialog.SelectedPath))
                    {
                        txtBackupPath.Text = dialog.SelectedPath;
                        LogInfo($"Path set to: {dialog.SelectedPath}");
                    }
                    else
                    {
                        MessageBox.Show("Selected path does not exist.", "Invalid Path",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void btnBrowseRestorePath_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select Backup File to Restore";
                dialog.Filter = "SQL Server Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.Multiselect = false;

                if (!string.IsNullOrWhiteSpace(txtRestorePath.Text))
                {
                    if (File.Exists(txtRestorePath.Text))
                    {
                        dialog.InitialDirectory = Path.GetDirectoryName(txtRestorePath.Text);
                        dialog.FileName = Path.GetFileName(txtRestorePath.Text);
                    }
                    else if (Directory.Exists(txtRestorePath.Text))
                    {
                        dialog.InitialDirectory = txtRestorePath.Text;
                    }
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtRestorePath.Text = dialog.FileName;
                    LogInfo($"Backup file selected: {dialog.FileName}");
                }
            }
        }
        private void btnTabChanged_Click(object sender, EventArgs e)
        {
            var activeTab = tabControl.SelectedTab;

            if (activeTab != null)
            {
                if (activeTab == tabBackupAndRestore)
                {
                    gbx1OperationType.Enabled = false;
                    gbx1OperationType.Visible = false;
                    gbx9OperationType.Enabled = true;
                    gbx9OperationType.Visible = true;
                }
                else
                {
                    gbx9OperationType.Enabled = false;
                    gbx9OperationType.Visible = false;
                    gbx1OperationType.Enabled = true;
                    gbx1OperationType.Visible = true;
                }
            }

            ReloadFrom();
        }
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }
        private void btnShowInformation_Click(object sender, EventArgs e)
        {
            var dataSource = GetInformation(out string sourceLabel, out string sourceTitle);
            if (dataSource != null)
            {
                var form = new DataTableViewer(dataSource, sourceLabel, sourceTitle);
                form.Show();
            }
        }

        #endregion

        #region GroupBoxEvents
        private void gbx1OperationType_Click(object sender, EventArgs e)
        {
            rbCreate.Checked = false;
            rbAlter.Checked = false;
            rbDrop.Checked = false;
            btnExecuteDb.Visible = false;
        }
        private void gbx9OperationType_Click(object sender, EventArgs e)
        {
            rbBackup.Checked = false;
            rbRestore.Checked = false;
            btnExecuteDb.Visible = false;
        }

        #endregion

        #region CheckBoxEvents
        private void cbxIsIdentity_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxIsIdentity.Visible && cbxIsIdentity.Checked)
            {
                numIdentityIncrement.Visible = true;
                numIdentityIncrement.Value = 1;
                lblIdentityIncrement.Visible = true;
                numIdentitySeed.Visible = true;
                numIdentitySeed.Value = 1;
                lblIdentitySeed.Visible = true;
            }
            else
            {
                numIdentityIncrement.Visible = false;
                lblIdentityIncrement.Visible = false;
                numIdentitySeed.Visible = false;
                lblIdentitySeed.Visible = false;
            }
        }
        private void cbxIsPrimaryKey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxIsPrimaryKey.Visible && cbxIsPrimaryKey.Checked)
            {
                txtPkName.Visible = true;
                lblPkName.Visible = true;
                txtPkName.Clear();
            }
            else
            {
                txtPkName.Visible = false;
                lblPkName.Visible = false;
                txtPkName.Clear();
            }
        }

        #endregion

        #region ComboBoxEvents
        private void cmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbServers.SelectedItem == null || _Initializing)
                return;

            _CurrentServer = cmbServers.SelectedItem.ToString()!;
            InitialDbServiceForCurrentServer();
        }
        private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? dbName = null;
            ComboBox? schemaLookup = null, schemaLookup2 = null;
            List<ComboBox?> nonClearingLookupList = new()
            {
                sender as ComboBox
            };

            if (cmbDatabases.Visible)
            {
                nonClearingLookupList.Add(cmbCollations);
            }
            if (cmbDatabases2.Visible)
            {
                if (cmbDatabases2.SelectedItem == null)
                {
                    MessageBox.Show("Please select a database", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dbName = cmbDatabases2.SelectedItem.ToString();
                schemaLookup = cmbSchemas;
                nonClearingLookupList.Add(cmbFileGroups);
            }
            else if (cmbDatabases3.Visible)
            {
                if (cmbDatabases3.SelectedItem == null)
                {
                    MessageBox.Show("Please select a database", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dbName = cmbDatabases3.SelectedItem.ToString();
                schemaLookup = cmbSchemas2;
                nonClearingLookupList.Add(cmbDataTypes);
                nonClearingLookupList.Add(cmbCollations2);
            }
            else if (cmbDatabases4.Visible)
            {
                if (cmbDatabases4.SelectedItem == null)
                {
                    MessageBox.Show("Please select a database", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dbName = cmbDatabases4.SelectedItem.ToString();
                schemaLookup = cmbSchemas3;
                nonClearingLookupList.Add(cmbIndexType);
                nonClearingLookupList.Add(cmbFileGroups2);
            }
            else if (cmbDatabases5.Visible)
            {
                if (cmbDatabases5.SelectedItem == null)
                {
                    MessageBox.Show("Please select a database", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dbName = cmbDatabases5.SelectedItem.ToString();
                schemaLookup = cmbSchemas4;
                schemaLookup2 = cmbSchemas5;
                nonClearingLookupList.Add(cmbOnDelete);
                nonClearingLookupList.Add(cmbOnUpdate);
            }

            ClearAllComboBox(nonClearingLookupList.ToArray());

            if (string.IsNullOrWhiteSpace(dbName) == false)
            {
                LoadSchemas(dbName, schemaLookup, schemaLookup2);
            }
        }
        private void cmbSchemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? dbName = null, schemaName = null, schemaName2 = null;
            ComboBox? tableLookup = null, tableLookup2 = null;

            cmbTables.Items.Clear();
            cmbTables2.Items.Clear();
            cmbTables3.Items.Clear();
            cmbTables4.Items.Clear();
            cmbTables5.Items.Clear();

            if (cmbDatabases2.Visible && cmbSchemas.Visible && sender == cmbSchemas)
            {
                dbName = cmbDatabases2.SelectedItem?.ToString();
                schemaName = cmbSchemas.SelectedItem?.ToString();
                tableLookup = cmbTables;
            }
            else if (cmbDatabases3.Visible && cmbSchemas2.Visible && sender == cmbSchemas2)
            {
                dbName = cmbDatabases3.SelectedItem?.ToString();
                schemaName = cmbSchemas2.SelectedItem?.ToString();
                tableLookup = cmbTables2;
            }
            else if (cmbDatabases4.Visible && cmbSchemas3.Visible && sender == cmbSchemas3)
            {
                dbName = cmbDatabases4.SelectedItem?.ToString();
                schemaName = cmbSchemas3.SelectedItem?.ToString();
                tableLookup = cmbTables3;
            }
            else if (cmbDatabases5.Visible)
            {
                if (cmbSchemas4.Visible && sender == cmbSchemas4)
                {
                    dbName = cmbDatabases5.SelectedItem?.ToString();
                    schemaName = cmbSchemas4.SelectedItem?.ToString();
                    tableLookup = cmbTables4;
                }
                if (cmbSchemas5.Visible && sender == cmbSchemas5)
                {
                    dbName = cmbDatabases5.SelectedItem?.ToString();
                    schemaName2 = cmbSchemas5.SelectedItem?.ToString();
                    tableLookup2 = cmbTables5;
                }
            }

            if (string.IsNullOrWhiteSpace(dbName) == false)
            {
                if (string.IsNullOrWhiteSpace(schemaName) == false)
                    LoadTables(dbName, schemaName, tableLookup, tableLookup2);

                if (string.IsNullOrWhiteSpace(schemaName2) == false)
                    LoadTables(dbName, schemaName2, tableLookup, tableLookup2);
            }
        }
        private void cmbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? dbName = null, schemaName = null, tableName = null;

            if (sender == cmbTables2)
            {
                cmbIndexes.Items.Clear();
                cmbColumns.Items.Clear();

                if (cmbDatabases3.Visible && cmbSchemas2.Visible && cmbTables2.Visible)
                {
                    dbName = cmbDatabases3.SelectedItem?.ToString();
                    schemaName = cmbSchemas2.SelectedItem?.ToString();
                    tableName = cmbTables2.SelectedItem?.ToString();
                }

                if (string.IsNullOrWhiteSpace(dbName) == false &&
                    string.IsNullOrWhiteSpace(schemaName) == false &&
                    string.IsNullOrWhiteSpace(tableName) == false)
                {
                    LoadColumns(dbName, schemaName, tableName, cmbColumns);
                }
            }
            else if (sender == cmbTables3)
            {
                cmbIndexes.Items.Clear();
                cmbIndexes.SelectedIndex = -1;
                cmbIndexes.ResetText();

                if (cmbDatabases4.Visible && cmbSchemas3.Visible && cmbTables3.Visible)
                {
                    dbName = cmbDatabases4.SelectedItem?.ToString();
                    schemaName = cmbSchemas3.SelectedItem?.ToString();
                    tableName = cmbTables3.SelectedItem?.ToString();
                }

                if (string.IsNullOrWhiteSpace(dbName) == false &&
                    string.IsNullOrWhiteSpace(schemaName) == false &&
                    string.IsNullOrWhiteSpace(tableName) == false)
                {
                    LoadIndexNames(dbName, schemaName, tableName, cmbIndexes);
                }
            }
            else if (sender == cmbTables4)
            {
                cmbColumns2.Items.Clear();

                if (cmbDatabases5.Visible && cmbSchemas4.Visible && cmbTables4.Visible)
                {
                    dbName = cmbDatabases5.SelectedItem?.ToString();
                    schemaName = cmbSchemas4.SelectedItem?.ToString();
                    tableName = cmbTables4.SelectedItem?.ToString();
                }

                if (string.IsNullOrWhiteSpace(dbName) == false &&
                    string.IsNullOrWhiteSpace(schemaName) == false &&
                    string.IsNullOrWhiteSpace(tableName) == false)
                {
                    LoadColumns(dbName, schemaName, tableName, cmbColumns2);
                }
            }
            else if (sender == cmbTables5)
            {
                cmbColumns3.Items.Clear();

                if (cmbDatabases5.Visible && cmbSchemas5.Visible && cmbTables5.Visible)
                {
                    dbName = cmbDatabases5.SelectedItem?.ToString();
                    schemaName = cmbSchemas5.SelectedItem?.ToString();
                    tableName = cmbTables5.SelectedItem?.ToString();
                }

                if (string.IsNullOrWhiteSpace(dbName) == false &&
                    string.IsNullOrWhiteSpace(schemaName) == false &&
                    string.IsNullOrWhiteSpace(tableName) == false)
                {
                    LoadColumns(dbName, schemaName, tableName, cmbColumns3);
                }
            }
        }

        #endregion

        #region TabPages
        private void DatabaseOperation_CheckedChanged(object? sender, EventArgs e)
        {
            RadioButton? rb = sender as RadioButton;
            var activeTab = tabControl.SelectedTab;

            btnShowInformation.Enabled = (rb == rbAlter && rbAlter.Checked);
            btnShowInformation.Visible = (rb == rbAlter && rbAlter.Checked);

            if (activeTab != null && rb != null)
            {
                if (activeTab == tabDatabase)
                {
                    SetDatabaseTabBusiness(rb);
                }
                else if (activeTab == tabTable)
                {
                    SetTableTabBusiness(rb);
                }
                else if (activeTab == tabColumn)
                {
                    SetColumnTabBusiness(rb);
                }
                else if (activeTab == tabIndex)
                {
                    SetIndexTabBusiness(rb);
                }
                else if (activeTab == tabForeignKey)
                {
                    SetForeignKeyTabBusiness(rb);
                }
                else if (activeTab == tabBackupAndRestore)
                {
                    SetBackupAndRestoreTabBusiness(rb);
                }
                else
                {
                    ReloadFrom();
                }
            }
            else
            {
                ReloadFrom();
            }
        }

        private void SetDatabaseTabBusiness(RadioButton rb)
        {
            if (!rb.Checked)
            {
                gbx2DatabaseDetails.Visible = false;
                btnExecuteDb.Visible = false;
                return;
            }
            if (gbx2DatabaseDetails.Visible == false)
            {
                gbx2DatabaseDetails.Visible = true;
                btnExecuteDb.Visible = true;
            }

            if (rb == rbCreate)
            {
                SetLabelInitialBusiness(lblDatabases, false);
                SetLabelInitialBusiness(lblDbName, true);
                SetLabelInitialBusiness(lblInitialSize, true);
                SetLabelInitialBusiness(lblFileGrowth, true);
                SetLabelInitialBusiness(lblCollation, true);
                SetLabelInitialBusiness(lblDataFilePath, true);

                SetComboBoxInitialBusiness(cmbDatabases, false);
                SetComboBoxInitialBusiness(cmbCollations, true, (comboBox) => LoadCollations(comboBox));

                SetTextBoxInitialBusiness(txtDbName, true);
                SetTextBoxInitialBusiness(txtDataFilePath, true);

                SetNumericUpDownInitialBusiness(numDbInitialSize, true, 8);
                SetNumericUpDownInitialBusiness(numDbFileGrowth, true, 64);

                SetCheckBoxInitialBusiness(cbxForceDisconnect, false, false, false);

                btnBrowseDataFilePath.Visible = true;
                btnExecuteDb.Text = "Create Database";
                btnExecuteDb.BackColor = Color.DarkGreen;
                btnExecuteDb.ForeColor = Color.Beige;
            }
            else if (rb == rbAlter)
            {
                SetLabelInitialBusiness(lblDatabases, true);
                SetLabelInitialBusiness(lblDbName, false);
                SetLabelInitialBusiness(lblInitialSize, false);
                SetLabelInitialBusiness(lblFileGrowth, false);
                SetLabelInitialBusiness(lblCollation, true);
                SetLabelInitialBusiness(lblDataFilePath, false);

                SetComboBoxInitialBusiness(cmbDatabases, true, (comboBox) => LoadDatabases(comboBox));
                SetComboBoxInitialBusiness(cmbCollations, true, (comboBox) => LoadCollations(comboBox));

                SetTextBoxInitialBusiness(txtDbName, false);
                SetTextBoxInitialBusiness(txtDataFilePath, false);

                SetNumericUpDownInitialBusiness(numDbInitialSize, false);
                SetNumericUpDownInitialBusiness(numDbFileGrowth, false);

                SetCheckBoxInitialBusiness(cbxForceDisconnect, false, false, false);

                btnBrowseDataFilePath.Visible = false;
                btnExecuteDb.Text = "Alter Database";
                btnExecuteDb.BackColor = Color.Transparent;
                btnExecuteDb.ForeColor = Color.Black;
            }
            else if (rb == rbDrop)
            {
                SetLabelInitialBusiness(lblDatabases, true);
                SetLabelInitialBusiness(lblDbName, false);
                SetLabelInitialBusiness(lblInitialSize, false);
                SetLabelInitialBusiness(lblFileGrowth, false);
                SetLabelInitialBusiness(lblCollation, false);
                SetLabelInitialBusiness(lblDataFilePath, false);

                SetComboBoxInitialBusiness(cmbDatabases, true, (comboBox) => LoadDatabases(comboBox));
                SetComboBoxInitialBusiness(cmbCollations, false);

                SetTextBoxInitialBusiness(txtDbName, false);
                SetTextBoxInitialBusiness(txtDataFilePath, false);

                SetNumericUpDownInitialBusiness(numDbInitialSize, false);
                SetNumericUpDownInitialBusiness(numDbFileGrowth, false);

                SetCheckBoxInitialBusiness(cbxForceDisconnect, true, false, true);

                btnBrowseDataFilePath.Visible = false;
                btnExecuteDb.Text = "Drop Database";
                btnExecuteDb.BackColor = Color.DarkRed;
                btnExecuteDb.ForeColor = Color.Beige;
            }
        }
        private void SetTableTabBusiness(RadioButton rb)
        {
            if (!rb.Checked)
            {
                gbx3TableDetails.Visible = false;
                btnExecuteDb.Visible = false;
                return;
            }
            if (gbx3TableDetails.Visible == false)
            {
                gbx3TableDetails.Visible = true;
                btnExecuteDb.Visible = true;
            }

            if (rb == rbCreate)
            {
                SetLabelInitialBusiness(lblDatabases2, true);
                SetLabelInitialBusiness(lblSchemaName, true);
                SetLabelInitialBusiness(lblTableName, true);
                SetLabelInitialBusiness(lblFileGroup, true);
                SetLabelInitialBusiness(lblDescription, true);

                SetComboBoxInitialBusiness(cmbDatabases2, true, (comboBox) => LoadDatabases(comboBox));
                SetComboBoxInitialBusiness(cmbSchemas, false);
                SetComboBoxInitialBusiness(cmbTables, false);
                SetComboBoxInitialBusiness(cmbFileGroups, true, (comboBox) => LoadFileGroups(comboBox));

                SetTextBoxInitialBusiness(txtSchemaName, true);
                SetTextBoxInitialBusiness(txtTableName, true);
                SetTextBoxInitialBusiness(txtDescription, true);

                SetCheckBoxInitialBusiness(cbxForceDropDependent, false, false, false);

                btnExecuteDb.Text = "Create Table";
                btnExecuteDb.BackColor = Color.DarkGreen;
                btnExecuteDb.ForeColor = Color.Beige;
            }
            else if (rb == rbAlter)
            {
                SetLabelInitialBusiness(lblDatabases2, true);
                SetLabelInitialBusiness(lblSchemaName, true);
                SetLabelInitialBusiness(lblTableName, true);
                SetLabelInitialBusiness(lblFileGroup, false);
                SetLabelInitialBusiness(lblDescription, true);

                SetComboBoxInitialBusiness(cmbDatabases2, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                SetComboBoxInitialBusiness(cmbFileGroups, false);

                SetTextBoxInitialBusiness(txtSchemaName, false);
                SetTextBoxInitialBusiness(txtTableName, false);
                SetTextBoxInitialBusiness(txtDescription, true);

                SetCheckBoxInitialBusiness(cbxForceDropDependent, false, false, false);

                btnExecuteDb.Text = "Alter Table";
                btnExecuteDb.BackColor = Color.Transparent;
                btnExecuteDb.ForeColor = Color.Black;
            }
            else if (rb == rbDrop)
            {
                SetLabelInitialBusiness(lblDatabases2, true);
                SetLabelInitialBusiness(lblSchemaName, true);
                SetLabelInitialBusiness(lblTableName, true);
                SetLabelInitialBusiness(lblFileGroup, false);
                SetLabelInitialBusiness(lblDescription, false);

                SetComboBoxInitialBusiness(cmbDatabases2, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                SetComboBoxInitialBusiness(cmbFileGroups, false);

                SetTextBoxInitialBusiness(txtSchemaName, false);
                SetTextBoxInitialBusiness(txtTableName, false);
                SetTextBoxInitialBusiness(txtDescription, false);

                SetCheckBoxInitialBusiness(cbxForceDropDependent, true, false, true);

                btnExecuteDb.Text = "Drop Table";
                btnExecuteDb.BackColor = Color.DarkRed;
                btnExecuteDb.ForeColor = Color.Beige;
            }
        }
        private void SetColumnTabBusiness(RadioButton rb)
        {
            if (!rb.Checked)
            {
                gbx4ColumnDetails.Visible = false;
                btnExecuteDb.Visible = false;
                return;
            }
            if (gbx4ColumnDetails.Visible == false)
            {
                gbx4ColumnDetails.Visible = true;
                btnExecuteDb.Visible = true;
            }

            if (rb == rbCreate)
            {
                SetLabelInitialBusiness(lblDatabases3, true);
                SetLabelInitialBusiness(lblSchemas, true);
                SetLabelInitialBusiness(lblTables, true);
                SetLabelInitialBusiness(lblColumnName, true);
                SetLabelInitialBusiness(lblDataType, true);
                SetLabelInitialBusiness(lblLength, true);
                SetLabelInitialBusiness(lblPrecision, true);
                SetLabelInitialBusiness(lblScale, true);
                SetLabelInitialBusiness(lblDefaultValue, true);
                SetLabelInitialBusiness(lblPkName, false);
                SetLabelInitialBusiness(lblCollation2, true);
                SetLabelInitialBusiness(lblComputedFormula, true);
                SetLabelInitialBusiness(lblDescription2, true);
                SetLabelInitialBusiness(lblIdentityIncrement, false);
                SetLabelInitialBusiness(lblIdentitySeed, false);

                SetComboBoxInitialBusiness(cmbDatabases3, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas2, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables2, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                SetComboBoxInitialBusiness(cmbColumns, false);
                SetComboBoxInitialBusiness(cmbDataTypes, true, (comboBox) => LoadDataTypes(comboBox));
                SetComboBoxInitialBusiness(cmbCollations2, true, (comboBox) => LoadCollations(comboBox));

                SetTextBoxInitialBusiness(txtColumnName, true);
                SetTextBoxInitialBusiness(txtPkName, false);
                SetTextBoxInitialBusiness(txtComputedFormula, true);
                SetTextBoxInitialBusiness(txtDefaultValue, true);
                SetTextBoxInitialBusiness(txtDescription2, true);

                SetNumericUpDownInitialBusiness(numIdentityIncrement, false);
                SetNumericUpDownInitialBusiness(numIdentitySeed, false);
                SetNumericUpDownInitialBusiness(numLength, true);
                SetNumericUpDownInitialBusiness(numPrecision, true);
                SetNumericUpDownInitialBusiness(numScale, true);

                SetCheckBoxInitialBusiness(cbxForceDropDependent2, false, false, false);
                SetCheckBoxInitialBusiness(cbxIsIdentity, true, false, false);
                SetCheckBoxInitialBusiness(cbxIsNullable, true, false, true);
                SetCheckBoxInitialBusiness(cbxIsPersisted, true, false, false);
                SetCheckBoxInitialBusiness(cbxIsPrimaryKey, true, false, false);

                btnExecuteDb.Text = "Create Column";
                btnExecuteDb.BackColor = Color.DarkGreen;
                btnExecuteDb.ForeColor = Color.Beige;
            }
            else if (rb == rbAlter)
            {
                SetLabelInitialBusiness(lblDatabases3, true);
                SetLabelInitialBusiness(lblSchemas, true);
                SetLabelInitialBusiness(lblTables, true);
                SetLabelInitialBusiness(lblColumnName, true);
                SetLabelInitialBusiness(lblDataType, true);
                SetLabelInitialBusiness(lblLength, true);
                SetLabelInitialBusiness(lblPrecision, true);
                SetLabelInitialBusiness(lblScale, true);
                SetLabelInitialBusiness(lblDefaultValue, true);
                SetLabelInitialBusiness(lblPkName, false);
                SetLabelInitialBusiness(lblCollation2, true);
                SetLabelInitialBusiness(lblComputedFormula, true);
                SetLabelInitialBusiness(lblDescription2, true);
                SetLabelInitialBusiness(lblIdentityIncrement, false);
                SetLabelInitialBusiness(lblIdentitySeed, false);

                SetComboBoxInitialBusiness(cmbDatabases3, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas2, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables2, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? tableName = cmbTables2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns, true, (comboBox) => LoadColumns(dbName, schemaName, tableName, comboBox));
                SetComboBoxInitialBusiness(cmbDataTypes, true, (comboBox) => LoadDataTypes(comboBox));
                SetComboBoxInitialBusiness(cmbCollations2, true, (comboBox) => LoadCollations(comboBox));

                SetTextBoxInitialBusiness(txtColumnName, false);
                SetTextBoxInitialBusiness(txtPkName, true);
                SetTextBoxInitialBusiness(txtComputedFormula, true);
                SetTextBoxInitialBusiness(txtDefaultValue, true);
                SetTextBoxInitialBusiness(txtDescription2, true);

                SetNumericUpDownInitialBusiness(numIdentityIncrement, false);
                SetNumericUpDownInitialBusiness(numIdentitySeed, false);
                SetNumericUpDownInitialBusiness(numLength, true);
                SetNumericUpDownInitialBusiness(numPrecision, true);
                SetNumericUpDownInitialBusiness(numScale, true);

                SetCheckBoxInitialBusiness(cbxForceDropDependent2, false, false, false);
                SetCheckBoxInitialBusiness(cbxIsIdentity, true, true, false);
                SetCheckBoxInitialBusiness(cbxIsNullable, true, true, false);
                SetCheckBoxInitialBusiness(cbxIsPersisted, true, true, false);
                SetCheckBoxInitialBusiness(cbxIsPrimaryKey, true, true, false);

                btnExecuteDb.Text = "Alter Column";
                btnExecuteDb.BackColor = Color.Transparent;
                btnExecuteDb.ForeColor = Color.Black;
            }
            else if (rb == rbDrop)
            {
                SetLabelInitialBusiness(lblDatabases3, true);
                SetLabelInitialBusiness(lblSchemas, true);
                SetLabelInitialBusiness(lblTables, true);
                SetLabelInitialBusiness(lblColumnName, true);
                SetLabelInitialBusiness(lblDataType, false);
                SetLabelInitialBusiness(lblLength, false);
                SetLabelInitialBusiness(lblPrecision, false);
                SetLabelInitialBusiness(lblScale, false);
                SetLabelInitialBusiness(lblDefaultValue, false);
                SetLabelInitialBusiness(lblPkName, false);
                SetLabelInitialBusiness(lblCollation2, false);
                SetLabelInitialBusiness(lblComputedFormula, false);
                SetLabelInitialBusiness(lblDescription2, false);
                SetLabelInitialBusiness(lblIdentityIncrement, false);
                SetLabelInitialBusiness(lblIdentitySeed, false);

                SetComboBoxInitialBusiness(cmbDatabases3, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas2, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables2, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? tableName = cmbTables2.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns, true, (comboBox) => LoadColumns(dbName, schemaName, tableName, comboBox));
                SetComboBoxInitialBusiness(cmbDataTypes, false);
                SetComboBoxInitialBusiness(cmbCollations2, false);

                SetTextBoxInitialBusiness(txtColumnName, false);
                SetTextBoxInitialBusiness(txtPkName, false);
                SetTextBoxInitialBusiness(txtComputedFormula, false);
                SetTextBoxInitialBusiness(txtDefaultValue, false);
                SetTextBoxInitialBusiness(txtDescription2, false);

                SetNumericUpDownInitialBusiness(numIdentityIncrement, false);
                SetNumericUpDownInitialBusiness(numIdentitySeed, false);
                SetNumericUpDownInitialBusiness(numLength, false);
                SetNumericUpDownInitialBusiness(numPrecision, false);
                SetNumericUpDownInitialBusiness(numScale, false);

                SetCheckBoxInitialBusiness(cbxForceDropDependent2, true, false, false);
                SetCheckBoxInitialBusiness(cbxIsIdentity, false, false, false);
                SetCheckBoxInitialBusiness(cbxIsNullable, false, false, false);
                SetCheckBoxInitialBusiness(cbxIsPersisted, false, false, false);
                SetCheckBoxInitialBusiness(cbxIsPrimaryKey, false, false, false);

                btnExecuteDb.Text = "Drop Column";
                btnExecuteDb.BackColor = Color.DarkRed;
                btnExecuteDb.ForeColor = Color.Beige;
            }
        }
        private void SetIndexTabBusiness(RadioButton rb)
        {
            if (!rb.Checked)
            {
                gbx5IndexDetails.Visible = false;
                btnExecuteDb.Visible = false;
                return;
            }
            if (gbx5IndexDetails.Visible == false)
            {
                gbx5IndexDetails.Visible = true;
                btnExecuteDb.Visible = true;
            }

            if (rb == rbCreate)
            {
                SetLabelInitialBusiness(lblDatabases4, true);
                SetLabelInitialBusiness(lblSchemas2, true);
                SetLabelInitialBusiness(lblTables2, true);
                SetLabelInitialBusiness(lblColumns, true);
                SetLabelInitialBusiness(lblIncludeColumns, true);
                SetLabelInitialBusiness(lblIndexType, true);
                SetLabelInitialBusiness(lblFillFactor, true);
                SetLabelInitialBusiness(lblFilterPredicate, true);
                SetLabelInitialBusiness(lblFileGroup2, true);
                SetLabelInitialBusiness(lblIndexName, true);
                SetLabelInitialBusiness(lblIndexes, false);

                SetComboBoxInitialBusiness(cmbDatabases4, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas3, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables3, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                SetComboBoxInitialBusiness(cmbIndexes, false);
                SetComboBoxInitialBusiness(cmbIndexType, true, (comboBox) => LoadIndexTypes(comboBox));
                SetComboBoxInitialBusiness(cmbFileGroups2, true, (comboBox) => LoadFileGroups(comboBox));

                SetTextBoxInitialBusiness(txtColumns, true);
                SetTextBoxInitialBusiness(txtIncludeColumns, true);
                SetTextBoxInitialBusiness(txtIndexName, true);
                SetTextBoxInitialBusiness(txtFilterPredicate, true);

                SetNumericUpDownInitialBusiness(numFillFactor, true);

                SetCheckBoxInitialBusiness(cbxIsUnique, true, false, false);
                SetCheckBoxInitialBusiness(cbxDropExisting, false, false, false);
                SetCheckBoxInitialBusiness(cbxAllowPageLocks, true, false, false);
                SetCheckBoxInitialBusiness(cbxAllowRowLocks, true, false, false);
                SetCheckBoxInitialBusiness(cbxPadIndex, true, false, false);
                SetCheckBoxInitialBusiness(cbxIgnoreIfNotExists, false, false, false);

                btnExecuteDb.Text = "Create Index";
                btnExecuteDb.BackColor = Color.DarkGreen;
                btnExecuteDb.ForeColor = Color.Beige;
            }
            else if (rb == rbAlter)
            {
                SetLabelInitialBusiness(lblDatabases4, true);
                SetLabelInitialBusiness(lblSchemas2, true);
                SetLabelInitialBusiness(lblTables2, true);
                SetLabelInitialBusiness(lblColumns, false);
                SetLabelInitialBusiness(lblIncludeColumns, false);
                SetLabelInitialBusiness(lblIndexType, false);
                SetLabelInitialBusiness(lblFillFactor, false);
                SetLabelInitialBusiness(lblFilterPredicate, false);
                SetLabelInitialBusiness(lblFileGroup2, false);
                SetLabelInitialBusiness(lblIndexName, false);
                SetLabelInitialBusiness(lblIndexes, true);

                SetComboBoxInitialBusiness(cmbDatabases4, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas3, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables3, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? tableName = cmbTables3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbIndexes, true, (comboBox) => LoadIndexNames(dbName, schemaName, tableName, comboBox));
                SetComboBoxInitialBusiness(cmbIndexType, false);
                SetComboBoxInitialBusiness(cmbFileGroups2, false);

                SetTextBoxInitialBusiness(txtColumns, false);
                SetTextBoxInitialBusiness(txtIncludeColumns, false);
                SetTextBoxInitialBusiness(txtIndexName, false);
                SetTextBoxInitialBusiness(txtFilterPredicate, false);

                SetNumericUpDownInitialBusiness(numFillFactor, false);

                SetCheckBoxInitialBusiness(cbxIsUnique, true, true, false);
                SetCheckBoxInitialBusiness(cbxDropExisting, true, true, false);
                SetCheckBoxInitialBusiness(cbxAllowPageLocks, true, true, false);
                SetCheckBoxInitialBusiness(cbxAllowRowLocks, true, true, false);
                SetCheckBoxInitialBusiness(cbxPadIndex, true, true, false);
                SetCheckBoxInitialBusiness(cbxIgnoreIfNotExists, false, false, false);

                btnExecuteDb.Text = "Alter Index";
                btnExecuteDb.BackColor = Color.Transparent;
                btnExecuteDb.ForeColor = Color.Black;
            }
            else if (rb == rbDrop)
            {
                SetLabelInitialBusiness(lblDatabases4, true);
                SetLabelInitialBusiness(lblSchemas2, true);
                SetLabelInitialBusiness(lblTables2, true);
                SetLabelInitialBusiness(lblColumns, false);
                SetLabelInitialBusiness(lblIncludeColumns, false);
                SetLabelInitialBusiness(lblIndexType, false);
                SetLabelInitialBusiness(lblFillFactor, false);
                SetLabelInitialBusiness(lblFilterPredicate, false);
                SetLabelInitialBusiness(lblFileGroup2, false);
                SetLabelInitialBusiness(lblIndexName, false);
                SetLabelInitialBusiness(lblIndexes, true);

                SetComboBoxInitialBusiness(cmbDatabases4, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas3, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables3, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? tableName = cmbTables3.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbIndexes, true, (comboBox) => LoadIndexNames(dbName, schemaName, tableName, comboBox));
                SetComboBoxInitialBusiness(cmbIndexType, false);
                SetComboBoxInitialBusiness(cmbFileGroups2, false);

                SetTextBoxInitialBusiness(txtColumns, false);
                SetTextBoxInitialBusiness(txtIncludeColumns, false);
                SetTextBoxInitialBusiness(txtIndexName, false);
                SetTextBoxInitialBusiness(txtFilterPredicate, false);

                SetNumericUpDownInitialBusiness(numFillFactor, false);

                SetCheckBoxInitialBusiness(cbxIsUnique, false, false, false);
                SetCheckBoxInitialBusiness(cbxDropExisting, false, false, false);
                SetCheckBoxInitialBusiness(cbxAllowPageLocks, false, false, false);
                SetCheckBoxInitialBusiness(cbxAllowRowLocks, false, false, false);
                SetCheckBoxInitialBusiness(cbxPadIndex, false, false, false);
                SetCheckBoxInitialBusiness(cbxIgnoreIfNotExists, true, true, true);

                btnExecuteDb.Text = "Drop Index";
                btnExecuteDb.BackColor = Color.DarkRed;
                btnExecuteDb.ForeColor = Color.Beige;
            }
        }
        private void SetForeignKeyTabBusiness(RadioButton rb)
        {
            if (!rb.Checked)
            {
                gbx6ForeignKeyDetails.Visible = false;
                btnExecuteDb.Visible = false;
                return;
            }
            if (gbx6ForeignKeyDetails.Visible == false)
            {
                gbx6ForeignKeyDetails.Visible = true;
                btnExecuteDb.Visible = true;
            }

            if (rb == rbCreate)
            {
                SetLabelInitialBusiness(lblDatabases5, true);
                SetLabelInitialBusiness(lblSchemas3, true);
                SetLabelInitialBusiness(lblSchemas4, true);
                SetLabelInitialBusiness(lblTables3, true);
                SetLabelInitialBusiness(lblTables4, true);
                SetLabelInitialBusiness(lblColumns2, true);
                SetLabelInitialBusiness(lblColumns3, true);
                SetLabelInitialBusiness(lblOnDelete, true);
                SetLabelInitialBusiness(lblOnUpdate, true);
                SetLabelInitialBusiness(lblFkName, false);

                SetComboBoxInitialBusiness(cmbDatabases5, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas4, true, (comboBox) => LoadSchemas(dbName, comboBox));
                SetComboBoxInitialBusiness(cmbSchemas5, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables4, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? schemaName2 = cmbSchemas5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables5, true, (comboBox) => LoadTables(dbName, schemaName2, comboBox));
                string? tableName = cmbTables4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns2, true, (comboBox) => LoadColumns(dbName, schemaName, tableName, comboBox));
                string? tableName2 = cmbTables5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns3, true, (comboBox) => LoadColumns(dbName, schemaName2, tableName2, comboBox));
                SetComboBoxInitialBusiness(cmbOnDelete, true, (comboBox) => LoadReferentialActions(comboBox));
                SetComboBoxInitialBusiness(cmbOnUpdate, true, (comboBox) => LoadReferentialActions(comboBox));

                SetTextBoxInitialBusiness(txtFkName, false);

                SetCheckBoxInitialBusiness(cbxEnabled, true, false, true);
                SetCheckBoxInitialBusiness(cbxIsNotForReplication, true, false, false);

                btnExecuteDb.Text = "Create Foreign Key";
                btnExecuteDb.BackColor = Color.DarkGreen;
                btnExecuteDb.ForeColor = Color.Beige;
            }
            else if (rb == rbAlter)
            {
                SetLabelInitialBusiness(lblDatabases5, true);
                SetLabelInitialBusiness(lblSchemas3, true);
                SetLabelInitialBusiness(lblSchemas4, true);
                SetLabelInitialBusiness(lblTables3, true);
                SetLabelInitialBusiness(lblTables4, true);
                SetLabelInitialBusiness(lblColumns2, true);
                SetLabelInitialBusiness(lblColumns3, true);
                SetLabelInitialBusiness(lblOnDelete, true);
                SetLabelInitialBusiness(lblOnUpdate, true);
                SetLabelInitialBusiness(lblFkName, false);

                SetComboBoxInitialBusiness(cmbDatabases5, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas4, true, (comboBox) => LoadSchemas(dbName, comboBox));
                SetComboBoxInitialBusiness(cmbSchemas5, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables4, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? schemaName2 = cmbSchemas5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables5, true, (comboBox) => LoadTables(dbName, schemaName2, comboBox));
                string? tableName = cmbTables4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns2, true, (comboBox) => LoadColumns(dbName, schemaName, tableName, comboBox));
                string? tableName2 = cmbTables5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns3, true, (comboBox) => LoadColumns(dbName, schemaName2, tableName2, comboBox));
                SetComboBoxInitialBusiness(cmbOnDelete, true, (comboBox) => LoadReferentialActions(comboBox));
                SetComboBoxInitialBusiness(cmbOnUpdate, true, (comboBox) => LoadReferentialActions(comboBox));

                SetTextBoxInitialBusiness(txtFkName, false);

                SetCheckBoxInitialBusiness(cbxEnabled, true, true, true);
                SetCheckBoxInitialBusiness(cbxIsNotForReplication, true, true, false);

                btnExecuteDb.Text = "Alter Foreign Key";
                btnExecuteDb.BackColor = Color.Transparent;
                btnExecuteDb.ForeColor = Color.Black;
            }
            else if (rb == rbDrop)
            {
                SetLabelInitialBusiness(lblDatabases5, true);
                SetLabelInitialBusiness(lblSchemas3, true);
                SetLabelInitialBusiness(lblSchemas4, true);
                SetLabelInitialBusiness(lblTables3, true);
                SetLabelInitialBusiness(lblTables4, true);
                SetLabelInitialBusiness(lblColumns2, true);
                SetLabelInitialBusiness(lblColumns3, true);
                SetLabelInitialBusiness(lblOnDelete, false);
                SetLabelInitialBusiness(lblOnUpdate, false);
                SetLabelInitialBusiness(lblFkName, true);

                SetComboBoxInitialBusiness(cmbDatabases5, true, (comboBox) => LoadDatabases(comboBox));
                string? dbName = cmbDatabases5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbSchemas4, true, (comboBox) => LoadSchemas(dbName, comboBox));
                SetComboBoxInitialBusiness(cmbSchemas5, true, (comboBox) => LoadSchemas(dbName, comboBox));
                string? schemaName = cmbSchemas4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables4, true, (comboBox) => LoadTables(dbName, schemaName, comboBox));
                string? schemaName2 = cmbSchemas5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbTables5, true, (comboBox) => LoadTables(dbName, schemaName2, comboBox));
                string? tableName = cmbTables4.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns2, true, (comboBox) => LoadColumns(dbName, schemaName, tableName, comboBox));
                string? tableName2 = cmbTables5.SelectedItem?.ToString();
                SetComboBoxInitialBusiness(cmbColumns3, true, (comboBox) => LoadColumns(dbName, schemaName2, tableName2, comboBox));
                SetComboBoxInitialBusiness(cmbOnDelete, false);
                SetComboBoxInitialBusiness(cmbOnUpdate, false);

                SetTextBoxInitialBusiness(txtFkName, true);

                SetCheckBoxInitialBusiness(cbxEnabled, false, false, false);
                SetCheckBoxInitialBusiness(cbxIsNotForReplication, false, false, false);

                btnExecuteDb.Text = "Drop Foreign Key";
                btnExecuteDb.BackColor = Color.DarkRed;
                btnExecuteDb.ForeColor = Color.Beige;
            }
        }
        private void SetBackupAndRestoreTabBusiness(RadioButton rb)
        {
            if (!rb.Checked)
            {
                gbx7Backup.Visible = false;
                gbx8Restore.Visible = false;
                btnExecuteDb.Visible = false;
                return;
            }

            btnExecuteDb.Visible = true;
            btnExecuteDb.Text = "Execute";
            btnExecuteDb.BackColor = Color.DarkGreen;
            btnExecuteDb.ForeColor = Color.Beige;
            gbx1OperationType.Visible = false;
            gbx9OperationType.Visible = true;

            if (rb == rbBackup)
            {
                gbx7Backup.Visible = true;

                SetLabelInitialBusiness(lblDatabases6, true);
                SetLabelInitialBusiness(lblDatabases7, false);
                SetLabelInitialBusiness(lblBackupPath, true);
                SetLabelInitialBusiness(lblRestorePath, false);

                SetComboBoxInitialBusiness(cmbDatabases6, true, (comboBox) => LoadDatabases(comboBox));
                SetComboBoxInitialBusiness(cmbDatabases7, false);

                SetTextBoxInitialBusiness(txtBackupPath, true);
                SetTextBoxInitialBusiness(txtRestorePath, false);

                btnBrowseBackupPath.Visible = true;
                btnBrowseRestorePath.Visible = false;
            }
            else if (rb == rbRestore)
            {
                gbx8Restore.Visible = true;

                SetLabelInitialBusiness(lblDatabases6, false);
                SetLabelInitialBusiness(lblDatabases7, true);
                SetLabelInitialBusiness(lblBackupPath, false);
                SetLabelInitialBusiness(lblRestorePath, true);

                SetComboBoxInitialBusiness(cmbDatabases7, true, (comboBox) => LoadDatabases(comboBox));
                SetComboBoxInitialBusiness(cmbDatabases6, false);

                SetTextBoxInitialBusiness(txtBackupPath, false);
                SetTextBoxInitialBusiness(txtRestorePath, true);

                btnBrowseBackupPath.Visible = false;
                btnBrowseRestorePath.Visible = true;
            }
        }

        #endregion

        #endregion

        #region DbServiceMethod
        private void DbServiceValidation(out bool canConnect)
        {
            canConnect = false;
            var config = AppSettings.GetServerConfiguration(_CurrentServer);

            if (config.SavePassword == false &&
                config.UseWindowsAuth == false &&
                string.IsNullOrWhiteSpace(config.Username) == false &&
                string.IsNullOrWhiteSpace(config.Password))
            {
                using (var dlg = new PasswordPromptForm(config.ServerName, config.Alias, config.Username))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        LogInfo($"Server: {_CurrentServer} -- Connection closed by user.");
                    }
                    else
                    {
                        config.Password = dlg.Password;
                        canConnect = true;
                    }
                }
            }
            else if (config.SavePassword == false &&
                     config.UseWindowsAuth == false &&
                     string.IsNullOrWhiteSpace(config.Password))
            {
                canConnect = true;
            }
            else if (config.UseWindowsAuth)
            {
                canConnect = true;
            }

            _DbService = new DbService(config);
        }

        private DataTable? GetInformation(out string sourceLabel, out string sourceTitle)
        {
            sourceLabel = string.Empty;
            sourceTitle = string.Empty;

            var activeTab = tabControl.SelectedTab;
            if (activeTab == null)
            {
                MessageBox.Show("Please select a tab", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (activeTab == tabDatabase)
            {
                var config = new DatabaseProcedure.Information();
                if (cmbDatabases.TryGetRequiredValue<string>(out string dbName, "Database name"))
                {
                    config.DatabaseName = dbName;
                }
                else return null;

                var result = _DbService!.GetDatabaseInformation(config);
                if (result.operationResult.IsSuccess)
                {
                    sourceLabel = "Database :";
                    sourceTitle = dbName;
                    return result.information;
                }
                else
                {
                    LogError($"Error: {result.operationResult.Message}");
                    MessageBox.Show($"Error: {result.operationResult.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else if (activeTab == tabTable)
            {
                var config = new TableProcedure.Information();
                if (cmbDatabases2.TryGetRequiredValue<string>(out string dbName, "Database name"))
                {
                    config.DatabaseName = dbName;
                }
                else return null;
                if (cmbSchemas.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
                {
                    config.SchemaName = schemaName;
                }
                else return null;
                if (cmbTables.TryGetRequiredValue<string>(out string tableName, "Table name"))
                {
                    config.TableName = tableName;
                }
                else return null;

                var result = _DbService!.GetTableInformation(config);
                if (result.operationResult.IsSuccess)
                {
                    sourceLabel = "Table :";
                    sourceTitle = $"{dbName}.{schemaName}.{tableName}";
                    return result.information;
                }
                else
                {
                    LogError($"Error: {result.operationResult.Message}");
                    MessageBox.Show($"Error: {result.operationResult.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else if (activeTab == tabColumn)
            {
                var config = new ColumnProcedure.Information();
                if (cmbDatabases3.TryGetRequiredValue<string>(out string dbName, "Database name"))
                {
                    config.DatabaseName = dbName;
                }
                else return null;
                if (cmbSchemas2.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
                {
                    config.SchemaName = schemaName;
                }
                else return null;
                if (cmbTables2.TryGetRequiredValue<string>(out string tableName, "Table name"))
                {
                    config.TableName = tableName;
                }
                else return null;
                if (cmbColumns.TryGetRequiredValue<string>(out string columnName, "Column name"))
                {
                    config.ColumnName = columnName;
                }
                else return null;

                var result = _DbService!.GetColumnInformation(config);
                if (result.operationResult.IsSuccess)
                {
                    sourceLabel = "Column :";
                    sourceTitle = $"{dbName}.{schemaName}.{tableName}.{columnName}";
                    return result.information;
                }
                else
                {
                    LogError($"Error: {result.operationResult.Message}");
                    MessageBox.Show($"Error: {result.operationResult.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else if (activeTab == tabIndex)
            {
                var config = new IndexProcedure.Information();
                if (cmbDatabases4.TryGetRequiredValue<string>(out string dbName, "Database name"))
                {
                    config.DatabaseName = dbName;
                }
                else return null;
                if (cmbSchemas3.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
                {
                    config.SchemaName = schemaName;
                }
                else return null;
                if (cmbTables3.TryGetRequiredValue<string>(out string tableName, "Table name"))
                {
                    config.TableName = tableName;
                }
                else return null;
                if (cmbIndexes.TryGetRequiredValue<string>(out string indexName, "Index name"))
                {
                    config.IndexName = indexName;
                }
                else return null;

                var result = _DbService!.GetIndexInformation(config);
                if (result.operationResult.IsSuccess)
                {
                    sourceLabel = "Indexes :";
                    sourceTitle = $"{dbName}.{schemaName}.{tableName}";
                    return result.information;
                }
                else
                {
                    LogError($"Error: {result.operationResult.Message}");
                    MessageBox.Show($"Error: {result.operationResult.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else if (activeTab == tabForeignKey)
            {
                var config = new ForeignKeyProcedure.Information();
                if (cmbDatabases5.TryGetRequiredValue<string>(out string dbName, "Database name"))
                {
                    config.DatabaseName = dbName;
                }
                else return null;
                if (cmbSchemas4.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
                {
                    config.SchemaName = schemaName;
                }
                else return null;
                if (cmbTables4.TryGetRequiredValue<string>(out string tableName, "Table name"))
                {
                    config.TableName = tableName;
                }
                else return null;
                if (cmbColumns2.TryGetRequiredValue<string>(out string columnName, "Column name"))
                {
                    config.ColumnName = columnName;
                }
                else return null;

                var result = _DbService!.GetForeignKeyInformation(config);
                if (result.operationResult.IsSuccess)
                {
                    sourceLabel = "Foreign Keys :";
                    sourceTitle = $"{dbName}.{schemaName}.{tableName}.{columnName}";
                    return result.information;
                }
                else
                {
                    LogError($"Error: {result.operationResult.Message}");
                    MessageBox.Show($"Error: {result.operationResult.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void ExecuteCreateDatabase()
        {
            var config = new DatabaseProcedure.Create();
            if (txtDbName.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (txtDataFilePath.TryGetRequiredValue<string>(out string dataFilePath, "DataFile path"))
            {
                config.DataFilePath = dataFilePath;
            }
            else return;
            if (cmbCollations.TryGetValue<string>(out string collation))
            {
                config.Collation = collation;
            }
            config.FileGrowthMB = (int)numDbFileGrowth.Value;
            config.InitialSizeMB = (int)numDbInitialSize.Value;

            LogInfo($"Creating database: {txtDbName.Text}...");

            var result = _DbService!.CreateDatabase(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Database '{dbName}' created successfully");
                MessageBox.Show("Database created successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to create database: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteAlterDatabase()
        {
            var config = new DatabaseProcedure.Alter();
            if (cmbDatabases.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbCollations.TryGetValue<string?>(out string? collation))
            {
                config.Collation = collation;
            }

            LogInfo($"Altering database: {dbName}...");

            var result = _DbService!.AlterDatabase(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Database '{dbName}' altered successfully");
                MessageBox.Show("Database altered successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to alter database: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteDropDatabase()
        {
            var config = new DatabaseProcedure.Drop();
            if (cmbDatabases.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            config.ForceDisconnect = cbxForceDisconnect.Checked;

            var confirm = MessageBox.Show(
                $"Are you sure you want to drop database '{dbName}'?\n\nThis action cannot be undone!",
                "Confirm Drop",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            LogInfo($"Dropping database: {dbName}...");

            var result = _DbService!.DropDatabase(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Database '{dbName}' dropped successfully");
                MessageBox.Show("Database dropped successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to drop database: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteCreateTable()
        {
            var config = new TableProcedure.Create();
            if (cmbDatabases2.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (txtSchemaName.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (txtTableName.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbFileGroups.TryGetValue<string>(out string fileGroup))
            {
                config.FileGroup = fileGroup;
            }
            if (txtDescription.TryGetValue<string>(out string description))
            {
                config.Description = description;
            }

            LogInfo($"Creating table: {tableName}...");

            var result = _DbService!.CreateTable(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Table '{tableName}' created successfully");
                MessageBox.Show("Table created successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to create table: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteAlterTable()
        {
            var config = new TableProcedure.Alter();
            if (cmbDatabases2.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (txtSchemaName.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (txtTableName.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (txtDescription.TryGetValue<string?>(out string? description))
            {
                config.Description = description;
            }

            LogInfo($"Altering table: {tableName}...");

            var result = _DbService!.AlterTable(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Table '{tableName}' altered successfully");
                MessageBox.Show("Table altered successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to altered table: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteDropTable()
        {
            var config = new TableProcedure.Drop();
            if (cmbDatabases2.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (txtSchemaName.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (txtTableName.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            config.ForceDropDependent = cbxForceDropDependent.Checked;

            var confirm = MessageBox.Show(
                $"Are you sure you want to drop table '{tableName}'?\n\nThis action cannot be undone!",
                "Confirm Drop",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            LogInfo($"Dropping table: {dbName}...");

            var result = _DbService!.DropTable(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Table '{tableName}' dropped successfully");
                MessageBox.Show("Table dropped successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to drop table: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteCreateColumn()
        {
            var config = new ColumnProcedure.Create();
            if (cmbDatabases3.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas2.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables2.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (txtColumnName.TryGetRequiredValue<string>(out string columnName, "Column name"))
            {
                config.ColumnName = columnName;
            }
            else return;
            if (cmbDataTypes.TryGetRequiredValue<string>(out string dataType, "DataType"))
            {
                config.DataType = dataType;
            }
            else return;
            if (txtDefaultValue.TryGetValue<string>(out string defaultValue))
            {
                config.DefaultValue = defaultValue;
            }
            if (txtPkName.TryGetValue<string>(out string pkName))
            {
                config.PKName = pkName;
            }
            if (cmbCollations2.TryGetValue<string>(out string collation))
            {
                config.Collation = collation;
            }
            if (txtComputedFormula.TryGetValue<string>(out string computedFormula))
            {
                config.ComputedFormula = computedFormula;
            }
            if (txtDescription2.TryGetValue<string>(out string description))
            {
                config.Description = description;
            }
            config.Length = (int)numLength.Value;
            config.Precision = (int)numPrecision.Value;
            config.Scale = (int)numScale.Value;
            config.IsNullable = cbxIsNullable.Checked;
            config.IsPersisted = cbxIsPersisted.Checked;
            config.IsIdentity = cbxIsIdentity.Checked;
            config.IdentitySeed = (int)numIdentitySeed.Value;
            config.IdentityIncrement = (int)numIdentityIncrement.Value;
            config.IsPrimaryKey = cbxIsPrimaryKey.Checked;

            LogInfo($"Creating column: {columnName}...");

            var result = _DbService!.CreateColumn(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Column '{columnName}' created successfully");
                MessageBox.Show("Column created successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to create column: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteAlterColumn()
        {
            var config = new ColumnProcedure.Alter();
            if (cmbDatabases3.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas2.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables2.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbColumns.TryGetRequiredValue<string>(out string columnName, "Column name"))
            {
                config.ColumnName = columnName;
            }
            else return;
            if (cmbDataTypes.TryGetValue<string?>(out string? dataType))
            {
                config.DataType = dataType;
            }
            if (txtDefaultValue.TryGetValue<string?>(out string? defaultValue))
            {
                config.DefaultValue = defaultValue;
            }
            if (cmbCollations2.TryGetValue<string?>(out string? collation))
            {
                config.Collation = collation;
            }
            if (txtDescription2.TryGetValue<string?>(out string? description))
            {
                config.Description = description;
            }
            if (numLength.TryGetValue<int?>(out int? length))
            {
                config.Length = length;
            }
            if (numPrecision.TryGetValue<int?>(out int? precision))
            {
                config.Precision = precision;
            }
            if (numScale.TryGetValue<int?>(out int? scale))
            {
                config.Scale = scale;
            }
            if (cbxIsNullable.TryGetValue<bool?>(out bool? isNullable))
            {
                config.IsNullable = isNullable;
            }

            LogInfo($"Altering column: {columnName}...");

            var result = _DbService!.AlterColumn(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Table '{tableName}' altered successfully");
                MessageBox.Show("Table altered successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to altered table: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteDropColumn()
        {
            var config = new ColumnProcedure.Drop();
            if (cmbDatabases3.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas2.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables2.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbColumns.TryGetRequiredValue<string>(out string columnName, "Column name"))
            {
                config.ColumnName = columnName;
            }
            else return;
            config.ForceDropDependent = cbxForceDropDependent.Checked;

            var confirm = MessageBox.Show(
                $"Are you sure you want to drop column '{columnName}'?\n\nThis action cannot be undone!",
                "Confirm Drop",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            LogInfo($"Dropping column: {columnName}...");

            var result = _DbService!.DropColumn(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Column '{columnName}' dropped successfully");
                MessageBox.Show("Column dropped successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to drop column: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteCreateIndex()
        {
            var config = new IndexProcedure.Create();
            if (cmbDatabases4.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas3.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables3.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbIndexType.TryGetRequiredValue<string>(out string indexType, "Index Type"))
            {
                config.IndexType = indexType;
            }
            else return;
            if (cmbFileGroups2.TryGetRequiredValue<string>(out string fileGroup, "File Group"))
            {
                config.FileGroup = fileGroup;
            }
            else return;
            if (txtColumns.TryGetRequiredValue<string>(out string columns, "Columns"))
            {
                config.Columns = columns;
            }
            else return;
            if (txtIncludeColumns.TryGetValue<string>(out string includeColumns))
            {
                config.IncludeColumns = includeColumns;
            }
            if (txtIndexName.TryGetValue<string>(out string indexName))
            {
                config.IndexName = indexName;
            }
            if (txtFilterPredicate.TryGetValue<string>(out string filterPredicate))
            {
                config.FilterPredicate = filterPredicate;
            }
            config.IsUnique = cbxIsUnique.Checked;
            config.AllowRowLocks = cbxAllowRowLocks.Checked;
            config.AllowPageLocks = cbxAllowPageLocks.Checked;
            config.PadIndex = cbxPadIndex.Checked;
            config.FillFactor = (short)numFillFactor.Value;

            LogInfo($"Creating index on columns: {columns}...");

            var result = _DbService!.CreateIndex(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Index created successfully");
                MessageBox.Show("Index created successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to create index: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteAlterIndex()
        {
            var config = new IndexProcedure.Alter();
            if (cmbDatabases4.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas3.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables3.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbIndexType.TryGetValue<string>(out string indexType))
            {
                config.IndexType = indexType;
            }
            if (cmbFileGroups2.TryGetValue<string>(out string fileGroup))
            {
                config.FileGroup = fileGroup;
            }
            if (txtColumns.TryGetValue<string>(out string columns))
            {
                config.Columns = columns;
            }
            if (txtIncludeColumns.TryGetValue<string>(out string includeColumns))
            {
                config.IncludeColumns = includeColumns;
            }
            if (txtIndexName.TryGetValue<string>(out string indexName))
            {
                config.IndexName = indexName;
            }
            if (txtFilterPredicate.TryGetValue<string?>(out string? filterPredicate))
            {
                config.FilterPredicate = filterPredicate;
            }
            if (cbxIsUnique.TryGetValue<bool?>(out bool? isUnique))
            {
                config.IsUnique = isUnique;
            }
            if (cbxAllowRowLocks.TryGetValue<bool?>(out bool? allowRowLocks))
            {
                config.AllowRowLocks = allowRowLocks;
            }
            if (cbxAllowPageLocks.TryGetValue<bool?>(out bool? allowPageLocks))
            {
                config.AllowPageLocks = allowPageLocks;
            }
            if (cbxPadIndex.TryGetValue<bool?>(out bool? padIndex))
            {
                config.PadIndex = padIndex;
            }
            if (numFillFactor.TryGetValue<short?>(out short? fillFactor))
            {
                config.FillFactor = fillFactor;
            }

            LogInfo($"Altering index on columns: {columns}...");

            var result = _DbService!.AlterIndex(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Index altered successfully");
                MessageBox.Show("Index altered successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to alter index: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteDropIndex()
        {
            var config = new IndexProcedure.Drop();
            if (cmbDatabases4.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas3.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables3.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbIndexes.TryGetRequiredValue<string>(out string indexName, "Index Name"))
            {
                config.IndexName = indexName;
            }
            else return;
            if (cbxIgnoreIfNotExists.TryGetValue<bool>(out bool IgnoreIfNotExists))
            {
                config.IgnoreIfNotExists = IgnoreIfNotExists;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to drop index '{indexName}'?\n\nThis action cannot be undone!",
                "Confirm Drop",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            LogInfo($"Dropping index: {indexName}...");

            var result = _DbService!.DropIndex(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Index '{indexName}' dropped successfully");
                MessageBox.Show("Index dropped successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to drop index: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteCreateForeignKey()
        {
            var config = new ForeignKeyProcedure.Create();
            if (cmbDatabases5.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas4.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables4.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbColumns2.TryGetRequiredValue<string>(out string columnName, "Column name"))
            {
                config.ColumnName = columnName;
            }
            else return;
            if (cmbSchemas5.TryGetRequiredValue<string>(out string schemaName2, "Ref schema name"))
            {
                config.RefSchemaName = schemaName2;
            }
            else return;
            if (cmbTables5.TryGetRequiredValue<string>(out string tableName2, "Ref table name"))
            {
                config.RefTableName = tableName2;
            }
            else return;
            if (cmbColumns3.TryGetRequiredValue<string>(out string columnName2, "Ref column name"))
            {
                config.RefColumnName = columnName2;
            }
            else return;
            if (cmbOnDelete.TryGetRequiredValue<string>(out string onDelete, "On delete"))
            {
                config.OnDelete = onDelete;
            }
            else return;
            if (cmbOnUpdate.TryGetRequiredValue<string>(out string onUpdate, "On update"))
            {
                config.OnUpdate = onUpdate;
            }
            else return;
            if (cbxEnabled.TryGetValue<bool>(out bool enabled))
            {
                config.Enabled = enabled;
            }
            if (cbxIsNotForReplication.TryGetValue<bool>(out bool isNotForReplication))
            {
                config.IsNotForReplication = isNotForReplication;
            }

            LogInfo($"Creating foreign key on: {schemaName}.{tableName}.{columnName}...");

            var result = _DbService!.CreateForeignKey(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Foreign Key created successfully");
                MessageBox.Show("Foreign Key created successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to create foreign key: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteAlterForeignKey()
        {
            var config = new ForeignKeyProcedure.Alter();
            if (cmbDatabases5.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas4.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables4.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbColumns2.TryGetRequiredValue<string>(out string columnName, "Column name"))
            {
                config.ColumnName = columnName;
            }
            else return;
            if (cmbSchemas5.TryGetRequiredValue<string>(out string schemaName2, "Ref schema name"))
            {
                config.RefSchemaName = schemaName2;
            }
            else return;
            if (cmbTables5.TryGetRequiredValue<string>(out string tableName2, "Ref table name"))
            {
                config.RefTableName = tableName2;
            }
            else return;
            if (cmbColumns3.TryGetRequiredValue<string>(out string columnName2, "Ref column name"))
            {
                config.RefColumnName = columnName2;
            }
            else return;
            if (cmbOnDelete.TryGetValue<string?>(out string? onDelete))
            {
                config.OnDelete = onDelete;
            }
            if (cmbOnUpdate.TryGetValue<string?>(out string? onUpdate))
            {
                config.OnUpdate = onUpdate;
            }
            if (cbxEnabled.TryGetValue<bool?>(out bool? enable))
            {
                config.Enabled = enable;
            }
            if (cbxIsNotForReplication.TryGetValue<bool?>(out bool? isNotForReplication))
            {
                config.IsNotForReplication = isNotForReplication;
            }

            LogInfo($"Altering foreign key on: {schemaName}.{tableName}.{columnName}...");

            var result = _DbService!.AlterForeignKey(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Foreign Key altered successfully");
                MessageBox.Show("Foreign Key altered successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to alter foreign key: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteDropForeignKey()
        {
            var config = new ForeignKeyProcedure.Drop();
            if (cmbDatabases5.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (cmbSchemas4.TryGetRequiredValue<string>(out string schemaName, "Schema name"))
            {
                config.SchemaName = schemaName;
            }
            else return;
            if (cmbTables4.TryGetRequiredValue<string>(out string tableName, "Table name"))
            {
                config.TableName = tableName;
            }
            else return;
            if (cmbColumns2.TryGetRequiredValue<string>(out string columnName, "Column name"))
            {
                config.ColumnName = columnName;
            }
            else return;
            if (cmbSchemas5.TryGetRequiredValue<string>(out string schemaName2, "Ref schema name"))
            {
                config.RefSchemaName = schemaName2;
            }
            else return;
            if (cmbTables5.TryGetRequiredValue<string>(out string tableName2, "Ref table name"))
            {
                config.RefTableName = tableName2;
            }
            else return;
            if (cmbColumns3.TryGetRequiredValue<string>(out string columnName2, "Ref column name"))
            {
                config.RefColumnName = columnName2;
            }
            else return;
            if (txtFkName.TryGetValue<string>(out string fkName))
            {
                config.FKName = fkName;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to drop this foreign key?\n\nThis action cannot be undone!",
                "Confirm Drop",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            LogInfo("Dropping foreign key ...");

            var result = _DbService!.DropForeignKey(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Foreign Key dropped successfully");
                MessageBox.Show("Foreign Key dropped successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to drop foreign key: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteBackup()
        {
            var config = new BackupProcedure();
            if (cmbDatabases6.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (txtBackupPath.TryGetRequiredValue<string>(out string backupPath, "Backup path"))
            {
                config.BackupPath = backupPath;
            }
            else return;

            LogInfo($"Save Backup on: {backupPath}...");

            var result = _DbService!.BackupDatabase(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Backup created successfully");
                MessageBox.Show("Backup created successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to create backup: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExecuteRestore()
        {
            var config = new RestoreProcedure();
            if (cmbDatabases7.TryGetRequiredValue<string>(out string dbName, "Database name"))
            {
                config.DatabaseName = dbName;
            }
            else return;
            if (txtRestorePath.TryGetRequiredValue<string>(out string restorePath, "Restore path"))
            {
                config.RestorePath = restorePath;
            }
            else return;

            LogInfo($"Restore database: {dbName}...");

            var result = _DbService!.RestoreDatabase(config);

            if (result.IsSuccess)
            {
                LogSuccess($"Restore successfully");
                MessageBox.Show("Restore successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ReloadFrom();
            }
            else
            {
                LogError($"Failed to restore: {result.Message}");
                MessageBox.Show($"Error: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Log
        private void LogInfo(string message)
        {
            rtbLog.SelectionColor = Color.LightBlue;
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] INFO: {message}\n");
            rtbLog.ScrollToCaret();
        }
        private void LogSuccess(string message)
        {
            rtbLog.SelectionColor = Color.LightGreen;
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] SUCCESS: {message}\n");
            rtbLog.ScrollToCaret();
        }
        private void LogError(string message)
        {
            rtbLog.SelectionColor = Color.IndianRed;
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] ERROR: {message}\n");
            rtbLog.ScrollToCaret();
        }

        #endregion

        #region Helper
        private void SetComboBoxInitialBusiness(ComboBox comboBox, bool isVisible, Action<ComboBox>? Initialize = null)
        {
            if (comboBox != null)
            {
                comboBox.Visible = isVisible;
                comboBox.Items.Clear();
                comboBox.SelectedIndex = -1;
                comboBox.ResetText();
                if (Initialize != null && isVisible)
                    Initialize(comboBox);
            }
        }
        private void SetTextBoxInitialBusiness(TextBox textBox, bool isVisible)
        {
            if (textBox != null)
            {
                textBox.Visible = isVisible;
                textBox.Clear();
            }
        }
        private void SetLabelInitialBusiness(Label label, bool isVisible)
        {
            if (label != null)
            {
                label.Visible = isVisible;
            }
        }
        private void SetNumericUpDownInitialBusiness(NumericUpDown numericUpDown, bool isVisible, int? initialValue = null)
        {
            if (numericUpDown != null)
            {
                numericUpDown.Visible = isVisible;
                if (initialValue.HasValue)
                {
                    numericUpDown.Value = initialValue.Value;
                }
                else
                {
                    numericUpDown.Value = numericUpDown.Minimum;
                }
            }
        }
        private void SetCheckBoxInitialBusiness(CheckBox checkBox, bool isVisible, bool isThreeState, bool isChecked)
        {
            if (checkBox != null)
            {
                checkBox.Visible = isVisible;
                checkBox.ThreeState = isThreeState;

                if (isChecked)
                {
                    checkBox.CheckState = CheckState.Checked;
                }
                else
                {
                    if (isThreeState)
                    {
                        checkBox.CheckState = CheckState.Indeterminate;
                    }
                    else
                    {
                        checkBox.CheckState = CheckState.Unchecked;
                    }
                }
            }
        }
        private void ReloadFrom()
        {
            rbCreate.Checked = false;
            rbAlter.Checked = false;
            rbDrop.Checked = false;
            rbRestore.Checked = false;
            rbBackup.Checked = false;
            btnExecuteDb.Visible = false;
            gbx2DatabaseDetails.Visible = false;
            gbx2DatabaseDetails.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            gbx3TableDetails.Visible = false;
            gbx3TableDetails.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            gbx4ColumnDetails.Visible = false;
            gbx4ColumnDetails.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            gbx5IndexDetails.Visible = false;
            gbx5IndexDetails.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            gbx6ForeignKeyDetails.Visible = false;
            gbx6ForeignKeyDetails.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            gbx7Backup.Visible = false;
            gbx7Backup.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            gbx8Restore.Visible = false;
            gbx8Restore.Controls.Cast<Control>().ToList().ForEach(c => c.Visible = false);
            ClearAllComboBox();
        }
        private void ClearAllComboBox(params ComboBox?[] exceptComboBoxes)
        {
            List<ComboBox?> exceptComboBoxeList = new()
            {
                cmbServers
            };

            if (exceptComboBoxes != null)
            {
                exceptComboBoxeList.AddRange(exceptComboBoxes.ToList());
            }

            ApplyToAllComboBoxes(this, cmb =>
            {
                if (exceptComboBoxeList.Contains(cmb) == false)
                {
                    cmb.Items.Clear();
                    cmb.SelectedIndex = -1;
                    cmb.ResetText();
                }
            });
        }
        private void ApplyToAllComboBoxes(Control parent, Action<ComboBox> action)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is ComboBox comboBox)
                {
                    action(comboBox);
                }

                if (ctrl.HasChildren)
                {
                    ApplyToAllComboBoxes(ctrl, action);
                }
            }
        }

        #endregion

        #endregion

    }
}
