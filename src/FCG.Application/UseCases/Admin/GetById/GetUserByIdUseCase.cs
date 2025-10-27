using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;

namespace FCG.Application.UseCases.Admin.GetById
{
    public class GetUserByIdUseCase : IGetUserByIdUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;

        public GetUserByIdUseCase(IReadOnlyUserRepository userRepository)
        {
            _readOnlyUserRepository = userRepository;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var user = await _readOnlyUserRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException($"User with Id: {request.Id} was not found.");
            }

            var response = MapUserToDetailResponse(user);

            return response;
        }

        private static GetUserByIdResponse MapUserToDetailResponse(User user)
        {
            var walletDto = user.Wallet == null ? null : new WalletDto
            {
                Id = user.Wallet.Id,
                Balance = user.Wallet.Balance
            };

            var libraryDto = user.Library == null ? null : new LibraryDto
            {
                Id = user.Library.Id,
                CreatedAt = user.Library.CreatedAt
            };

            return new GetUserByIdResponse
            {
                Id = user.Id,
                Name = user.Name.Value,
                Email = user.Email.Value,
                Role = user.Role.ToString(),
                Wallet = walletDto,
                Library = libraryDto,
                CreatedAt = user.CreatedAt
            };
        }
    }
}