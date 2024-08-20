using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PestControll_CRM.Data.Entity
{
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("contactstatus_id")]
        public int? contactstatus_id { get; set; }

        [ForeignKey("contactstatus_id")]
        public virtual ContactStatus? Status { get; set; }

        [Column("pib", TypeName = "varchar(60)")]
        public string PIB { get; set; }

        [Column("email", TypeName = "varchar(40)")]
        public string Email { get; set; } = null;

        [Column("notes", TypeName = "text")]
        public string Notes { get; set; } = null;

        public ObservableCollection<PhoneNumber> PhoneNumbers { get; set; }
        public string PhoneNumbersStr { get 
            {
                if (PhoneNumbers.Count > 0)
                {
                    string str = $"{PhoneNumbers[0].phone_number}";
                    for (int i = 1; i < PhoneNumbers.Count; i++)
                    {
                        str += $"\n{PhoneNumbers[i].phone_number}";
                    }
                    return str;
                }
                return "";
            }
        }
    }
}
