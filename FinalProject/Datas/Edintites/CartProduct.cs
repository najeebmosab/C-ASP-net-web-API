using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("CartProduct")]
    public class CartProduct
    {
        [Key]
        [Column("id")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long id { get; set; }

        [Column("productId")]
        public virtual int productId { get; set; }
        public virtual Products products { get; set; }

        [Column("cartId")]
        public virtual int cartId { get; set; }
        public virtual Cart cart { get; set; }


        [Column("quantity")]
        public int quantity { get; set; }
        
        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }
    }
}
