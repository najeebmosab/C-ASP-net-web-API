using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("orderId")]
        public int orderId { get; set; }
        [Required]
        public virtual Order order { get; set; }

        [Column("categoryId")]
        public int categoryId { get; set; }
        [Required]
        public virtual Category category { get; set; }

        [Column("brandId")]
        public int brandId { get; set; }
        [Required]
        public virtual Brands brand { get; set; }

        [Column("productImage")]
        public string productImage { get; set; }

        [Column("productName")]
        public string productName { get; set; }

        [Column("productPrice")]
        public decimal productPrice { get; set; }

        [Column("productDescription")]
        public string productDescription { get; set; }

        [Column("quantity")]
        public int quantity { get; set; }

        [Column("size")]
        public string size { get; set; }

        [Column("isOrder")]
        public bool isOrder { get; set; }


        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }

    }
}
