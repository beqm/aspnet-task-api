using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.User.GetUserWithTasks;

public class GetUserWithTasksQueryHandler : IRequestHandler<GetUserWithTasksQuery, UserWithTasksDto>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserWithTasksQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserWithTasksDto> Handle(GetUserWithTasksQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithTasksAsync(request.Id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }

        return _mapper.Map<UserWithTasksDto>(user);
    }
}


