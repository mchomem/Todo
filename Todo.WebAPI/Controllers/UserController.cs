using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
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
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/<UserController>

        [AllowAnonymous]
        [HttpGet]
        [Route("authentication")]
        public ActionResult<UserDto> GetAuthentication(string login, string password)
        {
            try
            {
                UserDto user = _userRepository
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
        public ActionResult<UserDto> Get(int id)
        {
            try
            {
                User user = _userRepository
                    .Details(new User() { UserID = id });

                UserDto userDto = new UserDto()
                {
                    UserID = user.UserID.Value,
                    Name = user.Name,
                    IsActive = user.IsActive.Value
                };

                return Ok(userDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Post(User user)
        {
            try
            {
                _userRepository.Create(user);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, User user)
        {
            try
            {
                User userUpdate = _userRepository
                    .Details(new User() { UserID = id });

                if (userUpdate == null)
                    return NotFound();

                _userRepository.Update(user);

                return StatusCode(204);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        [Route("password")]
        public ActionResult ChangePassword(int userId, string currentPassword, string newPassword)
        {
            try
            {
                _userRepository.ChangePassword(new User() { UserID = userId, Password = currentPassword }, newPassword);

                return StatusCode(204, new { message = "Password changed successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
