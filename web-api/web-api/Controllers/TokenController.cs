using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;
using web_api.Repositories;

namespace web_api.Controllers
{
    public class TokenController : Controller
    {
        private readonly IJwtToken tokenService;
        public TokenController(IJwtToken tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpPost, Route("Refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }

            AuthenticatedResponse authenticatedResponse = await tokenService.Refresh(tokenApiModel);

            return Ok(authenticatedResponse);
        }

        [HttpPost, Authorize, Route("Revoke")]
        public async Task<IActionResult> Revoke(string login)
        {
            bool res = await tokenService.Revoke(login);

            if (!res)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
