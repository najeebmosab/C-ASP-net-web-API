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
    public class OrderDetailsServices
    {
        private readonly ItemsContextForAllTables m_db;
        public OrderDetailsServices(ItemsContextForAllTables db)
        {
            m_db = db;
        }

        public  List<OrderDetails> GetOrderDetails()
        {
            return m_db.OrderDetails.Include(OD => OD.order).Include(OD => OD.brand).Include(OD => OD.category).ToList<OrderDetails>();
        }

        public OrderDetails GetOrderDetail(int Id)
        {
            return m_db.OrderDetails.Where(OD => OD.id == Id).Include(OD => OD.order).FirstOrDefault();//lampada
        }

        public async Task<List<OrderDetails>> GetByOrderIdAsync(int Id)
        {
            return await m_db.OrderDetails.Where(OD => OD.orderId == Id).Include(OD=>OD.order).Include(OD => OD.brand).Include(OD => OD.category).ToListAsync();//lampada
        }

        public async Task<OrderDetails> AddOrderDetailsAsync(OrderDetailsDTO orderDetails)
        {
            OrderDetails newOrderDetails = new OrderDetails();
            newOrderDetails.orderId = orderDetails.orderId;
            newOrderDetails.brandId = orderDetails.brandId;
            newOrderDetails.categoryId = orderDetails.categoryId;
            newOrderDetails.productDescription = orderDetails.productDescription;
            newOrderDetails.productImage = orderDetails.productImage;
            newOrderDetails.productName = orderDetails.productName;
            newOrderDetails.productPrice = orderDetails.productPrice;
            newOrderDetails.quantity = orderDetails.quantity;
            newOrderDetails.size = orderDetails.size;
            newOrderDetails.isOrder = orderDetails.isOrder;
            newOrderDetails.Create_AT = DateTime.Now.ToLocalTime();
            newOrderDetails.Update_AT = DateTime.Now.ToLocalTime();
            if (newOrderDetails.size == null && newOrderDetails.categoryId != 1)
            {
                newOrderDetails.size = "M";
            }
            else {
                newOrderDetails.size = "37";
            }
            m_db.OrderDetails.Add(newOrderDetails);
            int c = m_db.SaveChanges();
            if (c > 0)
            {
                return newOrderDetails;
            }
            return null;
        }

        public ResponseDTO DeleteorderDetails(int id)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            OrderDetails OrderDetailsToDelete = GetOrderDetail(id);
            if (OrderDetailsToDelete == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
        
            OrderDetailsToDelete.orderId = 0;
            m_db.OrderDetails.Remove(OrderDetailsToDelete);
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


        public ResponseDTO UpdateOrderDetails(int id, OrderDetailsDTO newOrderDetails)
        {

            // Items itemToDelete = m_db.Items.Where(item =>item.id == id).FirstOrDefault();
            OrderDetails OrderDetailsToUpdate = GetOrderDetail(id);
            if (OrderDetailsToUpdate == null)//בדיקה אם הישות קיים במאגר נתוניים
            {
                return new ResponseDTO()
                {
                    Status = StatusCode.Error,
                    StatusText = $"Entity With id:{id} not in DB"
                };
            }
            OrderDetailsToUpdate.orderId = newOrderDetails.orderId;
            OrderDetailsToUpdate.brandId = newOrderDetails.brandId;
            OrderDetailsToUpdate.categoryId = newOrderDetails.categoryId;
            OrderDetailsToUpdate.isOrder = newOrderDetails.isOrder;
            OrderDetailsToUpdate.productDescription = newOrderDetails.productDescription;
            OrderDetailsToUpdate.productImage = newOrderDetails.productImage;
            OrderDetailsToUpdate.productName = newOrderDetails.productName;
            OrderDetailsToUpdate.productPrice = newOrderDetails.productPrice;
            OrderDetailsToUpdate.quantity = newOrderDetails.quantity;
            OrderDetailsToUpdate.size = newOrderDetails.size;
            

            OrderDetailsToUpdate.Create_AT = OrderDetailsToUpdate.Create_AT;
            OrderDetailsToUpdate.Update_AT = DateTime.Now.ToLocalTime();
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
