using MediatR;
using Domain.Interfaces;

namespace Application.Commands.Task.DeleteTask;

public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, Guid>
{
    private readonly ITaskRepository _repository;

    public DeleteTaskHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return request.Id;
    }
}