using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;

namespace PestControll_CRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string server_name = "";
        
        public string GetConnectionString (string server_name, string login, string password)
        {
            return $"{server_name}{login}{password}";
        }

    }
}
