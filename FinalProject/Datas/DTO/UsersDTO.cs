using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class UsersDTO
    {
        public int id { get; set; }

        public string userName { get; set; }

        public string email { get; set; }


        public string password { get; set; }

        public bool isAdmin { get; set; }
        public bool isEmail { get; set; }

        public bool gender { get; set; }

        public string code { get; set; }

        public string userImage { get; set; }

        public DateTimeOffset Create_AT { get; set; }

        public DateTimeOffset Update_AT { get; set; }

        public IList <Order> order { get; set; }


    }
}
