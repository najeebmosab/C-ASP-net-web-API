using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class OrderDTO
    {
        public int id { get; set; }

        public int userId { get; set; }
        

        public string email { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public string city { get; set; }

        public string postalCode { get; set; }

        public string phone { get; set; }

        public decimal total { get; set; }

        public DateTimeOffset Create_AT { get; set; }

        public DateTimeOffset Update_AT { get; set; }

        public ICollection<OrderDetails> orderDetails { get; set; }
        public Users user { get; set; }


    }
}
