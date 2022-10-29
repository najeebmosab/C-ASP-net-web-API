using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("userId")]
        [Required]
        public int userId { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("address")]
        public string address { get; set; }

        [Column("city")]
        public string city { get; set; }

        [Column("postalCode")]
        public string postalCode { get; set; }

        [Column("phone")]
        public string phone { get; set; }

        [Column("total")]
        public decimal total { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }
        public IList<OrderDetails> orderDetails { get; set; }
        public virtual Users user{ get; set; }

    }
}
