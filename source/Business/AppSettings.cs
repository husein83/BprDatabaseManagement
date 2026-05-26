using DatabaseManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DatabaseManagement.Business
{
    public static class AppSettings
    {
        private static AppConfiguration? _AppConfiguration;
        private static readonly string ConfigFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "BprDbManagementSetting.json"
        );

        public static AppConfiguration LoadSettings()
        {
            if (_AppConfiguration != null)
                return _AppConfiguration;

            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    CreateDefaultSettings();
                }

                string json = File.ReadAllText(ConfigFilePath);
                _AppConfiguration = JsonSerializer.Deserialize<AppConfiguration>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return _AppConfiguration!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load configuration: {ex.Message}", ex);
            }
        }

        public static void SaveSettings(AppConfiguration settings)
        {
            try
            {
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(ConfigFilePath, json);
                _AppConfiguration = settings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save configuration: {ex.Message}", ex);
            }
        }

        public static ServerConfiguration GetServerConfiguration(string? serverName = null)
        {
            var settings = LoadSettings();

            if (string.IsNullOrWhiteSpace(serverName))
                serverName = settings.DefaultServer;

            if (settings.Servers.ContainsKey(serverName))
                return settings.Servers[serverName];

            throw new Exception($"Server configuration for '{serverName}' not found");
        }

        public static string GetConnectionString(string? serverName = null)
        {
            return GetServerConfiguration(serverName).BuildConnectionString();
        }

        public static List<string> GetAllServerNames()
        {
            var settings = LoadSettings();
            return settings.Servers.Keys.ToList();
        }

        public static void AddOrUpdateServer(string serverName, ServerConfiguration config)
        {
            var settings = LoadSettings();

            if (settings.Servers.ContainsKey(serverName))
                settings.Servers[serverName] = config;
            else
                settings.Servers.Add(serverName, config);

            SaveSettings(settings);
        }

        public static void RemoveServer(string serverName)
        {
            var settings = LoadSettings();

            if (settings.Servers.ContainsKey(serverName))
            {
                if (settings.DefaultServer == serverName)
                    throw new Exception("Cannot remove default server. Please set another server as default first.");

                settings.Servers.Remove(serverName);
                SaveSettings(settings);
            }
        }

        public static void SetDefaultServer(string serverName)
        {
            var settings = LoadSettings();

            if (!settings.Servers.ContainsKey(serverName))
                throw new Exception($"Server '{serverName}' does not exist");

            settings.DefaultServer = serverName;
            SaveSettings(settings);
        }

        public static string GetDefaultServerName()
        {
            return LoadSettings().DefaultServer;
        }

        private static void CreateDefaultSettings()
        {
            var defaultSettings = new AppConfiguration
            {
                DefaultServer = "LocalServer",
                Servers = new Dictionary<string, ServerConfiguration>
                {
                    {
                        "LocalServer",
                        new ServerConfiguration
                        {
                            ServerName = ".",
                            Alias = "LocalServer",
                            UseWindowsAuth = true,
                            ConnectionTimeout = 30,
                            CommandTimeout = 300,
                            AutoCreateStoredProcedures = true
                        }
                    }
                }
            };

            SaveSettings(defaultSettings);
        }

        public static void ReloadSettings()
        {
            _AppConfiguration = null;
            LoadSettings();
        }
    }
}
