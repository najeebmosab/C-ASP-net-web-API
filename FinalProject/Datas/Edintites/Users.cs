using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("userName")]
        public string userName { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("userImage")]
        public string userImage { get; set; }

        [Column("isEmail")]
        public bool isEmail { get; set; }

        [Column("code")]
        public string code { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Column("isAdmin")]
        public bool isAdmin { get; set; }

         [Column("gender")]
        public bool gender { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }
         
        public Cart CartUser { get; set; }

        public virtual IList<Order> order{ get; set; }

    }
}
