namespace Todo.API.Controllers;

/// <summary>
/// Controller responsible for managing user operations including account management, profile updates, and password changes.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserPictureService _userPictureService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userRepository">The user service.</param>
    /// <param name="userPictureRepository">The user picture service.</param>
    public UserController(IUserService userRepository, IUserPictureService userPictureRepository)
    {
        _userService = userRepository;
        _userPictureService = userPictureRepository;
    }

    /// <summary>
    /// Retrieves a user by their identifier.
    /// </summary>
    /// <param name="id">The identifier of the user.</param>
    /// <returns>Returns the user information if found.</returns>
    /// <response code="200">Returns the user data.</response>
    /// <response code="500">If an error occurs while retrieving the user.</response>
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

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="user">The user data to create.</param>
    /// <returns>Returns success if the user is created.</returns>
    /// <response code="200">If the user was created successfully.</response>
    /// <response code="500">If an error occurs while creating the user.</response>
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

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="id">The identifier of the user to update.</param>
    /// <param name="userDto">The updated user data.</param>
    /// <returns>Returns no content if the update is successful.</returns>
    /// <response code="204">If the user was updated successfully.</response>
    /// <response code="500">If an error occurs while updating the user.</response>
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

    /// <summary>
    /// Changes a user's password.
    /// </summary>
    /// <param name="userChangePassword">The password change data containing user identifier and new password.</param>
    /// <returns>Returns a success message if the password is changed.</returns>
    /// <response code="204">If the password was changed successfully.</response>
    /// <response code="500">If an error occurs while changing the password.</response>
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

    /// <summary>
    /// Deletes a user account.
    /// </summary>
    /// <param name="id">The identifier of the user to delete.</param>
    /// <returns>Returns success if the user is deleted.</returns>
    /// <response code="200">If the user was deleted successfully.</response>
    /// <response code="500">If an error occurs while deleting the user.</response>
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

    /// <summary>
    /// Deletes a user's profile picture.
    /// </summary>
    /// <param name="userId">The identifier of the user whose picture will be deleted.</param>
    /// <returns>Returns success if the picture is deleted.</returns>
    /// <response code="200">If the user picture was deleted successfully.</response>
    /// <response code="500">If an error occurs while deleting the picture.</response>
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
