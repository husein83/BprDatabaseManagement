using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DatabaseManagement.UI
{
    public partial class PasswordPromptForm : Form
    {
        public string Password => txtPassword.Text;

        public PasswordPromptForm(string serverName, string? serverAlias, string username)
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(serverAlias))
            {
                txtServerName.Text = serverName;
            }
            else
            {
                txtServerName.Text = $"{serverAlias} ({serverName})";
            }
            txtUsername.Text = username;
            txtPassword.PasswordChar = '●';
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter password", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

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
    }
}
