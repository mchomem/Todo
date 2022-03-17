using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IUserPictureRepository _userPictureRepository;

        public UserController(IUserRepository userRepository, IUserPictureRepository userPictureRepository)
        {
            _userRepository = userRepository;
            _userPictureRepository = userPictureRepository;
        }

        // GET: api/<UserController>
        [AllowAnonymous]
        [HttpGet]
        [Route("authentication")]
        public async Task<ActionResult<UserDto>> GetAuthentication(string login, string password)
        {
            try
            {
                UserDto user = (UserDto)await _userRepository
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
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            try
            {
                User user = (User)await _userRepository
                    .Details(new User() { UserID = id });

                UserDto userDto = new UserDto()
                {
                    UserID = user.UserID.Value,
                    Name = user.Name,
                    Picture = user.Picture?.Picture,
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
        public async Task<ActionResult> Post(User user)
        {
            try
            {
                if (((IEnumerable<User>)await _userRepository.Retrieve(new User() { Login = user.Login })).Any())
                    throw new Exception("This user is already being used");

                await _userRepository.Create(user);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UserDto userDto)
        {
            try
            {
                User userUpdate = (User)await _userRepository
                    .Details(new User() { UserID = id });

                if (userUpdate == null)
                    return NotFound();

                UserPicture userPicture;

                if (userUpdate.Picture != null)
                {
                    userPicture =
                        await _userPictureRepository
                            .Details(new UserPicture() { UserPictureID = userUpdate.Picture.UserPictureID });
                }
                else
                {
                    userPicture = new UserPicture()
                    {
                        PictureFromUserID = userUpdate.UserID,
                        User = userUpdate
                    };
                }

                userPicture.Picture = userDto.Picture;

                // Do already exists the user picture?
                if (userUpdate.Picture == null)
                {
                    await _userPictureRepository.Create(userPicture);
                }
                else
                {
                    await _userPictureRepository.Update(userPicture);
                }

                userUpdate.Name = userDto.Name;
                await _userRepository.Update(userUpdate);

                return StatusCode(204);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            try
            {
                await _userRepository
                    .ChangePassword(new User()
                    {
                        UserID = userId
                        ,
                        Password = currentPassword
                    }, newPassword);

                return StatusCode(204, new { message = "Password changed successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }

        [HttpDelete, Route("delete-user-picture")]
        public async Task<ActionResult> DeleteUserPicture(int userId)
        {
            try
            {
                User user = (User)await _userRepository.Details(new User() { UserID = userId });

                if (user == null)
                    return NotFound("User not found.");

                UserPicture userPicture = (UserPicture)await _userPictureRepository
                    .Details(new UserPicture() { PictureFromUserID = userId });

                if (userPicture == null)
                    return NotFound("User picture not found.");

                await _userPictureRepository
                    .Delete(userPicture);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
