using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PestControll_CRM.Data.Entity
{
    [Table("PhoneNumber")]
    public class PhoneNumber
    {
        [Key]
        [Column("phone_number")]
        public string phone_number { get; set; }
        public string phone_number_without_regions {
            get
            {
                return phone_number.Substring(4);
            }
        }

        [Column("contact_id")]
        public int? contact_id { get; set; }

        [ForeignKey("contact_id")]
        public virtual Contact? Contact { get; set; }
    }
}
