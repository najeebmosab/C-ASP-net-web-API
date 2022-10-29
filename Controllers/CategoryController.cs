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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryServices _sercives;
        public CategoryController(CategoryServices services)
        {
            _sercives = services;
        }

        [Route("{Id?}")]//defult
        [HttpGet]
        public async Task <ActionResult> GetItems(int Id = 0)
        {
            if (Id < 1)
            {
                List<Category> result = await  _sercives.GetCategorys();
                return Ok(result);
            }
            else
            {
                Category result2 =  _sercives.GetCategory(Id);
                return Ok(result2);
            }
        }


        /*public ActionResult GetItemsById(int Id)
        {
            Items result = _sercives.GetItem(Id);
            return Ok(result);
        }*/

        [HttpPost]
        public async Task <ActionResult> Add()
        {
            CategoryDTO Category = new CategoryDTO();
            Category.categoryName = HttpContext.Request.Form["categoryName"];
            var img = HttpContext.Request.Form.Files["picture"];
            bool ok = await _sercives.AddCategory(Category,img);
            if (ok)
            {
                return Created("", ok);
            }
            return BadRequest();

        }
        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id)
        {
            CategoryDTO Category = new CategoryDTO();
            Category.categoryName = HttpContext.Request.Form["categoryName"];
            var img = HttpContext.Request.Form.Files["picture"];
            ResponseDTO res = await _sercives.UpdetCategoryAsync(id, Category,img);
            return Ok(res);
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult delete(int id)
        {
            ResponseDTO res = _sercives.DeleteCategory(id);
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
        public ActionResult delete(int deleteId,int changeId)
        {
            ResponseDTO res = _sercives.DeleteCategory(deleteId, changeId);
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
