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
    public class CartController : ControllerBase
    {
        private readonly CartServices _sercives;
        public CartController(CartServices services)
        {
            _sercives = services;
        }

        [Route("{Id?}")]//defult
        [HttpGet]
        public async Task<ActionResult> GetItems(int Id = 0)
        {
            if (Id < 1)
            {
                List<Cart> result = await _sercives.GetCarts();
                return Ok(result);
            }
            else
            {
                Cart result2 = _sercives.GetCart(Id);
                return Ok(result2);
            }
        }

        [Route("{usersid?}/{id?}")]
        [HttpGet]
        public async Task<ActionResult<Cart>> GetCartByUserId( int id ) 
        {
            Cart result2 = await _sercives.GetCartByUserIdAsync(id);
            return Ok(result2);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> Add([FromBody] CartDTO Cart)
        {
            Cart ok = await _sercives.AddCart(Cart);
            if (ok != null)
            {
                return Created("",ok);
            }
            return BadRequest();

        }
        [Route("{id}")]
        [HttpPut]
        public ActionResult Update([FromBody] CartDTO Cart, int id)
        {
            ResponseDTO res = _sercives.UpdetCart(id, Cart);
            return Ok(res);
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult delete(int id)
        {
            ResponseDTO res = _sercives.DeleteCart(id);
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
