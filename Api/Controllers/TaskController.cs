using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskModel = Domain.Models.Task;
using Microsoft.AspNetCore.Authorization;
using Application.Commands.Task.CreateTask;
using Application.Commands.Task.UpdateTask;
using Application.Commands.Task.DeleteTask;
using Application.Queries.Task.GetTasksById;
using Application.Queries.Task.GetTasksRange;

namespace Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/task")]
public class TaskController : BaseController
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskModel), 200)]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
    {
        var result = await _mediator.Send(command with { UserId = GetUserId() });
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskModel), 200)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var result = await _mediator.Send(new GetTaskByIdQuery(id, userId, role));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskModel>), 200)]
    public async Task<IActionResult> GetRange([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var query = new GetTasksRangeQuery(page, pageSize, userId, role);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskModel), 200)]
    public async Task<IActionResult> UpdateById(Guid id, [FromBody] UpdateTaskCommand command)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var result = await _mediator.Send(command with { Id = id, RequestingUserId = userId, RequestingRole = role });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var userId = GetUserId();
        var role = GetUserRole();

        var result = await _mediator.Send(new DeleteTaskCommand(id, userId, role));
        if (result == Guid.Empty)
        {
            return NotFound();
        }

        return Ok(result);
    }
}