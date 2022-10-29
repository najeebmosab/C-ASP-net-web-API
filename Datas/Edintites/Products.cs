using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Products")]
    public class Products
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [ForeignKey("brandId")]
        [Column("brandId")]
        public int? brandId { get; set; }
        public Brands brand { get; set; }

        [Required]
        [ForeignKey("categoryId")]
        [Column("categoryId")]
        public int? categoryId { get; set; }

        [Required]
        public Category category { get; set; }

        [Column("productName")]
        public string? productName { get; set; }


        [Column("price")]
        public decimal? price { get; set; }


        [Column("salePrice")]
        public decimal? salePrice { get; set; }
        

        [Column("actualPrice")]
        public decimal? actualPrice { get; set; }

        [Column("quantity")]
        public string? quantity { get; set; }


        [Column("shortDescription")]
        public string? shortDescription { get; set; }

        [Column("description")]
        public string? description { get; set; }


        [Column("imageProduct")]
        public string? imageProduct { get; set; }

        [Column("gender")]
        public string? gender { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset? Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset? Update_AT { get; set; }

        [Required]
        public virtual IList<Images>? Image { get; set; }

        public IList<CartProduct> cartProducts { get; set; }

    }
}
