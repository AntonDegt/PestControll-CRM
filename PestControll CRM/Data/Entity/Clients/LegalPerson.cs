using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControll_CRM.Data.Entity
{
    [Table("LegalPersons")]
    public class LegalPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(40)")]
        public string name { get; set; }

        
        [Column("edrpou", TypeName = "varchar(20)")]
        public string EDRPOU { get; set; }


        [Column("taxsystem_id")]
        public int taxsystem_id { get; set; }

        [ForeignKey("taxsystem_id")]
        public virtual TaxSystem TaxSystem { get; set; }
        
        
        [Column("current_account", TypeName = "varchar(40)")]
        public string current_account { get; set; }

        [Column("address", TypeName = "varchar(100)")]
        public string address { get; set; }

        [Column("email", TypeName = "varchar(40)"), MaybeNull]
        public string email { get; set; } = null;

    }
}
