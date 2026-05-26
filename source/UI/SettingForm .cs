using DatabaseManagement.Business;
using DatabaseManagement.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatabaseManagement.UI
{
    public partial class SettingForm : Form
    {
        #region Variables
        private readonly AppConfiguration _config;
        private bool _isModified = false;
        private bool _suppressEvents = false;
        private int _lastSelectedIndex = -1;

        #endregion

        #region Constructor
        public SettingForm(AppConfiguration config)
        {
            InitializeComponent();
            InitialCustom();
            _config = config;
        }

        private void InitialCustom()
        {
            txtPassword.PasswordChar = '●';

        }

        #endregion

        #region ElementEvents

        #region Load
        private void SettingForm_Load(object sender, EventArgs e) => LoadServerList();

        #endregion

        #region ButtonEvents
        private void rbAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            UpdateAuthControls();
        }
        private void btnAddServer_Click(object sender, EventArgs e)
        {
            string key = "NewServer";
            int i = 1;
            while (_config.Servers.ContainsKey(key)) key = $"NewServer{i++}";

            _config.Servers[key] = new ServerConfiguration
            {
                ServerName = key,
                ConnectionTimeout = 30,
                Alias = string.Empty,
                AutoCreateStoredProcedures = true,
                CommandTimeout = 300,
                Password = string.Empty,
                SavePassword = false,
                Username = string.Empty,
                UseWindowsAuth = true
            };
            _isModified = true;
            LoadServerList(key);
        }
        private void btnRemoveServer_Click(object sender, EventArgs e)
        {
            var key = GetSelectedKey();
            if (key == null) return;

            if (MessageBox.Show($"Remove '{key}'?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            _config.Servers.Remove(key);
            if (_config.DefaultServer == key)
                _config.DefaultServer = _config.Servers.Keys.FirstOrDefault() ?? "";

            _isModified = true;
            LoadServerList();
        }
        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            var key = GetSelectedKey();
            if (key == null) return;
            _config.DefaultServer = key;
            _isModified = true;
            LoadServerList(key);
        }
        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            SaveCurrentDetails();
            var key = GetSelectedKey();
            if (key == null || !_config.Servers.TryGetValue(key, out var s)) return;

            btnTestConnection.Enabled = false;
            try
            {
                using var conn = new SqlConnection(s.BuildConnectionString());
                await conn.OpenAsync();
                MessageBox.Show("Connection successful!", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed:\n{ex.Message}", "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { btnTestConnection.Enabled = true; }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentDetails();
            _isModified = false;
            DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isModified &&
                MessageBox.Show("Unsaved changes. Cancel anyway?", "Warning",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion

        #region ListEvents
        private void lstServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressEvents) return;

            if (_lastSelectedIndex >= 0 && _lastSelectedIndex < lstServers.Items.Count)
            {
                var oldKey = GetKeyAtIndex(_lastSelectedIndex);
                if (oldKey != null && _config.Servers.TryGetValue(oldKey, out var oldServer))
                {
                    SaveDetailsToServer(oldServer);
                }
            }

            _lastSelectedIndex = lstServers.SelectedIndex;

            var key = GetSelectedKey();
            if (key == null || !_config.Servers.TryGetValue(key, out var s))
            {
                gbxServerDetails.Enabled = false;
                btnSetDefault.Enabled = false;
                btnRemoveServer.Enabled = false;
                return;
            }

            gbxServerDetails.Enabled = true;
            PopulateDetails(s);
            btnSetDefault.Enabled = key != _config.DefaultServer;
            btnRemoveServer.Enabled = true;
        }

        #endregion

        #region TextBoxEvents
        private void txtServerName_Leave(object sender, EventArgs e) => SaveCurrentDetails();
        private void txtServerAlias_Leave(object sender, EventArgs e) => SaveCurrentDetails();

        #endregion

        #region CheckBoxEvents
        private void cbxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (txtPassword.Enabled && txtPassword.Visible && cbxShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '●';
            }
        }

        #endregion

        #endregion

        #region Private
        private string GetDisplayKey(string alias, string serverName) => !string.IsNullOrWhiteSpace(alias) ? alias : serverName;
        private string? GetSelectedKey()
        {
            if (lstServers.SelectedItem is not string item) return null;
            return item.Replace(" [Default]", "").Trim();
        }
        private string? GetKeyAtIndex(int index)
        {
            if (index < 0 || index >= lstServers.Items.Count) return null;
            var item = lstServers.Items[index].ToString();
            return item?.Replace(" [Default]", "").Trim();
        }
        private void SaveDetailsToServer(ServerConfiguration server)
        {
            server.ServerName = txtServerName.Text.Trim();
            server.Alias = txtServerAlias.Text.Trim();
            server.UseWindowsAuth = rbWindowsAuth.Checked;
            server.Username = txtUsername.Text.Trim();
            server.Password = cbxSavePassword.Checked ? txtPassword.Text : "";
            server.SavePassword = cbxSavePassword.Checked;
            server.ConnectionTimeout = (int)numConnectionTimeout.Value;
            server.CommandTimeout = (int)numCommandTimeout.Value;
            server.AutoCreateStoredProcedures = cbxAutoCreateStoredProcedures.Checked;

            _isModified = true;
        }
        private void LoadServerList(string? selectKey = null)
        {
            _suppressEvents = true;
            lstServers.Items.Clear();
            foreach (var key in _config.Servers.Keys)
            {
                string display = key == _config.DefaultServer ? $"{key} [Default]" : key;
                lstServers.Items.Add(display);
            }

            string target = selectKey ?? _config.DefaultServer;
            for (int i = 0; i < lstServers.Items.Count; i++)
            {
                if (lstServers.Items[i].ToString()!.StartsWith(target))
                {
                    lstServers.SelectedIndex = i;
                    break;
                }
            }
            if (lstServers.SelectedIndex < 0 && lstServers.Items.Count > 0)
                lstServers.SelectedIndex = 0;

            _suppressEvents = false;
            lstServers_SelectedIndexChanged(null!, null!);
        }
        private void PopulateDetails(ServerConfiguration s)
        {
            _suppressEvents = true;
            txtServerName.Text = s.ServerName;
            txtServerAlias.Text = s.Alias;
            rbWindowsAuth.Checked = s.UseWindowsAuth;
            rbSqlAuth.Checked = !s.UseWindowsAuth;
            txtUsername.Text = s.Username;
            txtPassword.Text = s.SavePassword ? s.Password : "";
            cbxSavePassword.Checked = s.SavePassword;
            cbxAutoCreateStoredProcedures.Checked = s.AutoCreateStoredProcedures;
            numConnectionTimeout.Value = s.ConnectionTimeout > 0 ? s.ConnectionTimeout : 30;
            numCommandTimeout.Value = s.CommandTimeout > 0 ? s.CommandTimeout : 300;

            UpdateAuthControls();
            _suppressEvents = false;
        }
        private void UpdateAuthControls()
        {
            bool sqlAuth = rbSqlAuth.Checked;
            txtUsername.Enabled = sqlAuth;
            txtPassword.Enabled = sqlAuth;
            cbxShowPassword.Enabled = sqlAuth;
            cbxSavePassword.Enabled = sqlAuth;
        }
        private void SaveCurrentDetails()
        {
            if (_suppressEvents) return;
            var key = GetSelectedKey();
            if (key == null || !_config.Servers.TryGetValue(key, out var s)) return;

            SaveDetailsToServer(s);

            string newKey = GetDisplayKey(s.Alias, s.ServerName);
            if (!string.IsNullOrWhiteSpace(newKey) && newKey != key)
            {
                _config.Servers.Remove(key);
                _config.Servers[newKey] = s;
                if (_config.DefaultServer == key) _config.DefaultServer = newKey;
                _isModified = true;
                LoadServerList(newKey);
            }
        }

        #endregion

    }
}
