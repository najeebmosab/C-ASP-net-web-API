using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class CartDTO
    {
        public int id { get; set; }

        public int usersid { get; set; }

        public DateTimeOffset Create_AT { get; set; }

        public DateTimeOffset Update_AT { get; set; }
        public IList<CartProduct> cartProducts { get; set; }

    }
}
