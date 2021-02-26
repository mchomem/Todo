using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Todo.Core.Models.DataBase.Repositories;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Entities;
using Todo.WebAPI.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<UserDto> GetAuthentication(string login, string password)
        {
            try
            {
                UserDto user = new UserRepository()
                    .Authenticate(new User() { Login = login, Password = password });

                if (user == null)
                    return NotFound();

                user.Token = TokenHelper.Generate(user);

                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost]
        public void Post(User user)
        {
            new UserRepository().Create(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
