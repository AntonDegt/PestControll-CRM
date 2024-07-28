using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PestControll_CRM.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region DataContext + Collections
        private DataContext data;
        public ObservableCollection<Contact> contacts { get; set; }
        public ObservableCollection<ContactStatus> contactStatuses { get; set; }
        #endregion

        public MainWindow(DataContext data)
        {
            this.DataContext = this;
            this.data = data;

            if (data != null)
            {
                contacts = new ObservableCollection<Contact>(data.Contacts.ToList());
                contactStatuses = new ObservableCollection<ContactStatus>(data.ContactStatuses.ToList());
                contactStatuses.Insert(0, new ContactStatus() { Id = -1, StatusName = "" });
                foreach (Contact contact in contacts) 
                {
                    contact.PhoneNumbers = new ObservableCollection<PhoneNumber>(
                            data.PhoneNumbers.Where(num => num.Contact == contact)
                        );
                }
            }
            InitializeComponent();
        }

        #region Search/Filter Contacts

        private bool FilteredContact(Contact contact)
        {
            bool flag = true;
            if (ContactSearchTextBox.Text.Length > 0)
                if (!contact.PIB.Contains(ContactSearchTextBox.Text))
                    flag = false;

            ContactStatus? status = ContactStatusComboBox.SelectedValue as ContactStatus;
            if (status != null)
                if (status.Id != -1)
                    if (contact.Status != status)
                        flag = false;

            return flag;
        }

        private void SearchContacts()
        {
            contacts = new ObservableCollection<Contact>(data.Contacts.ToList().Where(c => FilteredContact(c)));
            ContactsListBox.ItemsSource = contacts;
        }
        private void ContactStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchContacts();
        }

        private void ContactSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchContacts();
            if (ContactSearchTextBox.Text == "")
                ClearSearch.Visibility = Visibility.Hidden;
            else
                ClearSearch.Visibility = Visibility.Visible;
        }

        private void ContactSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchContacts();
        }
        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            ContactSearchTextBox.Text = "";
        }
        #endregion

        #region Contacts Crud Buttons
        private void ContactsListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Contact? contact = ContactsListBox.SelectedItem as Contact;
            if (contact != null)
            {

            }
        }
        #endregion
    }
}
