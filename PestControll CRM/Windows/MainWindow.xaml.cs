using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using PestControll_CRM.Windows.CRUD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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
        public ObservableCollection<CallType> callTypes { get; set; }
        public ObservableCollection<CallResultType> callResultTypes { get; set; }
        public ObservableCollection<Call> calls { get; set; }
        public ObservableCollection<PlannedCall> plannedCalls { get; set; }
        #endregion

        public MainWindow(DataContext data)
        {
            this.DataContext = this;
            this.data = data;
            
            foreach (ContactStatus status in data.contactStatuses.ToList())
            {
                Contact t = data.contacts.Where(c => c.contactstatus_id == status.Id).FirstOrDefault();
                if (t != null)
                    data.Entry(t).Reference(c => c.Status).Load();
            }


            InitializeComponent();


            contacts = new ObservableCollection<Contact>(data.contacts.ToList());
            UpdateStatusesListBox();
            UpdateContactsListBox();
            foreach (Contact contact in contacts) 
            {
                contact.PhoneNumbers = new ObservableCollection<PhoneNumber>(
                        data.PhoneNumbers.Where(num => num.Contact == contact)
                    );
            }

            callTypes = new ObservableCollection<CallType>(data.callTypes.ToList());
            callResultTypes = new ObservableCollection<CallResultType>(data.callResultType.ToList());
            UpdateCallsListBox();
            UpdatePlannedCallsListBox();

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

            UpdatePlannedCallsListBox();
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

        #region Contacts CRUD
        private void UpdateContactsListBox()
        {
            contacts = new ObservableCollection<Contact>(data.contacts.ToList().Where(c => FilteredContact(c)));
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
                data.contacts.Remove(contact);
                data.SaveChanges();
                UpdateContactsListBox();
            }
        }
        #endregion

        #region Statuses CRUD
        private void UpdateStatusesListBox()
        {
            contactStatuses = new ObservableCollection<ContactStatus>(data.contactStatuses.ToList());
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

            List<Contact> contacts_with_this_status = data.contacts.Where(c => c.Status == status).ToList();
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
                data.contactStatuses.Remove(status);
                data.SaveChanges();
                UpdateStatusesListBox();
            }
        }


        #endregion

        #region PlannedCalls CRUD
        private void UpdatePlannedCallsListBox()
        {
            plannedCalls = new ObservableCollection<PlannedCall>(data.plannedCalls.OrderBy(c => c.date).ThenBy(n => n.time));
            PlannedCallsListBox.ItemsSource = plannedCalls;

            if (plannedCalls.Where(pc => pc.TodayOrEarlier()).Count() > 0)
            {
                CallIcon.Visibility = Visibility.Collapsed;
                CallWarningIcon.Visibility = Visibility.Visible;
            }
            else
            {
                CallIcon.Visibility = Visibility.Visible;
                CallWarningIcon.Visibility = Visibility.Collapsed;
            }
        }
        private void PlannedCallsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlannedCall call = PlannedCallsListBox.SelectedItem as PlannedCall;
            if (call == null) return;

            StartPlannedCallButton.IsEnabled = true;
            EditPlannedCallButton.IsEnabled = true;
            DeletePlannedCallButton.IsEnabled = true;
        }
        private void CreatePlannedCallButton_Click(object sender, RoutedEventArgs e)
        {
            new PlannedCallCRUD(new PlannedCall(), data, PlannedCallCRUD.PlannedCallAction.Create, UpdatePlannedCallsListBox).Show();
        }

        private void StartPlannedCallButton_Click(object sender, RoutedEventArgs e)
        {
            PlannedCall call = PlannedCallsListBox.SelectedItem as PlannedCall;
            if (call == null) return;

            new CallingPlannedCall(data, call, UpdatePlannedCallsListBox, UpdateCallsListBox).Show();
        }

        private void EditPlannedCallButton_Click(object sender, RoutedEventArgs e)
        {
            PlannedCall call = PlannedCallsListBox.SelectedItem as PlannedCall;
            if (call == null) return;

            new PlannedCallCRUD(call, data, PlannedCallCRUD.PlannedCallAction.Update, UpdatePlannedCallsListBox).Show();
        }

        private void DeletePlannedCallButton_Click(object sender, RoutedEventArgs e)
        {
            PlannedCall call = PlannedCallsListBox.SelectedItem as PlannedCall;
            if (call == null) return;

            MessageBoxResult result = MessageBox.Show("Ви дійсно хочете видалити Запланований дзвінок?", "Видалення!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.plannedCalls.Remove(call);
                data.SaveChanges();
                UpdatePlannedCallsListBox();
            }
        }
        private Button last = null;
        private void ShowGoalPlannedCallButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Grid grid = button.Parent as Grid;
            if (grid == null) return;

            TextBlock goal = null;
            TextBlock arrow_up = null;
            TextBlock arrow_down = null;

            foreach (UIElement element in grid.Children) 
            {
                TextBlock block = element as TextBlock;
                if (block != null)
                {
                    if (block.Name == "GoalPlannedCallTextBlock")
                    {
                        goal = block;
                        continue;
                    }
                }
                else
                {
                    Button showButton = element as Button;
                    if (showButton != null)
                    {
                        StackPanel panel = showButton.Content as StackPanel;
                        if (panel != null)
                            foreach (UIElement el in panel.Children)
                            {
                                TextBlock b = el as TextBlock;
                                if (b != null)
                                {
                                    if (b.Name == "ShowGoalPlannedCallDown")
                                    {
                                        arrow_down = b;
                                        continue;
                                    }
                                    else if (b.Name == "ShowGoalPlannedCallUp")
                                    {
                                        arrow_up = b;
                                        continue;
                                    }
                                }
                            }
                    }
                }
            }
            if (goal == null) return;
            if (arrow_up == null) return; 
            if (arrow_down == null) return;

            bool goal_visible = (goal.Visibility == Visibility.Visible);
            if (goal_visible)
            {
                arrow_up.Visibility = Visibility.Collapsed;
                arrow_down.Visibility = Visibility.Visible;
                goal.Visibility = Visibility.Collapsed;
                last = null;
            }
            else
            {
                arrow_up.Visibility = Visibility.Visible;
                arrow_down.Visibility = Visibility.Collapsed;
                goal.Visibility = Visibility.Visible;
                if (last != null)
                    ShowGoalPlannedCallButton_Click(last, null);
                last = button;
            }
            int Id = (int)button.Tag;
            PlannedCallsListBox.SelectedItem = plannedCalls.Where(c => c.Id == Id).First();
        }
        private void TextBlock_Initialized(object sender, EventArgs e)
        {
            TextBlock block = sender as TextBlock;
            if (block == null) return;

            string r_name = block.Tag as string;
            if (r_name == null) return;

            block.Foreground = this.FindResource(r_name) as SolidColorBrush;
        }


        #endregion

        #region Calls CRUD

        public void UpdateCallsListBox()
        {
            calls = new ObservableCollection<Call>(data.calls.OrderByDescending(c => c.date_time));
            CallsListBox.ItemsSource = calls;
        }

        private void EditCallButton_Click(object sender, RoutedEventArgs e)
        {
            Call? call = CallsListBox.SelectedItem as Call;
            if (call == null) return;

            new CallCRUD(call, data, CallCRUD.CallAction.Update, UpdateCallsListBox).Show();
        }

        private void DeleteCallButton_Click(object sender, RoutedEventArgs e)
        {
            Call call = CallsListBox.SelectedItem as Call;
            if (call == null) return;

            MessageBoxResult result = MessageBox.Show("Ви дійсно хочете видалити Дзвінок з історії?", "Видалення!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.calls.Remove(call);
                data.SaveChanges();
                UpdateCallsListBox();
            }
        }
        private void CallsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Call call = CallsListBox.SelectedItem as Call;
            if (call != null)
            {
                EditCallButton.IsEnabled = true;
                DeleteCallButton.IsEnabled = true;
            }
            else
            {
                EditCallButton.IsEnabled = false;
                DeleteCallButton.IsEnabled = false;
            }
        }

        private void StartInnerCallButton_Click(object sender, RoutedEventArgs e)
        {
            Call call = new Call() { 
                call_type = data.innerCallTypeId, 
                callType = data.callTypes.Where(c => c.Id == data.innerCallTypeId).First() 
            };

            new CallCRUD(call, data, CallCRUD.CallAction.Create, UpdateCallsListBox).Show();
        }

        #endregion

        #region LegalPerson CRUD
        private void ViewLegalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateLegalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditLegalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteLegalPerson_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LegalPersonListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        #region NaturalPerson CRUD
        private void EditNaturalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNaturalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewNaturalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteNaturalPersonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NaturalPersonListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion
    }
}
