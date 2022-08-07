using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGroupController : Controller
    {
        private readonly IRepository<UserGroup> userGroupRepo;
        public UserGroupController(IRepository<UserGroup> UserGroupRepo)
        {
            this.userGroupRepo = UserGroupRepo;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var userGroups = await userGroupRepo.Get();

            return Ok(userGroups);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userGroup = await userGroupRepo.Get(id);

            if (userGroup != null)
            {
                return Ok(userGroup);
            }

            return NotFound();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserGroup userGroup)
        {
            bool isCreated = await userGroupRepo.Create(userGroup);

            if (!isCreated)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserGroup userGroup)
        {
            bool result = await userGroupRepo.Update(userGroup);

            if (result == false)
            {
                BadRequest(false);
            }

            return Ok(true);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await userGroupRepo.Delete(id);

            if (result)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }
    }
}
