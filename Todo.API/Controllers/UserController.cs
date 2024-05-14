namespace Todo.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserPictureService _userPictureService;

    public UserController(IUserService userRepository, IUserPictureService userPictureRepository)
    {
        _userService = userRepository;
        _userPictureService = userPictureRepository;
    }

    // GET: api/<UserController>
    [AllowAnonymous]
    [HttpGet]
    [Route("authentication")]
    public async Task<ActionResult<UserDto>> GetAuthentication(string login, string password)
    {
        try
        {
            UserDto user = await _userService.AuthenticateAsync(login, password);

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
            User user = await _userService.DetailsAsync(new User() { UserID = id });

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
            if ((await _userService.RetrieveAsync(new User() { Login = user.Login })).Any())
                throw new Exception("This user is already being used");

            await _userService.CreateAsync(user);
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
            User userUpdate = await _userService.DetailsAsync(new User() { UserID = id });

            if (userUpdate == null)
                return NotFound();

            UserPicture userPicture;

            if (userUpdate.Picture != null)
            {
                userPicture = await _userPictureService.DetailsAsync(new UserPicture() { UserPictureID = userUpdate.Picture.UserPictureID });
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
                await _userPictureService.CreateAsync(userPicture);
            }
            else
            {
                await _userPictureService.UpdateAsync(userPicture);
            }

            userUpdate.Name = userDto.Name;
            userUpdate.Password = CypherHelper.Decrypt(userUpdate.Password);

            await _userService.UpdateAsync(userUpdate);

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
            await _userService
                .ChangePasswordAsync(new User()
                {
                    UserID = userId,
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
            User user = await _userService.DetailsAsync(new User() { UserID = userId });

            if (user == null)
                return NotFound("User not found.");

            UserPicture userPicture = await _userPictureService.DetailsAsync(new UserPicture() { PictureFromUserID = userId });

            if (userPicture == null)
                return NotFound("User picture not found.");

            await _userPictureService.DeleteAsync(userPicture);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
