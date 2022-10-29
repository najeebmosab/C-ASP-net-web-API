using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("usersid")]
        public int usersid { get; set; }

        public virtual Users users { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }
        public IList<CartProduct> cartProducts { get; set; }

    }
}
