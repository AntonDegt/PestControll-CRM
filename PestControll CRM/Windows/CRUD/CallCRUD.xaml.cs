using Microsoft.EntityFrameworkCore.Query.Internal;
using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
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
using System.Windows.Shapes;
using static PestControll_CRM.Windows.CRUD.ContactCRUD;

namespace PestControll_CRM.Windows.CRUD
{
    /// <summary>
    /// Логика взаимодействия для CallCRUD.xaml
    /// </summary>
    public partial class CallCRUD : Window
    {
        public enum CallAction { Create = 0, Update };
        public CallAction action;
        public delegate void callsListBoxUpdate();
        public callsListBoxUpdate update;

        private DataContext data;
        public Call call { get; set; } 
        public ObservableCollection<CallType> callTypes { get; set; }
        public ObservableCollection<CallResultType> callResultTypes { get; set; }

        private bool changed = false;


        public Contact? selectedContact { get; set; }

        public CallCRUD(Call call, DataContext data, CallAction action, callsListBoxUpdate update)
        {
            this.call = call;
            this.selectedContact = call.contact;
            this.data = data;
            this.action = action;
            this.update = update;
            this.DataContext = this;

            callTypes = new ObservableCollection<CallType>(data.callTypes.ToList());
            callResultTypes = new ObservableCollection<CallResultType>(data.callResultType.ToList());


            InitializeComponent();
            ResultComboBox_SelectionChanged(null, null);

            if (selectedContact != null)
                UpdateContactInfo();
            callingDateTimePicker.Value = DateTime.Now;
        }

        private void UpdateContactInfo()
        {
            if (call == null || selectedContact == null)
            {
                ContactInfoGrid.Visibility = Visibility.Collapsed;
                return;
            }
            else
                ContactInfoGrid.Visibility = Visibility.Visible;

            PIBTextBlock.Text = selectedContact.PIB;
            if (selectedContact.Status != null)
            {
                StatusTextBlock.Text = selectedContact.Status.StatusName;
                BrushConverter bc = new BrushConverter();
                StatusTextBlock.Foreground = bc.ConvertFrom(selectedContact.Status.StatusColor) as Brush;
            }


            PhoneNumbersTextBlock.Text = selectedContact.PhoneNumbersStr;

        }
        private void SelectContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactSelect select = new ContactSelect(data);
            select.ShowDialog();

            selectedContact = select.selectedContact;
            UpdateContactInfo();

            changed = true;
        }


        private void SaveCall_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void WindowCRUD_Closing(object sender, CancelEventArgs e)
        {
            if (changed)
            {
                if (SaveCall.IsEnabled)
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
                else
                {
                    MessageBoxResult result = MessageBox.Show("УВАГА!\nВи не можете зберігти ці зміни!\nЗакрити баз зберігання змін?", "Увага", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes) { e.Cancel = false; }
                    else if (result == MessageBoxResult.No) { e.Cancel = true; }
                }
            }
        }
        private bool Save()
        {
            CallType? callType = CallTypeComboBox.SelectedItem as CallType;
            if (callType == null)
            {
                MessageBox.Show("Вкажіть тип дзвінку", "Помилка зберігання", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }

            CallResultType callResultType = CallResultTypeComboBox.SelectedItem as CallResultType;
            if (callResultType == null)
            {
                MessageBox.Show("Вкажіть тип результату дзвінку", "Помилка зберігання", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }

            DateTime? datetime = callingDateTimePicker.Value as DateTime?;
            if (datetime == null)
            {
                MessageBox.Show("Вкажіть Дату/Час дзвінку", "Помилка зберігання", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }


            call.contact = selectedContact;
            call.callType = callType;
            call.callResultType = callResultType;
            call.date_time = datetime.Value;
            call.comment = CommentTextBox.Text;

            data.SaveChanges();
            update.Invoke();
            action = CallAction.Update;
            changed = false;

            MessageBox.Show("Успішно збережено!", "Збережено!", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void ResultComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changed = true;
        }
        private void CallTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changed = true;
        }
        private void callingDateTimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            changed = true;
        }
    }
}
