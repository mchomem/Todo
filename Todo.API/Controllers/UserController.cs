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

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetAsync([FromRoute] int id)
    {
        try
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] UserInsertDto user)
    {
        try
        {
            await _userService.CreateAsync(user);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync([FromRoute] int id, [FromBody] UserUpdateDto userDto)
    {
        try
        {
            await _userService.UpdateAsync(userDto);
            return StatusCode(204);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPut]
    [Route("change-password")]
    public async Task<ActionResult> ChangePasswordAsync([FromBody] UserChangePasswordDto userChangePassword)
    {
        try
        {
            await _userService.ChangePasswordAsync(userChangePassword);
            return StatusCode(204, new { message = "Password changed successfully" });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpDelete, Route("delete-user-picture/{userId}")]
    public async Task<ActionResult> DeleteUserPictureAsync([FromRoute] int userId)
    {
        try
        {
            await _userPictureService.DeleteByUserIdAsync(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
