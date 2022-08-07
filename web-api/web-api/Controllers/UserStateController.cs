using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserStateController : Controller
    {
        private readonly IRepository<UserState> userStateRepo;
        public UserStateController(IRepository<UserState> UserStateRepo)
        {
            this.userStateRepo = UserStateRepo;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var userStates = await userStateRepo.Get();

            return Ok(userStates);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userState = await userStateRepo.Get(id);

            if (userState != null)
            {
                return Ok(userState);
            }

            return NotFound();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserState userState)
        {
            bool isCreated = await userStateRepo.Create(userState);

            if (!isCreated)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserState userState)
        {
            bool result = await userStateRepo.Update(userState);

            if (result == false)
            {
                BadRequest(false);
            }

            return Ok(true);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await userStateRepo.Delete(id);

            if (result)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }
    }
}
