using FCG.Domain.Repositories.UserRepository;
using FCG.Infrastructure.Persistance.Repositories;
using MediatR;
using System.Linq;

namespace FCG.Application.UseCases.Users.GetAllUsers
{
    public class GetAllUsersUseCase : IRequestHandler<GetAllUserCaseQuery, List<UserListResponse>>
    {
        private readonly IReadOnlyUserRepository _userRepository;
        public GetAllUsersUseCase(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserListResponse>> Handle(GetAllUserCaseQuery request,
            CancellationToken cancellationToken)
        {
            var userEntites = await _userRepository.GetAllUsers(cancellationToken);
            var result = userEntites
                .Select(entity => new UserListResponse
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Email = entity.Email

                })
                .ToList();
            return result;

        }
    }
}
