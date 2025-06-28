using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Commands.Task.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public UpdateTaskHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with Id {request.Id} not found.");
        }

        if (request.Title != null || request.Description != null)
        {
            task.UpdateDetails(request.Title, request.Description);
        }

        if (request.Complete.HasValue)
        {
            if (request.Complete.Value)
                task.Completed();
            else
                task.Uncompleted();
        }

        await _repository.SaveChangesAsync();
        return _mapper.Map<TaskDto>(task);
    }
}
