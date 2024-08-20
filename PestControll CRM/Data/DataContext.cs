
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using PestControll_CRM.Data.Entity;
using PestControll_CRM.Data.Entity.Clients;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PestControll_CRM.Data
{
    public class DataContext : DbContext
    {
        // Contacts
        public DbSet<ContactStatus> contactStatuses { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }


        // Calls
        public DbSet<CallType> callTypes { get; set; }
        public DbSet<CallResultType> callResultType { get; set; }
        public DbSet<Call> calls { get; set; }
        public DbSet<PlannedCall> plannedCalls { get; set; }


        //Clients
        public DbSet<TaxSystem> taxSystems { get; set; }
        public DbSet<NaturalPerson> naturalPersons { get; set; }
        public DbSet<LegalPerson> legalPeople { get; set; }
        public DbSet<Position> positions { get; set; }



        public readonly int[] replannedCallVisibilResultsId = { 1, 2 };
        public readonly int innerCallTypeId = 1;
        public readonly int outterCallTypeId = 2;


        private TextBox loginBox;
        private PasswordBox passwordBox;
        public void EnterParams(TextBox loginBox, PasswordBox passwordBox)
        {
            this.loginBox = loginBox;
            this.passwordBox = passwordBox;
        }
        public DataContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                $"Server=localhost;Database=pccrm;User=root;Password=;",
                //$"Server={App.server};Database={App.database};User={loginBox.Text};Password={passwordBox.Password};",
            new MySqlServerVersion(new Version(7, 0, 0)));
        }

        public bool isConnect 
        { 
            get 
            {
                try
                {
                    Database.OpenConnection();
                    Database.CloseConnection();
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            } 
        }
    }
}
