using FinalProject.Datas.DTO;
using FinalProject.Datas.DTO.AuthRequests;
using FinalProject.Datas.Edintites;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserServices _services;
        private readonly Jwt _Jwtservices;
        private readonly ProductsServices _productsServices;
        public AdminController(UserServices services, Jwt DB_Jwtservices, ProductsServices productsServices) // בנאי
        {
            _services = services;
            _Jwtservices = DB_Jwtservices;
            _productsServices = productsServices;
        }

        public async Task<ActionResult> getDataAsync()//int id = 0
        {

            int id = int.Parse(_Jwtservices.GetTokenClaims());
            Users result2 = await _services.GetUser(id);
            return Ok(result2);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult> GetAllProductsForAdmin()//int id = 0
        {
            List<Products> result = await _productsServices.GetAllProductsForAdmin();// מחזיר איסם אחד
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> getAllUsersAsync()//int id = 0
        {
            List<Users> result = await _services.GetUsers();// מחזיר איסם אחד
            return Ok(result);
        }

        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public IActionResult Auth([FromBody] AuthRequest request)
        {

            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest("you Must Enter User Name and Password");
            }
            Users userFound = _services.PostUser(request.email, request.password);

            if (userFound != null)
            {

                string jwtToken = _Jwtservices.GenerateToken(userFound.id.ToString(), userFound.isAdmin);
                //if (_Jwtservices.GenerateToken(userFound.UserId.ToString()) != null)
                //{
                //    return Ok(getData(userFound.UserId));
                //}
                //string s1 = _Jwtservices.GetTokenClaims();
                return Ok(jwtToken);

            }
            return Unauthorized("Not Found");//for request 401 user Not Found 
        }
    }
}
