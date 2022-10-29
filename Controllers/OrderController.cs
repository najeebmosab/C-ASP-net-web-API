using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using FinalProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServices _services;
        public OrderController(OrderServices services) // בנאי
        {
            _services = services;
        }

        [Route("{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        public async Task<ActionResult> getDataAsync(int id = 0)
        {
            if (id < 1)
            {
                List<Order> result = _services.GetOrders();// מחזיר איסם אחד
                return Ok(result);
            }
            Order result2 = await _services.GetOrder(id);
            return Ok(result2);
        }

        [Route("{GetOrderByUserId}/{userId}")]
        [HttpGet]
        public async Task<ActionResult> GetOrderByUserId(int userId)//DTO = data transfer object
        {
            List<Order> ok = await _services.GetOrderByUserId(userId);
            if (ok != null)
            {
                return Ok(ok);
            }
            return BadRequest();
        }

        [HttpPost]
        public async  Task<ActionResult> Add([FromBody] OrderDTO Order)//DTO = data transfer object
        {
            Order ok = await _services.AddOrder(Order);
            if (ok != null)
            {
                return Ok(ok);
            }
            return BadRequest();
        }


        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] OrderDTO Order)
        {
            ResponseDTO res = await _services.UpdateOrderAsync(id, Order);
            return Ok(res);
        }


        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            ResponseDTO res = await _services.DeleteOrderAsync(id);
            if (res.Status == Datas.DTO.StatusCode.Error)
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }
    }
}
