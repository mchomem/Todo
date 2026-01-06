namespace Todo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("authentication")]
    public async Task<ActionResult<UserDto>> GetAuthenticationAsync([FromQuery] string login, [FromQuery] string password)
    {
        try
        {
            var user = await _authService.AuthenticateAsync(login, password);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
