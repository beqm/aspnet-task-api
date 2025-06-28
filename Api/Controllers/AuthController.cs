using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.User.LoginUser;
using Application.Commands.User.RegisterUser;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public AuthController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        if (userId is null)
        {
            return BadRequest("Email já cadastrado.");
        }

        return Ok(new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var token = await _mediator.Send(command);
        if (token is null)
        {
            return Unauthorized("Email ou senha inválidos");
        }

        return Ok(new { token });
    }
}
