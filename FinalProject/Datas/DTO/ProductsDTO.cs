using FinalProject.Datas.Edintites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class ProductsDTO
    {
        public int id { get; set; }

        public string? productName { get; set; }

        public int? brandId { get; set; }
        public virtual Brands brand { get; set; }

        public Nullable<int> categoryId { get; set; }


        public virtual Category category { get; set; }

        public decimal? price { get; set; }


        public decimal? salePrice { get; set; }

        public decimal? actualPrice { get; set; }

        public string? quantity { get; set; }

        public string? shortDescription { get; set; }

        public string? description { get; set; }

        public string? imageProduct { get; set; }

        public string? gender { get; set; }

        public DateTimeOffset? Create_AT { get; set; }

        public DateTimeOffset? Update_AT { get; set; }

        public virtual IList<Images>? Image { get; set; }


        public IList<CartProduct> cartProducts { get; set; }


    }
}
