using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Task.GetTasksRange;

public class GetTasksRangeQueryHandler : IRequestHandler<GetTasksRangeQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public GetTasksRangeQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksRangeQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetRangeAsync(request.Page, request.PageSize);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }
}
