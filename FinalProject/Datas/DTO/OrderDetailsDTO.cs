using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class OrderDetailsDTO
    {
        
        public int id { get; set; }

        public int orderId { get; set; }
        public virtual Order order { get; set; }

        public int categoryId { get; set; }
        public virtual Category category { get; set; }

        public int brandId { get; set; }
        public virtual Brands brand { get; set; }

        public string productImage { get; set; }

        public string productName { get; set; }

        public decimal productPrice { get; set; }

        public string productDescription { get; set; }

        public int quantity { get; set; }

        public string size { get; set; }

        public bool isOrder { get; set; }

        public DateTimeOffset Create_AT { get; set; }

        public DateTimeOffset Update_AT { get; set; }

    }
}
