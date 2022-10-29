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
    public class ImagesController : ControllerBase
    {
        private readonly ImagesServices _services;
        public ImagesController(ImagesServices services) // בנאי
        {
            _services = services;
        }

        [Route("{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        public ActionResult getData(int id = 0)
        {
            if (id < 1)
            {
                List<Images> result = _services.GetImages();// מחזיר איסם אחד
                return Ok(result);
            }
            Images result2 = _services.GetImage(id);
            return Ok(result2);
        }

        [Route("{GetImagesByProductId}")]
        [HttpPost]
        public async Task<ActionResult> GetImagesByProductId([FromBody] int Id) 
        {
            List<Images> result2 = await _services.GetImagesByProductId(Id);
            return Ok(result2);
        }


        [HttpPost]
        public async Task<ActionResult> Add()//DTO = data transfer object
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < HttpContext.Request.Form.Files.Count; i++)
            {
                images.Add(HttpContext.Request.Form.Files[i]);
            }

            int id = int.Parse(HttpContext.Request.Form["productId"]);

            bool ok = await _services.AddImages(images, id);
            if (ok)
            {
                return Created("", null);
            }
            return BadRequest();
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < HttpContext.Request.Form.Files.Count; i++)
            {
                images.Add(HttpContext.Request.Form.Files[i]);
            }

            ResponseDTO res = await _services.UpdateImagesAsync(id,images);
            return Ok(res);
        }


        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ResponseDTO res = _services.DeleteImages(id);
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
