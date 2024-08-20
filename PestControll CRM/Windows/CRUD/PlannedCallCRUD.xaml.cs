using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using SharpVectors.Scripting;
using static PestControll_CRM.Windows.CRUD.ContactCRUD;

namespace PestControll_CRM.Windows.CRUD
{
    /// <summary>
    /// Логика взаимодействия для PlannedCallCRUD.xaml
    /// </summary>
    public partial class PlannedCallCRUD : Window
    {
        public enum PlannedCallAction { Create=0, Update };
        public PlannedCallAction action;
        public delegate void plannedcallsListBoxUpdate();
        public plannedcallsListBoxUpdate update;


        private bool saved = false;
        public DataContext data;
        public PlannedCall plannedCall { get; set; }
        public Contact selectedContact { get; set; }
        public PlannedCallCRUD(PlannedCall plannedCall, DataContext data, PlannedCallAction action, plannedcallsListBoxUpdate update)
        {
            this.data = data;
            this.DataContext = this;

            this.plannedCall = plannedCall;
            this.action = action;
            this.update = update;

            if (plannedCall != null)
                selectedContact = plannedCall.contact;
            if (action == PlannedCallAction.Create)
                plannedCall.date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            InitializeComponent();
            UpdateContactInfo();
        }
        private void UpdateContactInfo()
        {

            if (plannedCall == null || selectedContact == null)
            {
                ContactInfoGrid.Visibility = Visibility.Collapsed;
                return;
            }
            else
                ContactInfoGrid.Visibility = Visibility.Visible;
            
            PIBTextBlock.Text = selectedContact.PIB;
            StatusTextBlock.Text = selectedContact.Status.StatusName;
            PhoneNumbersTextBlock.Text = selectedContact.PhoneNumbersStr;

            BrushConverter bc = new BrushConverter();
            StatusTextBlock.Foreground = (Brush)bc.ConvertFrom(selectedContact.Status.StatusColor);
        }

        private void SelectContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactSelect select = new ContactSelect(data);
            select.ShowDialog();

            selectedContact = select.selectedContact;
            UpdateContactInfo();
        }

        private bool Save()
        {
            DateTime date = (DateTime)DateDatePicker.SelectedDate;
            DateTime? time = TimeTimePicker.Value;

            if (selectedContact == null)
            {
                MessageBox.Show("Вкажіть контакт", "Помилка зберігання", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            if (date == null)
            {
                MessageBox.Show("Вкажіть дату", "Помилка зберігання", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }



            plannedCall.contact = selectedContact;
            plannedCall.date = new DateOnly(date.Year, date.Month, date.Day);
            if (time != null)
                plannedCall.time = new TimeOnly(time.Value.Hour, time.Value.Minute);
            else
                plannedCall.time = null;
            plannedCall.goal = GoalTextBox.Text;

            if (action == PlannedCallAction.Create)
            {
                data.plannedCalls.Add(plannedCall);
                action = PlannedCallAction.Update;
            }
            data.SaveChanges();
            update.Invoke();
            saved = true;

            MessageBox.Show("Успішно збережено!", "Збережено!", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void SavePlannedCall_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!saved)
            {
                MessageBoxResult result = MessageBox.Show("Зберегти зміни?", "Зактири", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    if (Save())
                        e.Cancel = false;
                }
                else if (result == MessageBoxResult.No) { e.Cancel = false; }
                else if (result == MessageBoxResult.Cancel) { e.Cancel = true; }
            }
        }
    }
}
