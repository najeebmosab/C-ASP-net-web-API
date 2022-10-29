using FinalProject.Datas;
using FinalProject.Datas.DTO;
using FinalProject.Datas.DTO.AuthRequests;
using FinalProject.Datas.Edintites;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customers")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _services;
        private readonly Jwt _Jwtservices;
        private readonly ItemsContextForAllTables db;
        public UserController(UserServices services,Jwt DB_Jwtservices, ItemsContextForAllTables _db) // בנאי
        {
            _services = services;
            _Jwtservices = DB_Jwtservices;
            db = _db;
        }

        //[Route("{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        [AllowAnonymous]
        public async Task<ActionResult> getDataAsync()//int id = 0
        {

            int id = int.Parse(_Jwtservices.GetTokenClaims());
            if (id < 1)
            {
                List<Users> result =await _services.GetUsers();// מחזיר איסם אחד
                return Ok(result);
            }
            
            Users result2 = await _services.GetUser(id);
            return Ok(result2);
        }

        [Route("{id?}")]//מקבל נתונים עם פרמטר מסויים
        [HttpGet] // מקבל נתונים
        [AllowAnonymous]
        public async Task<ActionResult> getUserById(int id)//int id = 0
        {
            Users result2 = await _services.GetUser(id);
            return Ok(result2);
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

                string jwtToken = _Jwtservices.GenerateToken(userFound.id.ToString(),userFound.isAdmin);
                //if (_Jwtservices.GenerateToken(userFound.UserId.ToString()) != null)
                //{
                //    return Ok(getData(userFound.UserId));
                //}
                //string s1 = _Jwtservices.GetTokenClaims();
                return  Ok(jwtToken);

            }
            return Unauthorized("Not Found");//for request 401 user Not Found 
        }

        [Route("CheckEmail/{E?}")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CheckEmailAsync(UsersDTO E)
      {
            var u =  db.Users.Where(U => U.email == E.email.ToString()).FirstOrDefault();
            if (u != null)
            {
                return Ok(u);
            }
            return Ok(null);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Add()//DTO = data transfer object
        {
            UsersDTO User = new UsersDTO();
            User.userName = HttpContext.Request.Form["userName"];
            User.email = HttpContext.Request.Form["email"];
            User.password = HttpContext.Request.Form["password"];
            User.code = HttpContext.Request.Form["code"];
            User.gender = bool.Parse(HttpContext.Request.Form["gender"]);
            User.isAdmin = bool.Parse(HttpContext.Request.Form["isAdmin"]);
            User.isEmail = bool.Parse(HttpContext.Request.Form["isEmail"]);
            User.userImage = HttpContext.Request.Form["userImage"];

            Users ok = await _services.AddUser(User);
            if (ok != null)
            {
                return Created("", ok);
            }
            return BadRequest();
        }


        [Route("{id}")]
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateAsync(int id)
        {
            UsersDTO User = new UsersDTO();
            User.userName = HttpContext.Request.Form["userName"];
            User.email = HttpContext.Request.Form["email"];
            User.password = HttpContext.Request.Form["password"];
            User.gender = bool.Parse(HttpContext.Request.Form["gender"]);
            User.isAdmin = bool.Parse(HttpContext.Request.Form["isAdmin"]);
            var img = HttpContext.Request.Form.Files["userImage"];

            ResponseDTO res =await _services.UpdateItemAsync(id, User,img);
            return Ok(res);
        }

        [Route("{id}")]
        [HttpDelete]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            ResponseDTO res =await _services.DeleteItemAsync(id);
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
