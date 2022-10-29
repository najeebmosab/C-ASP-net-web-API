using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Images")]
    public class Images
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("productId")]
        public int productId { get; set; }
        public virtual Products products { get; set; }

        [Column("Path")]
        public string Path { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }
    }
}
