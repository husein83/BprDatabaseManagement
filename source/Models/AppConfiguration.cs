using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace DatabaseManagement.Models
{
    public class AppConfiguration
    {
        public string DefaultServer { get; set; } = string.Empty;
        public Dictionary<string, ServerConfiguration> Servers { get; set; } = new();
    }

    public class ServerConfiguration
    {
        public string ServerName { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public bool UseWindowsAuth { get; set; } = true;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool SavePassword { get; set; }
        public int ConnectionTimeout { get; set; } = 30;
        public int CommandTimeout { get; set; } = 300;
        public bool AutoCreateStoredProcedures { get; set; }

        public string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = string.IsNullOrWhiteSpace(ServerName) ? "." : ServerName,
                ConnectTimeout = ConnectionTimeout,
                CommandTimeout = CommandTimeout,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true,
            };

            if (UseWindowsAuth)
                builder.IntegratedSecurity = true;
            else
            {
                if (string.IsNullOrEmpty(Username))
                    throw new InvalidOperationException("Username is required for SQL Server authentication.");

                builder.UserID = Username;
                builder.Password = Password;
            }

            return builder.ConnectionString;
        }
    }
}
