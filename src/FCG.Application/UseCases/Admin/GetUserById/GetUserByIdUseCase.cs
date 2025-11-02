using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Admin.GetUserById
{
    public class GetUserByIdUseCase : IGetUserByIdUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ILogger<GetUserByIdUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public GetUserByIdUseCase(
            IReadOnlyUserRepository userRepository,
            ILogger<GetUserByIdUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _readOnlyUserRepository = userRepository;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[GetUserByIdUseCase] [CorrelationId: {CorrelationId}] Getting user by ID: {UserId}",
                correlationId, request.Id);

            var user = await _readOnlyUserRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning(
                    "[GetUserByIdUseCase] [CorrelationId: {CorrelationId}] User not found: {UserId}",
                    correlationId, request.Id);

                throw new NotFoundException($"User with Id: {request.Id} was not found.");
            }

            var response = MapUserToDetailResponse(user);

            _logger.LogInformation(
                "[GetUserByIdUseCase] [CorrelationId: {CorrelationId}] Successfully retrieved user: {UserId}",
                correlationId, user.Id);

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
                CreatedAt = user.Library.CreatedAt,
                Games = user.Library.LibraryGames.Select(lg => new LibraryGameDto
                {
                    GameId = lg.GameId,
                    Name = lg.Game?.Name.Value ?? string.Empty,
                    Description = lg.Game?.Description ?? string.Empty,
                    Category = lg.Game?.Category.ToString() ?? string.Empty,
                    PurchasePrice = lg.PurchasePrice.Value,
                    PurchaseDate = lg.PurchaseDate
                }).ToList()
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