using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControll_CRM.Data.Entity
{
    [Table("Calls")]
    public class Call
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }


        [Column("call_type_id")]
        public int? call_type { get; set; }

        [ForeignKey("call_type")]
        public virtual CallType? callType { get; set; }


        [Column("contact_id")]
        public int? contact_id { get; set; }

        [ForeignKey("contact_id")]
        public virtual Contact? contact { get; set; }


        [Column("date_time", TypeName = "datetime")]
        public DateTime date_time { get; set; }
        

        [Column("call_result_type_id")]
        public int? call_result_type { get; set; }

        [ForeignKey("call_result_type")]
        public virtual CallResultType? callResultType { get; set; }


        [Column("comment", TypeName = "text")]
        public string comment { get; set; }
    }
}
