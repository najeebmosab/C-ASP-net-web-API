using FinalProject.Datas;
using FinalProject.Datas.DTO;
using FinalProject.Datas.DTO.AuthRequests;
using FinalProject.Datas.Edintites;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly ProductsServices _sercives;
        private readonly ImagesServices _imageService;

        private readonly ItemsContextForAllTables _context;
        public ProductsController(ProductsServices services, ItemsContextForAllTables context, ImagesServices imageService)
        {
            _sercives = services;
            _context = context;
            _imageService = imageService;
        }

        [HttpGet("ByCatagoryId/{catId}")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProductsByCatagoryId(int catId)
        {
            return await _context.Product.Where(p => p.categoryId == catId).ToListAsync();
        }
        [HttpGet("ByCatagoryIdAndBrandId/{catId}/{brnId}")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProductsByCatagoryIdAndBrand(int catId,int brnId)
        {
            return await _context.Product.Where(p => p.categoryId == catId && p.brandId == brnId).ToListAsync();
        }

        [Route("{Id?}")]//defult
        [HttpGet]
        public async Task<ActionResult> GetItem(int Id = 0)
        {
            if (Id < 1)
            {
                List<ProductsDTO> result = await _sercives.GetProducts();
                return Ok(result);
            }
            else
            {
                Products result2 = await _sercives.GetProduct(Id);
                return Ok(result2);
            }
        }

        [Route("DisCount")]//defult
        [HttpGet]
        public async Task<ActionResult> GetDisCountProducts()
        {
            List<Products> P = await _sercives.GetDisCountProducts();
            return Ok(P);
        }

        [Route("NewProducts")]//defult
        [HttpGet]
        public async Task<ActionResult> GetNewProducts()
        {
            List<Products> P = await _sercives.GetnewProduct();
            return Ok(P);
        }

        [Route("{GetByName}/{productName}")]//defult
        [HttpGet]
        public async Task<ActionResult> GetProductsByName(string productName)
        {
           List<Products> P = await _sercives.GetProductsName(productName);
            return Ok(P);
        }


        /*public ActionResult GetItemsById(int Id)
        {
            Items result = _sercives.GetItem(Id);
            return Ok(result);
        }*/

        [HttpPost]
        public async Task<ActionResult> Add()
        {
            ProductsDTO productsDTO = new ProductsDTO();
            productsDTO.categoryId = int.Parse(HttpContext.Request.Form["categoryId"]);
            productsDTO.brandId = int.Parse(HttpContext.Request.Form["brandId"]);
            productsDTO.productName = HttpContext.Request.Form["productName"];
            productsDTO.price = int.Parse(HttpContext.Request.Form["price"]);
            productsDTO.salePrice = int.Parse(HttpContext.Request.Form["salePrice"]);
            productsDTO.quantity = HttpContext.Request.Form["quantity"];
            productsDTO.shortDescription = HttpContext.Request.Form["shortDescription"];
            productsDTO.description = HttpContext.Request.Form["description"];
            productsDTO.actualPrice = int.Parse(HttpContext.Request.Form["actualPrice"]);
            productsDTO.gender = HttpContext.Request.Form["gender"];
            var img  = HttpContext.Request.Form.Files["imageProduct"];
            
            if (img != null && img.Length > 0)
            {
                Products ok = await _sercives.AddProducts(productsDTO,img);
                if (ok != null)
                {
                    return Created("", ok);
                }
            }
            
            return BadRequest();

        }


        [Route("{id}")]
        [HttpPut]
        public  async Task <ActionResult> Update(int id)
        {
            ProductsDTO productsDTO = new ProductsDTO();
            productsDTO.categoryId = int.Parse(HttpContext.Request.Form["categoryId"]);
            productsDTO.brandId = int.Parse(HttpContext.Request.Form["brandId"]);
            productsDTO.productName = HttpContext.Request.Form["productName"];
            productsDTO.price = int.Parse(HttpContext.Request.Form["price"]);
            productsDTO.salePrice = int.Parse(HttpContext.Request.Form["salePrice"]);
            productsDTO.quantity = HttpContext.Request.Form["quantity"];
            productsDTO.shortDescription = HttpContext.Request.Form["shortDescription"];
            productsDTO.description = HttpContext.Request.Form["description"];
            productsDTO.actualPrice = int.Parse(HttpContext.Request.Form["actualPrice"]);
            productsDTO.gender = HttpContext.Request.Form["gender"];
            var img = HttpContext.Request.Form.Files["imageProduct"];
            if (img == null)
            {
                productsDTO.imageProduct = HttpContext.Request.Form["imageProduct"];
            }

            ResponseDTO ok = await _sercives.UpdateProducts(id,productsDTO,img);
            if (ok != null)
            {
                return Created("", ok);
            }
            
            return BadRequest();

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task <ActionResult> delete(int id)
        {
            ResponseDTO res =await _sercives.DeleteProducts(id);
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
