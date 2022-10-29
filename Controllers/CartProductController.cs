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
    public class CartProductController : ControllerBase
    {
        private readonly CartProductService _sercives;
        public CartProductController(CartProductService services)
        {
            _sercives = services;
        }

        [Route("{id?}")]
        [HttpGet]
        public async Task<ActionResult> GetCartProduct(int Id)
        {
            CartProduct result2 = await _sercives.GetCartProduct(Id);
            return Ok(result2);
        }

        [Route("{GetProductByCartId}")]
        [HttpPost]
        public async Task<ActionResult> GetItems([FromBody] int Id)
        {    
                List<CartProduct> result2 = await _sercives.GetCartProducts(Id);
                return Ok(result2);   
        }

        [HttpPost]
        public async Task<ActionResult<CartProduct>> Add([FromBody] CartProductDTO cartProduct)
        {
            CartProduct ok = await _sercives.AddCart(cartProduct);
            if (ok != null)
            {
                return Created("", ok);
            }
            return BadRequest();
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody]CartProductDTO obj,int id )
        {
            
            ResponseDTO res = await _sercives.UpdeteAsync(id, obj);
            return Ok(res);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> delete(int id)
        {
            ResponseDTO res = await _sercives.DeleteItem(id);
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
