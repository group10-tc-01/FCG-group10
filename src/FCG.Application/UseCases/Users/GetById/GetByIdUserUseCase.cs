using FCG.Application.UseCases.Users.GetById.GetUserDTO;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using MediatR;


namespace FCG.Application.UseCases.Users.GetById
{

    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, UserIdListResponse>
    {
        private readonly IReadOnlyUserRepository _userRepository;

        public GetByIdUserQueryHandler(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserIdListResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new NotFoundException($"Usuário com Id: {request.Id} não encontrado.");
            }
            var response = new UserIdListResponse
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };

            return response;
        }
    }
}