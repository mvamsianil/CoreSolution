using APIApplication.Services;
using APIEntity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIApplication.Controllers
{
    /// <summary>
    /// UserController
    /// </summary>
    [ApiController, Route("coreapi/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        /// <summary>
        /// UserController
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("authenticate"), AllowAnonymous]
        public IActionResult GetToken(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns></returns>       
        [HttpGet, Route("getallusers")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("validatetoken"), AllowAnonymous]
        public IActionResult ValidateToken(string token) {
            bool response = _userService.ValidateToken(token);

            if (!response)
                return Unauthorized(new { message = "Token Invalid" });

            return Ok(response);
        }
    }
}
