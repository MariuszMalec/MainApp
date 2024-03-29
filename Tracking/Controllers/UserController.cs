﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Models;
using Tracking.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryService<User> _userService;

        public UserController(IRepositoryService<User> userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAll();
            if (!users.Any())
                return BadRequest($"Brak uzytkowników!");
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Brak uzytkownika!");
            await _userService.Insert(user);
            //return Ok($"User with id {user.Id} added");
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return BadRequest($"Brak uzytkownika!");
            return Ok(user);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(User user)
        {
            if (user == null)
                return BadRequest($"Brak uzytkownika!");
            await _userService.Update(user);
            return Ok($"User with id {user.Id} edited");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return BadRequest($"Brak uzytkownika!");
            await _userService.Delete(id);
            return Ok($"User with id {id} deleted");
        }
    }
}
