using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using PestControll_CRM.Windows.CRUD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
                foreach (Contact contact in contacts) 
                {
                    contact.PhoneNumbers = new ObservableCollection<PhoneNumber>(
                            data.PhoneNumbers.Where(num => num.Contact == contact)
                        );
                }
            }

            InitializeComponent();
            for (int i = 0; i < StatusesListBox.Items.Count; i++)
            {
                ListBoxItem item = StatusesListBox.Items[i] as ListBoxItem;
                if (item == null) continue;
                if (i % 2 == 0)
                {
                    item.Background = this.FindResource("pc_white") as SolidColorBrush;
                }
                else
                {
                    item.Background = this.FindResource("pc_gray") as SolidColorBrush;
                }
            }
        }

        #region Search/Filter Contacts

        private bool FilteredContact(Contact contact)
        {
            bool flag = true;
            if (ContactSearchTextBox.Text.Length > 0)
                if (!contact.PIB.ToLower().Contains(ContactSearchTextBox.Text.ToLower()))
                    flag = false;

            ContactStatus? status = ContactStatusComboBox.SelectedValue as ContactStatus;
            if (status != null)
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
            if (e.Key == System.Windows.Input.Key.Enter)
                SearchContacts();
        }
        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            ContactSearchTextBox.Text = "";
        }
        private void ClearSelectedStatusButton_Click(object sender, RoutedEventArgs e)
        {
            ContactStatusComboBox.SelectedItem = null;
        }
        #endregion

        #region Contacts CRUD Buttons
        private void UpdateContactsListBox()
        {
            contacts = new ObservableCollection<Contact>(data.Contacts.ToList().Where(c => FilteredContact(c)));
            ContactsListBox.ItemsSource = contacts;
        }

        private void ContactsListBoxItem_MouseDoubleClick(object sender, EventArgs e)
        {
            Contact? contact = ContactsListBox.SelectedItem as Contact;
            if (contact == null) return;
            
            new ContactCRUD(contact, ContactCRUD.ContactAction.Read, data, UpdateContactsListBox).Show();
        }
        private void CreateContactButton_Click(object sender, RoutedEventArgs e)
        {
            new ContactCRUD(new Contact(), ContactCRUD.ContactAction.Create, data, UpdateContactsListBox).Show();
        }

        private void EditContactButton_Click(object sender, RoutedEventArgs e)
        {
            Contact? contact = ContactsListBox.SelectedItem as Contact;
            if (contact == null) return;

            new ContactCRUD(contact, ContactCRUD.ContactAction.Update, data, UpdateContactsListBox).Show();
        }

        private void DeleteContactButton_Click(object sender, RoutedEventArgs e)
        {
            Contact? contact = ContactsListBox.SelectedItem as Contact;
            if (contact == null) return;

            MessageBoxResult result = MessageBox.Show("Ви дійсно хочете видалити Контакт?", "Видалення!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                foreach (PhoneNumber number in data.PhoneNumbers.Where(num => num.Contact == contact).ToList())
                    data.PhoneNumbers.Remove(number);
                data.Contacts.Remove(contact);
                data.SaveChanges();
                UpdateContactsListBox();
            }
        }
        #endregion

        #region Statuses CRUD Buttons
        private void UpdateStatusesListBox()
        {
            contactStatuses = new ObservableCollection<ContactStatus>(data.ContactStatuses.ToList());
            StatusesListBox.ItemsSource = contactStatuses;
        }
        private void StatusesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusesListBox.SelectedItem == null)
            {
                EditStatusButton.IsEnabled = false;
                DeleteStatusButton.IsEnabled = false;
            }
            else
            {
                EditStatusButton.IsEnabled = true;
                DeleteStatusButton.IsEnabled = true;
            }
        }
        private void AddStatusButton_Click(object sender, RoutedEventArgs e)
        {
            new StatusCRUD(new ContactStatus(), StatusCRUD.ActionStatus.Create, data, UpdateStatusesListBox).Show();
        }

        private void EditStatusButton_Click(object sender, RoutedEventArgs e)
        {
            ContactStatus status = StatusesListBox.SelectedItem as ContactStatus;
            if (status == null)
                return;

            new StatusCRUD(status, StatusCRUD.ActionStatus.Update, data, UpdateStatusesListBox).Show();
        }

        private void DeleteStatusButton_Click(object sender, RoutedEventArgs e)
        {
            ContactStatus status = StatusesListBox.SelectedItem as ContactStatus;
            if (status == null)
                return;

            List<Contact> contacts_with_this_status = data.Contacts.Where(c => c.Status == status).ToList();
            if (contacts_with_this_status.Count > 0)
            {
                string str = contacts_with_this_status[0].PIB;
                for (int i = 1; i < contacts_with_this_status.Count; i++)
                {
                    str += ", " + contacts_with_this_status[i].PIB;
                    if (i == 2) break;
                }
                if (contacts_with_this_status.Count > 3) str += ", ...";
                MessageBox.Show($"Неможливо видалити статус! Цей статус прив'язан до: \n {str}", "Неможливо!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Ви дійсно хочете видалити Статус?", "Видалення!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.ContactStatuses.Remove(status);
                data.SaveChanges();
                UpdateStatusesListBox();
            }
        }
        #endregion
    }
}
