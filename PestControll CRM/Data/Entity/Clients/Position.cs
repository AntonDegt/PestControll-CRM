using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControll_CRM.Data.Entity.Clients
{
    [Table("Positions")]
    public class Position
    {
        [Key]
        [Column("contact_id")]
        public int contact_id { get; set; }

        [ForeignKey("contact_id")]
        public Contact contact { get; set; }


        [Column("legalperson_id")]
        public int legalperson_id { get; set; }

        [ForeignKey("legalperson_id")]
        public LegalPerson legalPerson { get; set; }



        [Column("priority_contact", TypeName = "tinyint(1)")]
        public bool priorityContact { get; set; }

        [Column("position", TypeName = "varchar(40)")]
        public string position { get; set; }
    }
}
