using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;


namespace PestControll_CRM.Windows
{
    /// <summary>
    /// Логіка вікна доступу до Бази Даних
    /// </summary>
    public partial class Login : Window
    {
        private DataContext data;

        public Login()
        {
            InitializeComponent();
            data = new DataContext();
            data.EnterParams(loginBox, passBox);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!data.isConnect)
            {
                MessageBox.Show(
                    "Немає відповіді, перевірте налаштування підключення, логін або пароль",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            new MainWindow(data).Show();
            this.Close();
        }
    }
}
