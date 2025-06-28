using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.User.GetUsersRange;

public class GetUsersRangeQueryHandler : IRequestHandler<GetUsersRangeQuery, IEnumerable<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUsersRangeQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersRangeQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetRangeAsync(request.Page, request.PageSize);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}
