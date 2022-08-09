using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuth auth;
        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }

        [HttpPost, Route("SignIn")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Invalid client request");
            }

            await auth.SignIn(loginModel);

            return Ok();
        }

        [HttpPost, Route("SignOut")]
        public async Task<IActionResult> SignOut([FromBody] string login)
        {
            bool res = await auth.SignOut(login);

            if (!res)
            {
                return BadRequest();
            }

            return Unauthorized("You have been unauthorized");
        }
    }
}
