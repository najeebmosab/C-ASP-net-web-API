using AutoMapper;
using FinalProject.Datas;
using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class CartServices
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly IMapper Mapper;
        public CartServices(ItemsContextForAllTables db, IMapper _Mapper)
        {
            m_db = db;
            Mapper = _Mapper;
        }

        public async Task<List<Cart>> GetCarts()
        {
            return m_db.Cart.ToList<Cart>();
        }

        public async Task<Cart> GetCartByUserIdAsync(int id)
        {
            var e =  m_db.Cart.Where(cart => cart.usersid == id).Include(C=>C.cartProducts).FirstOrDefault();
            if (e == null)
            {
                CartDTO C = new CartDTO();
                C.usersid = id;
                this.AddCart(C);
            }
            return e;
        }

        public Cart GetCart(int? Id)
        {
            var e = m_db.Cart.Where(cart => cart.id == Id).FirstOrDefault();
            return e;
        }

        public async Task<Cart> AddCart(Cart C)
        {
            Cart newItem = new Cart();
            newItem.usersid = C.usersid;
            newItem.Create_AT = DateTime.Now.ToLocalTime();
            newItem.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Cart.AddAsync(newItem);

            int c = await m_db.SaveChangesAsync();
            if (c > 0) return newItem;
            return null;
        }

        public async Task<Cart> AddCart(CartDTO C)
        {
            Cart newItem = new Cart();
            newItem.usersid = C.usersid;
            newItem.Create_AT = DateTime.Now.ToLocalTime();
            newItem.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Cart.AddAsync(newItem);

            int c = await m_db.SaveChangesAsync();
            if (c > 0) return newItem;
            return null;
        }


        public ResponseDTO DeleteCart(int id)
        {
            Cart itemToDelete = GetCart(id);
            if (itemToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }

            m_db.Cart.Remove(itemToDelete);
            int c = m_db.SaveChanges();
            ResponseDTO response = new ResponseDTO();
            if (c > 0)
            {
                response.Status = StatusCode.Success;
            }
            else
            {
                response.Status = StatusCode.Error;
                response.StatusText = "Error";
            }
            return response;
        }

       

        public ResponseDTO UpdetCart(int id, CartDTO newCart)
        {

            
            Cart itemToUpdet = GetCart(id);
            if (itemToUpdet == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            itemToUpdet.usersid = newCart.usersid;
            itemToUpdet.Create_AT = itemToUpdet.Create_AT;
            itemToUpdet.Update_AT = DateTime.Now.ToLocalTime();


            m_db.Cart.Update(itemToUpdet);
            int c = m_db.SaveChanges();
            ResponseDTO response = new ResponseDTO();
            if (c > 0)
            {
                response.Status = StatusCode.Success;
            }
            else
            {
                response.Status = StatusCode.Error;
                response.StatusText = "Error";
            }
            return response;
        }
    }
}
