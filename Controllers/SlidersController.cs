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
    public class SlidersController : ControllerBase
    {
        public readonly SlidersServices _services;
        public SlidersController(SlidersServices services) // בנאי
        {
            _services = services;
        }

        [Route("{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        public ActionResult getData(int id = 0)
        {
            if (id < 1)
            {
                List<Sliders> result = _services.GetSliders();// מחזיר איסם אחד
                return Ok(result);
            }
            Sliders result2 = _services.GetSlider(id);
            return Ok(result2);
        }

        [HttpPost]
        public async Task<ActionResult> Add()//DTO = data transfer object
        {
            Sliders ok = null;
            SlidersDTO Slider = new SlidersDTO();
            Slider.heading = HttpContext.Request.Form["heading"];
            Slider.description = HttpContext.Request.Form["description"];
            Slider.linke = HttpContext.Request.Form["linke"];
            Slider.linkeName = HttpContext.Request.Form["linkeName"];
            var img = HttpContext.Request.Form.Files["image"];
            var status = HttpContext.Request.Form["status"];
            switch (status)
            {
                case "true":
                    Slider.status = true;
                    break;
                case "false":
                    Slider.status = false;
                    break;
            }

            if ( img != null && img.Length > 0)
            { 
                 ok = await _services.AddSliders(Slider,img);
            }
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
            ResponseDTO ok = null;
            SlidersDTO Slider = new SlidersDTO();
            Slider.heading = HttpContext.Request.Form["heading"];
            Slider.description = HttpContext.Request.Form["description"];
            Slider.linke = HttpContext.Request.Form["linke"];
            Slider.linkeName = HttpContext.Request.Form["linkeName"];
            var img = HttpContext.Request.Form.Files["image"];
            var status = HttpContext.Request.Form["status"];
            switch (status)
            {
                case "true":
                    Slider.status = true;
                    break;
                case "false":
                    Slider.status = false;
                    break;
            }
            if (img != null && img.Length > 0)
            {
                ok = await _services.UpdateSlidersAsync(id, Slider, img);
            }
            else 
            {
                ok = await _services.UpdateSlidersAsync(id, Slider, img);
            }
            if (ok != null)
            {
                return Created("", ok);
            }
            return BadRequest();
        }


        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ResponseDTO res = _services.DeleteSliders(id);
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
