using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using MediatR;
using System.Linq;


namespace FCG.Application.UseCases.AdminUsers.GetById
{

    public class GetByIdUserUseCase : IRequestHandler<GetByIdUserQuery, UserDetailResponse>
    {
        private readonly IReadOnlyUserRepository _userRepository;

        public GetByIdUserUseCase(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDetailResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException($"Usuário com Id: {request.Id} não encontrado.");
            }

            var walletDto = user.Wallet == null ? null : new WalletDTO
            {
                Id = user.Wallet.Id,
                Balance = user.Wallet.Balance

            };
            var libraryDto = user.Library == null ? null : new LibraryDTO
            {
                Id = user.Library.Id,
                CreatedAt = user.Library.CreatedAt
            };

            var response = new UserDetailResponse
            {
                Id = user.Id,
                Name = user.Name.Value,
                Email = user.Email.Value,
                Role = user.Role.ToString(),
                Wallet = walletDto,
                Library = libraryDto,
                CreatedAt = user.CreatedAt
            };

            return response;
        }
    }
}