using Microsoft.EntityFrameworkCore.Query.Internal;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using PestControll_CRM.Windows.CRUD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Логика взаимодействия для ContactSelect.xaml
    /// </summary>
    public partial class ContactSelect : Window
    {
        private DataContext data { get; set; }
        private ObservableCollection<Contact> contacts { get; set; }
        public ObservableCollection<ContactStatus> contactStatuses { get; set; }

        public Contact selectedContact { get; set; }

        public ContactSelect(DataContext data)
        {
            this.data = data;
            this.DataContext = this;

            contacts = new ObservableCollection<Contact>(data.contacts.ToList());
            contactStatuses = new ObservableCollection<ContactStatus>(data.contactStatuses.ToList());
            foreach (Contact contact in contacts)
            {
                contact.PhoneNumbers = new ObservableCollection<PhoneNumber>(
                        data.PhoneNumbers.Where(num => num.Contact == contact)
                    );
            }

            InitializeComponent();
            SearchContacts();
        }

        private void SelectContactButton_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = ContactsListBox.SelectedItem as Contact;
            if (contact != null)
            {
                selectedContact = contact;
                this.Close();
            }
        }
        private void ContactsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectContactButton_Click(null, null);
        }

        #region Search/Filter Contacts
        private bool FilteredContact(Contact contact)
        {
            bool flag = true;
            if (ContactSearchTextBox.Text.Length > 0)
                if (!contact.PIB.ToLower().Contains(ContactSearchTextBox.Text.ToLower()) &&
                    !contact.PhoneNumbersStr.Contains(ContactSearchTextBox.Text))
                    flag = false;

            ContactStatus? status = ContactStatusComboBox.SelectedValue as ContactStatus;
            if (status != null)
                if (contact.Status != status)
                    flag = false;
            return flag;
        }

        private void SearchContacts()
        {
            contacts = new ObservableCollection<Contact>(data.contacts.ToList().Where(c => FilteredContact(c)));
            ContactsListBox.ItemsSource = contacts;
        }
        private void ContactStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContactStatusComboBox.SelectedItem != null)
                ClearSelectedStatusButton.Visibility = Visibility.Visible;
            else
                ClearSelectedStatusButton.Visibility = Visibility.Hidden;
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

        private void CreateContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactCRUD cCRUD = new ContactCRUD(new Contact(), ContactCRUD.ContactAction.Create, data, SearchContacts, null, true);
            cCRUD.Show();
            cCRUD.Closed += ContactCreated;
        }

        private void ContactCreated(object sender, EventArgs e)
        {
            ContactCRUD contactCRUD = sender as ContactCRUD;
            if (contactCRUD == null) return;

            Contact contact = contactCRUD.contact;
            ContactSearchTextBox.Text = contact.PIB;
        }
    }
}
