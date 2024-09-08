using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PestControll_CRM.Data.Entity
{
    [Table("ContactStatus")]
    public class ContactStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }


        [Column("StatusName", TypeName = "varchar(20)")]
        public string StatusName { get; set; }


        [Column("StatusColor", TypeName = "varchar(20)")]
        public string StatusColor { get; set; } = null;

        public Color? color
        {
            get
            {
                Color? c;
                try
                {
                    c = (Color)ColorConverter.ConvertFromString(this.StatusColor);
                }
                catch
                {
                    c = null;
                }
                return c;
            }
        }
    }
}
