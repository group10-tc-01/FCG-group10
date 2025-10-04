using FCG.Domain.Repositories.UserRepository;
using FCG.Infrastructure.Persistance.Repositories;
using MediatR;
using System.Linq;
using FCG.Domain.Exceptions;

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
            if (userEntites == null)
            {
                throw new NotFoundException($"Usuário não encontrado");

            }
            var result = userEntites
                .Select(entity => new UserListResponse
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Email = entity.Email,
                    CreatedAt = entity.CreatedAt,
                    Role = entity.Role.ToString()

                })
                .ToList();
            return result;

        }
    }
}
