using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;
using TaskModel = Domain.Models.Task;

namespace Application.Commands.Task.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = TaskModel.Create(request.UserId, request.Title, request.Description);
        await _repository.AddAsync(task);
        return _mapper.Map<TaskDto>(task);
    }
}
