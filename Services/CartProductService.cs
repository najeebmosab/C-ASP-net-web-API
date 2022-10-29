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
    public class CartProductService
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly IMapper Mapper;
        public CartProductService(ItemsContextForAllTables db, IMapper _Mapper)
        {
            m_db = db;
            Mapper = _Mapper;
        }

        public async Task<List<CartProduct>> GetCartProducts(int id)
        {
            return m_db.CartProduct.Where(CP=>CP.cartId == id).Include(CP => CP.products).ToList<CartProduct>();
        }


        public async Task<CartProduct> GetCartProduct(long id)
        {
            return m_db.CartProduct.Where(CP => CP.id == id).FirstOrDefault();
        }

        public async Task<CartProduct> AddCart(CartProductDTO C)
        {
            var e = m_db.CartProduct.Where(CP => CP.productId == C.productId && CP.cartId == C.cartId).FirstOrDefault();
            if (e != null)
            {
                return null;
            }
            else {
            CartProduct newItem = new CartProduct();
            newItem.productId = C.productId;
            newItem.cartId = C.cartId;
            newItem.quantity = C.quantity;
            newItem.Create_AT = DateTime.Now.ToLocalTime();
            newItem.Update_AT = DateTime.Now.ToLocalTime();
            m_db.CartProduct.AddAsync(newItem);

            newItem.id = await m_db.SaveChangesAsync();
            if (newItem.id > 0) return newItem;

            }
            return null;
        }

        public async Task<ResponseDTO> UpdeteAsync(long id, CartProductDTO obj) {
            CartProduct itemToUpdet = await GetCartProduct(id);
            if (itemToUpdet == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }

            itemToUpdet.quantity = obj.quantity;

            int c = await m_db.SaveChangesAsync();
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

        public async Task<ResponseDTO> DeleteItem(int id)
        {
            CartProduct itemToDelete = await GetCartProduct(id);
            if (itemToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            
            m_db.CartProduct.Remove(itemToDelete);
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
