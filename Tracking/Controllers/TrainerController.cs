using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Models;
using Tracking.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly IRepositoryService<Trainer> _userService;

        public TrainerController(IRepositoryService<Trainer> userService)
        {
            _userService = userService;
        }

        //TODO from route email is send as authorize
        [HttpGet("{email}/{password}")]
        public async Task<IActionResult> Get([FromRoute] string email, string password)
        {
            var user = await _userService.Authenticate(email);

            if (user == null)
                return Content("401 Not authorize!");

            var users = await _userService.GetAll();
            if (!users.Any())
                return NotFound($"404 Brak uzytkowników!");
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAll();
            if (!users.Any())
                return NotFound($"Brak uzytkowników!");
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Trainer user)
        {
            if (user == null)
                return NotFound("404 Brak uzytkownika!");
            await _userService.Insert(user);
            //return Ok($"User with id {user.Id} added");
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainer(int id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return NotFound($"404 Brak uzytkownika!");
            return Ok(user);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(Trainer user)
        {
            if (user == null)
                return BadRequest($"404 Brak uzytkownika!");
            await _userService.Update(user);
            return Ok($"User with id {user.Id} edited");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return NotFound($"404 Brak uzytkownika!");
            await _userService.Delete(id);
            return Ok($"User with id {id} deleted");
        }

    }
}
