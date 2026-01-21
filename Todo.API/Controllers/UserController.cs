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
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var user = await _userService.GetAsync(id);
        var response = new ApiResponse<UserDto>(user);
        return Ok(response);
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
    public async Task<IActionResult> PostAsync([FromBody] UserInsertDto user)
    {
        await _userService.CreateAsync(user);
        return Ok();
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
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UserUpdateDto userDto)
    {
        await _userService.UpdateAsync(userDto);
        var response = new ApiResponse<string>("User updated successfully.");
        return Ok(response);
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
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePasswordDto userChangePassword)
    {
        await _userService.ChangePasswordAsync(userChangePassword);
        var reponse = new ApiResponse<string>("Password changed successfully");
        return Ok(reponse);
    }

    /// <summary>
    /// Deletes a user account.
    /// </summary>
    /// <param name="id">The identifier of the user to delete.</param>
    /// <returns>Returns success if the user is deleted.</returns>
    /// <response code="200">If the user was deleted successfully.</response>
    /// <response code="500">If an error occurs while deleting the user.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _userService.DeleteAsync(id);
        return Ok();
    }

    /// <summary>
    /// Deletes a user's profile picture.
    /// </summary>
    /// <param name="userId">The identifier of the user whose picture will be deleted.</param>
    /// <returns>Returns success if the picture is deleted.</returns>
    /// <response code="200">If the user picture was deleted successfully.</response>
    /// <response code="500">If an error occurs while deleting the picture.</response>
    [HttpDelete, Route("delete-user-picture/{userId}")]
    public async Task<IActionResult> DeleteUserPictureAsync([FromRoute] int userId)
    {
        await _userPictureService.DeleteByUserIdAsync(userId);
        return Ok();
    }
}
