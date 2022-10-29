using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class CartProductDTO
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long id { get; set; }

        public virtual int productId { get; set; }
        public virtual Products products { get; set; }

        public virtual int cartId { get; set; }
        public virtual Cart cart { get; set; }

        public int quantity { get; set; }

        public DateTimeOffset Create_AT { get; set; }

        public DateTimeOffset Update_AT { get; set; }
    }
}
