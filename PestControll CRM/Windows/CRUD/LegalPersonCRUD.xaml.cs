using PestControll_CRM.Data.Entity;
using PestControll_CRM.Data;
using System;
using System.Collections.Generic;
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
using static PestControll_CRM.Windows.CRUD.ContactCRUD;

namespace PestControll_CRM.Windows.CRUD
{
    /// <summary>
    /// Логика взаимодействия для LegalPersonCRUD.xaml
    /// </summary>
    public partial class LegalPersonCRUD : Window
    {
        public enum LegalPersonAction { Read = 0, Create, Update }
        private LegalPersonAction action;

        public delegate void legalPeopleListBoxUpdate();
        public legalPeopleListBoxUpdate update;

        private bool changed = false;

        public LegalPerson legalPerson { get; set; }
        public DataContext data { get; set; }

        public LegalPersonCRUD(LegalPerson legalPerson, LegalPersonAction action, DataContext data, legalPeopleListBoxUpdate update)
        {
            this.legalPerson = legalPerson;
            this.action = action;
            this.update = update;
            this.data = data; 
            

            InitializeComponent();
        }
    }
}
