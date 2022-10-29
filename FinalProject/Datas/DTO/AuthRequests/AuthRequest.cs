using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO.AuthRequests
{
    public class AuthRequest
    {
        public AuthRequest(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
        public string email { get; set; }
        public string password { get; set; }
    }
}
