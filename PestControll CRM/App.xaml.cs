using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Newtonsoft.Json;

namespace PestControll_CRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string appsettings_path = "./appsettings.json";

        public static string server {  get; set; }
        public static string database {  get; set; }

        private class Connection
        {
            public string server {  get; set; }
            public string database { get; set; }
        }

        public App()
        {
            using (StreamReader r = new StreamReader(appsettings_path))
            {
                string json = r.ReadToEnd();
                Connection conn = JsonConvert.DeserializeObject<Connection>(json);

                if (conn != null)
                {
                    server = conn.server;
                    database = conn.database;
                }
                else MessageBox.Show("Connection settings error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
