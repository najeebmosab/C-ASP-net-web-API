using FinalProject.Datas;
using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class UserServices
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly CartServices cartServices;
        private readonly CartProductService cartProductServices;
        private readonly OrderDetailsServices orderDetailsServices;
        private readonly IHostingEnvironment _host;

        public UserServices(ItemsContextForAllTables db, CartServices _cartServices, IHostingEnvironment host, CartProductService _cartProductServices, OrderDetailsServices _orderDetailsServices)
        {
            m_db = db;
            cartServices = _cartServices;
            _host = host;
            cartProductServices = _cartProductServices;
            orderDetailsServices = _orderDetailsServices;


        }
        public async Task<List<Users>> GetUsers()
        {
            return m_db.Users.ToList();
        }
        public async Task<Users> GetUser(int id)
        {
            Users U = m_db.Users.Where(u => u.id == id).Include(U=>U.CartUser).Include(U=>U.order).FirstOrDefault();
            return U;
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssff");
        }

        public async Task <Users> AddUser(UsersDTO user)
        {
           var u =  m_db.Users.Where(u =>u.email == user.email).FirstOrDefault();
            int c = 0;
            if (u == null)
            {
                try
                {
                    Users newUser = new Users();
                    newUser.userName = user.userName;
                    newUser.email = user.email;
                    newUser.password = GetMD5(user.password);
                    newUser.isAdmin = user.isAdmin;
                    newUser.isEmail = user.isEmail;
                    newUser.code = user.code;
                    newUser.gender = user.gender;
                    newUser.userImage = user.userImage;
                    newUser.Create_AT = DateTime.Now.ToLocalTime();
                    newUser.Update_AT = DateTime.Now.ToLocalTime();
                    m_db.Users.Add(newUser);
                    c = await m_db.SaveChangesAsync();
                    if (c > 0)
                    {
                        return newUser;
                    }
                }
                catch (Exception) { Console.WriteLine(); }
            }
            
             return null;
        }

        public async Task <Users> CheckEmailAsync(string E)
        {
            var u = m_db.Users.Where(u => u.email == E).FirstOrDefault();
            return u;
        }

        public Users PostUser(string email, string password)
        {
            string passwordAfterMD5 = GetMD5(password);

            return m_db.Users.Where(i => i.email.ToLower() == email.ToLower() && i.password == passwordAfterMD5)
                .FirstOrDefault();//lampada
        }

        private string GetMD5(string input) //123
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

        public async Task<ResponseDTO> DeleteItemAsync(int id)
        {
            //שליפה ישות מהמאגר
            Users UserToDelete = await GetUser(id);
            if (UserToDelete == null)
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity with id: {id} not found in DB"
                };
            }
            try
            {
                var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/users", UserToDelete.userImage));
                if (a)
                {
                    File.Delete(Path.Combine(_host.WebRootPath + "/images/users", UserToDelete.userImage));
                }
            }
            catch (Exception) { Console.WriteLine(""); }
            if (UserToDelete.CartUser != null) { 
                List<CartProduct> CP = await cartProductServices.GetCartProducts(UserToDelete.CartUser.id);
                for (int i = 0; i < CP.Count; i++)
                {
                    m_db.CartProduct.Remove(CP[i]);
                }
                m_db.Cart.Remove(UserToDelete.CartUser);
            }

            if (UserToDelete.order !=null)
            {
                for (int i = 0; i < UserToDelete.order.Count; i++)
                {
                    List<OrderDetails> OD = await orderDetailsServices.GetByOrderIdAsync(UserToDelete.order[i].id);
                    for (int j = 0; j < OD.Count; j++)
                    {
                        m_db.OrderDetails.Remove(OD[j]);
                    }
                    m_db.Order.Remove(UserToDelete.order[i]);
                }
            }
            

            
            m_db.Users.Remove(UserToDelete);
            int c = m_db.SaveChanges();
            ResponseDTO response = new ResponseDTO();
            if (c > 0)
            {
                response.Status = StatusCode.Success;
            }
            else
            {
                response.Status = StatusCode.Error;
            }
            return response;
        }

        public async Task<ResponseDTO> UpdateItemAsync(int id, UsersDTO user,IFormFile img)
        {
            Users UserFromDB = await GetUser(id);

            if (UserFromDB == null)
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Item {user.userName} with id {id} not found ib db"
                };
            }
            UserFromDB.userName = user.userName;
            UserFromDB.email = user.email;
            
            if (UserFromDB.password != user.password)
            {
                UserFromDB.password = GetMD5(user.password);
            }
            else {
                UserFromDB.password = UserFromDB.password;
            }

            UserFromDB.isAdmin = user.isAdmin;
            UserFromDB.gender = user.gender;
            if (img != null)
            {
                String timeStamp = GetTimestamp(DateTime.Now);
                var filePath = Path.Combine(_host.WebRootPath + "/images/users", timeStamp + img.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    try { 
                    var a = File.Exists(Path.Combine(_host.WebRootPath + "/images/users", UserFromDB.userImage));
                    if (a && UserFromDB.userImage != "defultImg.jpg")
                    { 
                        File.Delete(Path.Combine(_host.WebRootPath + "/images/users", UserFromDB.userImage));
                    }
                    }
                    catch (Exception) { Console.WriteLine(""); }
                    await img.CopyToAsync(fileStream);
                    UserFromDB.userImage = timeStamp + img.FileName;

                }
            }
            else {
                UserFromDB.userImage = UserFromDB.userImage;
            }
            UserFromDB.Create_AT = UserFromDB.Create_AT;
            UserFromDB.Update_AT = DateTime.Now.ToLocalTime();
            int c = m_db.SaveChanges();

            ResponseDTO response = new ResponseDTO();
            if (c > 0)
            {
                response.Status = StatusCode.Success;
            }
            else
            {
                response.Status = StatusCode.Error;
            }
            return response;
        }
    }
}
