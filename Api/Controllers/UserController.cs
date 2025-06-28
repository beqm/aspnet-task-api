using MediatR;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using TaskModel = Domain.Models.Task;
using Microsoft.AspNetCore.Authorization;
using Application.Commands.User.DeleteUser;
using Application.Queries.User.GetUserById;
using Application.Queries.User.GetUsersRange;
using Application.Commands.User.ChangePassword;
using Application.Queries.User.GetUserWithTasks;


namespace Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/user")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetUsersRangeQuery(page, pageSize);
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        if (role != "admin" && userId != id)
            return Forbid();

        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("{id}/password")]
    [ProducesResponseType(204)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordCommand command)
    {
        var userId = GetUserId();

        if (userId != id)
            return Forbid();

        command = command with { Id = id };

        var result = await _mediator.Send(command);
        if (result == Guid.Empty)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();

        if (userId != id)
        {
            return Forbid();
        }

        var command = new DeleteUserCommand(id);
        await _mediator.Send(command);

        return Ok();
    }

    [Authorize]
    [HttpGet("{id}/tasks")]
    [ProducesResponseType(typeof(IEnumerable<TaskModel>), 200)]
    public async Task<IActionResult> GetUserWithTasks(Guid id)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        if (role != "admin" && userId != id)
        {
            return Forbid();
        }

        var tasks = await _mediator.Send(new GetUserWithTasksQuery(id));
        return Ok(tasks);
    }
}
