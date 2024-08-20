using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControll_CRM.Data.Entity
{
    [Table("PlannedCalls")]
    public class PlannedCall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }



        [Column("contact_id")]
        public int? contact_id { get; set; }

        [ForeignKey("contact_id")]
        public virtual Contact? contact { get; set; }



        [Column("date", TypeName = "date")]
        public DateOnly date { get; set; }
        public DateTime dateDateTime
        {
            get
            {
                if (date != null)
                    return new DateTime(date.Year, date.Month, date.Day);
                else
                    return DateTime.Now;
            }
        }
        public string date_str
        {
            get 
            {
                return date.ToLongDateString();
            }
        }

        [Column("time", TypeName = "time"), MaybeNull]
        public TimeOnly? time { get; set; }
        public DateTime? timeDateTime
        {
            get
            {
                if (time != null)
                    return new DateTime(date.Year, date.Month, date.Day, time.Value.Hour, time.Value.Minute, 0);

                return null;
            }
        }
        public string time_str
        {
            get
            {
                if (time != null)
                    return time.Value.ToString("t", CultureInfo.InvariantCulture);
                else
                    return "";
            }
        }




        [Column("goal", TypeName = "text")]
        public string goal { get; set; }


        public string color_binding_name
        {
            get
            {
                string color = "pc_black";

                DateTime today = DateTime.Now;
                DateOnly dateOnly = new DateOnly(today.Year, today.Month, today.Day);
                TimeOnly timeOnly = new TimeOnly(today.Hour, today.Minute);
                if (dateOnly == this.date)
                {
                    color = "pc_orange";
                    if (timeOnly >= this.time)
                        color = "pc_red";
                }
                else if (dateOnly > this.date)
                    color = "pc_red";

                return color;
            }
        }
        public bool TodayOrEarlier()
        {
            DateTime today = DateTime.Now;
            DateOnly dateOnly = new DateOnly(today.Year, today.Month, today.Day);
            TimeOnly timeOnly = new TimeOnly(today.Hour, today.Minute);
            if (dateOnly >= this.date)
                return true;
            else
                return false;
        }
    }
}
