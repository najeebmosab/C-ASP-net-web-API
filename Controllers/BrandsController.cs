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
    public class BrandsController : ControllerBase
    {
        private readonly BrandServices _sercives;
        public BrandsController(BrandServices services)
        {
            _sercives = services;
        }

        [Route("{Id?}")]//defult
        [HttpGet]
        public async Task<ActionResult> GetItems(int Id = 0)
        {
            if (Id < 1)
            {
                List<Brands> result = await _sercives.GetBrands();
                return Ok(result);
            }
            else
            {
                Brands result2 = await _sercives.GetBrand(Id);
                return Ok(result2);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Brands>> Add()
        {
            BrandsDTO brand = new BrandsDTO();
            brand.name = HttpContext.Request.Form["name"];
            var img = HttpContext.Request.Form.Files["image"];
            Brands ok = await _sercives.AddBrands(brand,img);
            if (ok != null)
            {
                return Created("", ok);
            }
            return BadRequest();

        }

        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id)
        {
            BrandsDTO brand = new BrandsDTO();
            brand.name = HttpContext.Request.Form["name"];
            var img = HttpContext.Request.Form.Files["image"];
            ResponseDTO res = await _sercives.UpdateBrands(id, brand,img);
            return Ok(res);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> deleteAsync(int id)
        {
            ResponseDTO res = await _sercives.DeleteBrands(id);
            if (res.Status == Datas.DTO.StatusCode.Error)
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }

        [Route("{withoutProduct}/{deleteId}/{changeId}")]
        [HttpDelete]
        public async Task<ActionResult> deleteAsync(int deleteId, int changeId)
        {
            ResponseDTO res = await _sercives.DeleteBrandAsync(deleteId, changeId);
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
