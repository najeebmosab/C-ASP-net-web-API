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
    public class OrderServices
    {
        private readonly ItemsContextForAllTables m_db;
        private readonly UserServices _service;
        public OrderServices(ItemsContextForAllTables db, UserServices service)
        {
            m_db = db;
            _service = service;
        }

        public List<Order> GetOrders()
        {
            return m_db.Order.Include(OD => OD.orderDetails).ToList();
        }

        public async Task<Order> GetOrder(int Id)
        {
            var e = await m_db.Order.Where(i => i.id == Id).Include(OD=>OD.orderDetails).FirstOrDefaultAsync();//lampada

            return e;
        }

        public async Task<List<Order>> GetOrderByUserId(int id) 
        {
            
            List<Order> order = m_db.Order.Where(OU => OU.userId == id).ToList();
            return order;
        }

        public async Task <Order> AddOrder(OrderDTO order)
        {
            Order newOrder      = new Order();
            newOrder.userId     = order.userId;
            newOrder.name       = order.name;
            newOrder.email      = order.email;
            newOrder.city       = order.city;
            newOrder.postalCode = order.postalCode;
            newOrder.address    = order.address;
            newOrder.phone      = order.phone;
            newOrder.total      = order.total;
            newOrder.Create_AT  = DateTime.Now.ToLocalTime();
            newOrder.Update_AT  = DateTime.Now.ToLocalTime();
            m_db.Order.AddAsync(newOrder);
            int c =  await m_db.SaveChangesAsync();
            if (c > 0)
            {
                return newOrder;
            }
            return null;
        }

        public async Task<ResponseDTO> DeleteOrderAsync(int id)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Order OrderToDelete = await GetOrder(id);
            if (OrderToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            for (int i = 0; i < OrderToDelete.orderDetails.Count; i++)
            {
                m_db.OrderDetails.Remove(OrderToDelete.orderDetails[i]);
                m_db.SaveChangesAsync();
            }
            
            m_db.Order.Remove(OrderToDelete);
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


        public async Task<ResponseDTO> UpdateOrderAsync(int id, OrderDTO newOrder)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            Order OrderToUpdate = await GetOrder(id);
            if (OrderToUpdate == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            OrderToUpdate.userId         = newOrder.userId;
            OrderToUpdate.name           = newOrder.name;
            OrderToUpdate.email          = newOrder.email;
            OrderToUpdate.city           = newOrder.city;
            OrderToUpdate.postalCode     = newOrder.postalCode;
            OrderToUpdate.address        = newOrder.address;
            OrderToUpdate.phone          = newOrder.phone;
            OrderToUpdate.total          = newOrder.total;
            OrderToUpdate.Create_AT = OrderToUpdate.Create_AT;
            OrderToUpdate.Update_AT = DateTime.Now.ToLocalTime();
            m_db.Order.Update(OrderToUpdate);
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
