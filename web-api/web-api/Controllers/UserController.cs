using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using web_api.Enums;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IRepository<User> userRepo;
        public UserController(IRepository<User> UserRepo)
        {
            this.userRepo = UserRepo;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var users = await userRepo.Get();

            return Ok(users);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await userRepo.Get(id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            bool isCreated = await userRepo.Create(user);

            if (!isCreated)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            bool result = await userRepo.Update(user);

            if (!result)
            {
                BadRequest(false);
            }

            return Ok(true);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await userRepo.Delete(id);

            if (result)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }
    }
}
