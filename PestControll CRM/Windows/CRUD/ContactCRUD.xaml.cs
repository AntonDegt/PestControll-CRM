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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PestControll_CRM.Data.Entity;
using PestControll_CRM.Data;
using static System.Collections.Specialized.BitVector32;
using System.Data;
using System.ComponentModel;
using static PestControll_CRM.Windows.CRUD.ContactCRUD;
using System.Reflection;
using System.Windows.Markup;

namespace PestControll_CRM.Windows.CRUD
{
    /// <summary>
    /// Логика взаимодействия для ContactCRUD.xaml
    /// </summary>
    public partial class ContactCRUD : Window
    {
        public enum ContactAction { Read=0, Create, Update}
        private ContactAction action;

        public bool changed { get; set; } = false;

        public delegate void contactsListBoxUpdate();
        public contactsListBoxUpdate update;
        public List<Grid> phoneNumbersGrids = new List<Grid>();
        public List<TextBox> phoneNumbersTextBoxes = new List<TextBox>();


        private DataContext data { get; set; }
        public Contact contact { get; set; }
        public ObservableCollection<ContactStatus> statuses { get; set; }
        public ObservableCollection<PhoneNumber> phoneNumbers { get; set; }



        public ContactCRUD(Contact contact, ContactAction action, DataContext data, contactsListBoxUpdate update)
        {
            this.contact = contact;
            this.action = action;
            this.DataContext = this;
            this.data = data;
            this.update = update;

            statuses = new ObservableCollection<ContactStatus>(data.ContactStatuses.ToList());

            if (contact.PhoneNumbers == null)
                contact.PhoneNumbers = new ObservableCollection<PhoneNumber>();
            phoneNumbers = new ObservableCollection<PhoneNumber>(contact.PhoneNumbers.ToList());
            
            InitializeComponent();

        }
        private void WindowCRUD_Loaded(object sender, RoutedEventArgs e) => CheckAction(action);

        private void CheckAction(ContactAction action)
        {
            this.action = action;

            switch (action)
            {
                case ContactAction.Read:
                    WindowCRUD.Title = $"Контакт: {contact.PIB}";
                    PIBTextBox.IsEnabled = false;
                    StatusesComboBox.IsEnabled = false;
                    EmailTextBox.IsEnabled = false;
                    NotesTextBox.IsEnabled = false;
                    AddPhoneNumberButton.IsEnabled = false;
                    foreach (Grid grid in phoneNumbersGrids) grid.IsEnabled = false;

                    EditContactButton.IsEnabled = true;
                    SaveContactButton.IsEnabled = false;
                    break;

                case ContactAction.Create:
                    WindowCRUD.Title = "Створення контакту";
                    PIBTextBox.IsEnabled = true;
                    StatusesComboBox.IsEnabled = true;
                    EmailTextBox.IsEnabled = true;
                    NotesTextBox.IsEnabled = true;
                    AddPhoneNumberButton.IsEnabled = true;
                    foreach (Grid grid in phoneNumbersGrids) grid.IsEnabled = true;

                    EditContactButton.IsEnabled = false;
                    SaveContactButton.IsEnabled = true;

                    break;

                case ContactAction.Update:
                    WindowCRUD.Title = $"Контакт: {contact.PIB}";
                    PIBTextBox.IsEnabled = true;
                    StatusesComboBox.IsEnabled = true;
                    EmailTextBox.IsEnabled = true;
                    NotesTextBox.IsEnabled = true;
                    AddPhoneNumberButton.IsEnabled = true;
                    foreach (Grid grid in phoneNumbersGrids) grid.IsEnabled = true;

                    EditContactButton.IsEnabled = false;
                    SaveContactButton.IsEnabled = true;

                    break;
            }
        }

        private bool SaveContact()
        {
            // PIB
            PIBTextBox.BorderBrush = this.FindResource("pc_black") as SolidColorBrush;
            if (PIBTextBox.Text.Length == 0)
            {
                MessageBox.Show("Не заповнене поле ПІБ");
                PIBTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }

            // Status
            StatusesComboBox.BorderBrush = this.FindResource("pc_black") as SolidColorBrush;
            if (StatusesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Не вибраний статус Контакту");
                StatusesComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }

            // Phone Numbers
            bool flag = false;
            foreach (TextBox box in phoneNumbersTextBoxes)
                if (box.Text.Length != 9)
                    flag = true;
                else if (phoneNumbersTextBoxes.Where(b => b.Text == box.Text).Count() > 1)
                    flag = true;
                else if (data.PhoneNumbers.Where(num =>
                    (num.phone_number == "+380" + box.Text) && num.contact_id != contact.Id
                    ).Count() != 0)
                    flag = true;
            if (flag)
            {
                MessageBox.Show("Невірно вказані номера телефонів");
                return false;
            }

            // Save

            contact.PIB = PIBTextBox.Text;
            contact.Status = StatusesComboBox.SelectedItem as ContactStatus;
            contact.Email = EmailTextBox.Text;
            contact.Notes = NotesTextBox.Text;

            switch (action)
            {
                case ContactAction.Create:
                    data.Contacts.Add(contact);
                    break;
            }

            foreach (PhoneNumber phone in data.PhoneNumbers.Where(num => num.Contact == contact))
                data.PhoneNumbers.Remove(phone);
            foreach (TextBox box in phoneNumbersTextBoxes)
                data.PhoneNumbers.Add(new PhoneNumber() { Contact = contact, phone_number = ("+380" + box.Text) });


            data.SaveChanges();
            update.Invoke();

            action = ContactAction.Update;
            changed = false;
            return true;
        }
        private void EditContactButton_Click(object sender, RoutedEventArgs e) => CheckAction(ContactAction.Update);
        private void SaveContactButton_Click(object sender, RoutedEventArgs e) => SaveContact();
        
        
        private void PhoneNumber_Initialized(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null)
            {
                phoneNumbersTextBoxes.Add(box);
                PhoneNumber_TextChanged((object)box, null);
            }
        }
        private void PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null)
            {
                changed = true;
                if (box.Text.Length != 9)
                {
                    box.BorderBrush = this.FindResource("pc_red") as SolidColorBrush;
                    SaveContactButton.IsEnabled = false;
                    ErrorNumbersTextBox.Text = "Невірна довжина номеру";
                }
                else if (phoneNumbersTextBoxes.Where(b => b.Text == box.Text).Count() > 1)
                {
                    box.BorderBrush = this.FindResource("pc_red") as SolidColorBrush;
                    SaveContactButton.IsEnabled = false;
                    ErrorNumbersTextBox.Text = "Номер повторюється";
                }
                else if (data.PhoneNumbers.Where(num => 
                    (num.phone_number == "+380" + box.Text) && num.contact_id != contact.Id
                    ).Count() != 0)
                {
                    box.BorderBrush = this.FindResource("pc_red") as SolidColorBrush;
                    SaveContactButton.IsEnabled = false;
                    ErrorNumbersTextBox.Text = "Номер вже прив'язан до іншого контакту";
                }
                else
                {
                    box.BorderBrush = this.FindResource("pc_black") as SolidColorBrush;
                    SaveContactButton.IsEnabled = true;
                    ErrorNumbersTextBox.Text = "";
                }
            }
        }
        private void AddPhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            phoneNumbers.Add(new PhoneNumber() { contact_id = contact.Id, Contact = contact, phone_number = "+380" });
        }

        private void DeletePhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Grid grid = button.Parent as Grid;
            if (grid == null) return;


            ContentPresenter content = grid.TemplatedParent as ContentPresenter;
            if (content == null) return;

            ListBoxItem item = content.TemplatedParent as ListBoxItem;
            if (item == null) return;

            PhoneNumber number = item.Content as PhoneNumber;
            if (number == null) return;

            MessageBoxResult result = MessageBox.Show("Ви дійсно хочете видалити номер телефону?", "Видалити?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                TextBox deleteBox = phoneNumbersTextBoxes.Where(box => box.Text == number.phone_number_without_regions).First();
                phoneNumbersTextBoxes.Remove(deleteBox);
                phoneNumbers.Remove(number);

                if (phoneNumbersTextBoxes.Count == 0)
                    SaveContactButton.IsEnabled = true;
                else
                    PhoneNumber_TextChanged(phoneNumbersTextBoxes.First(), null);
            }
        }
        private void PhoneListBoxItemGrid_Initialized(object sender, EventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid != null)
                phoneNumbersGrids.Add(grid);
        }
        private void WindowCRUD_Closing(object sender, CancelEventArgs e)
        {
            if (action != ContactAction.Read)
            {
                if (changed)
                {
                    if (SaveContactButton.IsEnabled)
                    {
                        MessageBoxResult result = MessageBox.Show("Зберегти зміни?", "Зактири", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            e.Cancel = true;
                            if (SaveContact())
                                e.Cancel = false;
                        }
                        else if (result == MessageBoxResult.No) { e.Cancel = false; }
                        else if (result == MessageBoxResult.Cancel) { e.Cancel = true; }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("УВАГА!\nВи не можете зберігти ці зміни!\nЗакрити баз зберігання змін?", "Увага", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes) { e.Cancel = false; }
                        else if (result == MessageBoxResult.No) { e.Cancel = true; }
                    }
                }
            }
        }


        private void PIBTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(PIBTextBox.Text != contact.PIB) changed = true;
        }
        private void StatusesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusesComboBox.SelectedItem as ContactStatus != contact.Status) changed = true;
        }
        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmailTextBox.Text != contact.Email) changed = true;
        }
        private void NotesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NotesTextBox.Text != contact.Notes) changed = true;
        }

        private void StatusesComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (statuses.Count != data.ContactStatuses.Count())
            {
                statuses = new ObservableCollection<ContactStatus>(data.ContactStatuses.ToList());
                StatusesComboBox.ItemsSource = statuses;
            }
        }
    }
}
