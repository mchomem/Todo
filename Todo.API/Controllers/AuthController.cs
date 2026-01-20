namespace Todo.API.Controllers;

/// <summary>
/// Controller responsible for handling user authentication operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">The authentication service.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="login">The user's login identifier.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>Returns the authenticated user information if successful.</returns>
    /// <response code="200">Returns the authenticated user data.</response>
    /// <response code="500">If an error occurs during authentication.</response>
    [AllowAnonymous]
    [HttpGet]
    [Route("authentication")]
    public async Task<IActionResult> GetAuthenticationAsync([FromQuery] string login, [FromQuery] string password)
    {
        var user = await _authService.AuthenticateAsync(login, password);
        var response = new ApiResponse<UserDto>(user);
        return Ok(response);
    }
}
