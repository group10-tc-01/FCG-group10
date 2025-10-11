using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using MediatR;


namespace FCG.Application.UseCases.AdminUsers.GetById
{

    public class GetByIdUserUseCase : IRequestHandler<GetByIdUserQuery, GetUserByIdResponse>
    {
        private readonly IReadOnlyUserRepository _userRepository;

        public GetByIdUserUseCase(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new NotFoundException($"Usuário com Id: {request.Id} não encontrado.");
            }
            var response = new GetUserByIdResponse
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