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
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailsServices _services;
        public OrderDetailsController(OrderDetailsServices services) // בנאי
        {
            _services = services;
        }

        [Route("{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        public ActionResult getData(int id = 0)
        {
            if (id < 1)
            {
                List<OrderDetails> result = _services.GetOrderDetails();// מחזיר איסם אחד
                return Ok(result);
            }
            OrderDetails result2 = _services.GetOrderDetail(id);
            return Ok(result2);
        }

        [Route("{GetByOrderId}/{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        public async Task<ActionResult> GetByOrderIdAsync(int id)
        {
            List<OrderDetails> result2 = await _services.GetByOrderIdAsync(id);
            return Ok(result2);
        }


        [HttpPost]
        public async Task <ActionResult> Add([FromBody] OrderDetailsDTO OrderDetails)//DTO = data transfer object
        {
            OrderDetails ok = await _services.AddOrderDetailsAsync(OrderDetails);
            if (ok != null)
            {
                return Ok(ok);
            }
            return BadRequest();
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody] OrderDetailsDTO OrderDetails)
        {
            ResponseDTO res = _services.UpdateOrderDetails(id, OrderDetails);
            return Ok(res);
        }


        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ResponseDTO res = _services.DeleteorderDetails(id);
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
