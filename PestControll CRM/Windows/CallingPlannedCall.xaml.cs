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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PestControll_CRM.Windows
{
    /// <summary>
    /// Логика взаимодействия для CallingPlannedCall.xaml
    /// </summary>
    public partial class CallingPlannedCall : Window
    {
        private DataContext data;
        public PlannedCall plannedCall { get; set; }
        public ObservableCollection<CallResultType> resultTypes { get; set; }

        public delegate void ListBoxUpdate();
        public ListBoxUpdate plannedCallsUpdate;
        public ListBoxUpdate callsUpdate;

        public CallingPlannedCall(DataContext data, PlannedCall plannedCall, ListBoxUpdate plannedCallsUpdate, ListBoxUpdate callsUpdate)
        {
            this.data = data;
            this.DataContext = this;
            this.plannedCall = plannedCall;
            this.resultTypes = new ObservableCollection<CallResultType>(data.callResultType.ToList());
            this.plannedCallsUpdate = plannedCallsUpdate;
            this.callsUpdate = callsUpdate;

            InitializeComponent();
            callingDateTimePicker.Value = DateTime.Now;
        }

        private void ResultComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallResultType? resultType = ResultComboBox.SelectedItem as CallResultType;
            if (resultType == null) return;

            if (data.replannedCallVisibilResultsId.Contains(resultType.Id))
                ReplacePlannedCallGrids.Visibility = Visibility.Visible;
            else
                ReplacePlannedCallGrids.Visibility = Visibility.Collapsed;
        }

        private void FinishPlannedCall_Click(object sender, RoutedEventArgs e)
        {
            DateTime datetime = (DateTime)callingDateTimePicker.Value;
            if (datetime == null)
            {
                MessageBox.Show("Вкажіть дату/час поточного дзвінку", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            CallResultType resultType = ResultComboBox.SelectedItem as CallResultType;
            if (resultType == null)
            {
                MessageBox.Show("Вкажіть результат дзвінка", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            if (data.replannedCallVisibilResultsId.Contains(resultType.Id))
            {
                DateTime dt = (DateTime)DateDatePicker.SelectedDate;
                if (dt == null)
                {
                    MessageBox.Show("Вкажіть дату наступного дзвінка", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
            }



            CallType outter = data.callTypes.Where(ct => ct.Id == data.outterCallTypeId).First();
            if (outter == null) return;

            Call call = new Call()
            {
                callType = outter,
                contact = plannedCall.contact,
                date_time = datetime,
                callResultType = resultType,
                comment = CommentTextBox.Text
            };
            data.calls.Add(call);
            data.plannedCalls.Remove(plannedCall);

            if (data.replannedCallVisibilResultsId.Contains(resultType.Id))
            {
                DateTime dt = (DateTime)DateDatePicker.SelectedDate;

                PlannedCall newplannedCall = new PlannedCall()
                {
                    contact = plannedCall.contact,
                    date = new DateOnly(dt.Year, dt.Month, dt.Day),
                    goal = GoalTextBox.Text
                };

                DateTime? t = null;
                if (TimeTimePicker.Value != null) t = (DateTime)TimeTimePicker.Value;
                if (t != null) newplannedCall.time = new TimeOnly(t.Value.Hour, t.Value.Minute);

                data.plannedCalls.Add(newplannedCall);
            }

            data.SaveChanges();
            plannedCallsUpdate.Invoke();
            callsUpdate.Invoke();

            MessageBox.Show("Успішно збережено!", "Збережено!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
