using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControll_CRM.Data.Entity
{
    [Table("NaturalPersons")]
    public class NaturalPerson
    {
        [Key]
        [Column("contact_id")]
        public int contact_id { get; set; }

        [ForeignKey("contact_id")]
        public Contact contact { get; set; }


        [Column("ipn", TypeName = "varchar(20)")]
        public string IPN { get; set; }


        [Column("address", TypeName = "varchar(100)")]
        public string address { get; set; }
    }
}
