﻿using PestControll_CRM.Data;
using PestControll_CRM.Data.Entity;
using System;
using System.Collections.Generic;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PestControll_CRM.Windows.CRUD.ContactCRUD;
using static System.Collections.Specialized.BitVector32;

namespace PestControll_CRM.Windows.CRUD
{
    /// <summary>
    /// Логика взаимодействия для StatusCRUD.xaml
    /// </summary>
    public partial class StatusCRUD : Window
    {
        public enum ActionStatus { Update = 0, Create };

        public DataContext data { get; set; }
        public ContactStatus status { get; set; }
        public ActionStatus action { get; set; }
        public bool changed = false;

        public delegate void statusesListBoxUpdate();
        public statusesListBoxUpdate update;


        public StatusCRUD(ContactStatus status, ActionStatus actionStatus, DataContext data, statusesListBoxUpdate update)
        {
            this.status = status;
            this.action = actionStatus;
            this.data = data;
            this.update = update;

            InitializeComponent();

            Color selectedColor = (Color)ColorConverter.ConvertFromString(status.StatusColor);
            StatusColorPicker.SelectedColor = selectedColor;
            
            CheckAction();
        }
        private void CheckAction()
        {
            switch (action)
            {
                case ActionStatus.Update:
                    WindowCRUD.Title = $"Статус: {status.StatusName}";
                    NameTextBox.Text = status.StatusName;
                    StatusColorPicker.SelectedColor = status.color;
                    break;
                case ActionStatus.Create:
                    WindowCRUD.Title = $"Створення статусу";
                    break;
            }
        }
        private void SaveStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (!changed)
                return;

            SaveStatus();
        }
        private bool SaveStatus()
        {
            Color? color = StatusColorPicker.SelectedColor;
            if (color == null)
            {
                MessageBox.Show("Вкажіть колір.");
                return false;
            }
            if (!color.HasValue)
            {
                MessageBox.Show("Вкажіть колір.");
                return false;
            }

            status.StatusName = NameTextBox.Text;
            status.StatusColor = StatusColorPicker.SelectedColorText;

            if (action == ActionStatus.Create)
                data.contactStatuses.Add(status);

            data.SaveChanges();
            action = ActionStatus.Update;
            changed = false;
            CheckAction();
            update.Invoke();


            MessageBox.Show("Успішно збережено!", "Збережено!", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e) => changed = true;
        

        private void WindowCRUD_Loaded(object sender, RoutedEventArgs e) => CheckAction();

        private void WindowCRUD_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changed)
            {
                if (SaveStatusButton.IsEnabled)
                {
                    MessageBoxResult result = MessageBox.Show("Зберегти зміни?", "Зактири", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                        if (SaveStatus())
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

        private void StatusColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) => changed = true;
    }
}
