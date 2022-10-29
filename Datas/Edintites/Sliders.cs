using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Sliders")]
    public class Sliders
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("heading")]
        public string? heading { get; set; }

        [Column("description")]
        public string? description { get; set; }

        [Column("linke")]
        public string? linke { get; set; }

        [Column("linke_name")]
        public string? linkeName { get; set; }

        [Column("image")]
        public string? image { get; set; }

        [Column("status")]
        public bool? status { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset? Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset? Update_AT { get; set; }
    }
}
