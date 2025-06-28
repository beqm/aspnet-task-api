using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Task.GetTasksById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
        }

        if (request.Role != "admin" && task.UserId != request.UserId)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
        }

        return _mapper.Map<TaskDto>(task);
    }
}
