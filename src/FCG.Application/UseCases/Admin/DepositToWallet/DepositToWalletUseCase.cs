using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Admin.DepositToWallet
{
    public class DepositToWalletUseCase : IDepositToWalletUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IReadOnlyWalletRepository _readOnlyWalletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DepositToWalletUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public DepositToWalletUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IReadOnlyWalletRepository readOnlyWalletRepository,
            IUnitOfWork unitOfWork,
            ILogger<DepositToWalletUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _readOnlyWalletRepository = readOnlyWalletRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<DepositToWalletResponse> Handle(DepositToWalletRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[DepositToWalletUseCase] [CorrelationId: {CorrelationId}] Admin depositing {Amount} to wallet {WalletId} of user {UserId}",
                correlationId, request.Amount, request.WalletId, request.UserId);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _readOnlyUserRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user == null)
                {
                    _logger.LogWarning(
                        "[DepositToWalletUseCase] [CorrelationId: {CorrelationId}] User not found: {UserId}",
                        correlationId, request.UserId);

                    throw new NotFoundException($"User not found with ID: {request.UserId}");
                }

                var wallet = await _readOnlyWalletRepository.GetByIdAsync(request.WalletId, cancellationToken);
                if (wallet == null)
                {
                    _logger.LogWarning(
                        "[DepositToWalletUseCase] [CorrelationId: {CorrelationId}] Wallet not found: {WalletId}",
                        correlationId, request.WalletId);

                    throw new NotFoundException($"Wallet not found with ID: {request.WalletId}");
                }

                if (wallet.UserId != request.UserId)
                {
                    _logger.LogWarning(
                        "[DepositToWalletUseCase] [CorrelationId: {CorrelationId}] Wallet {WalletId} does not belong to user {UserId}. Actual owner: {ActualUserId}",
                        correlationId, request.WalletId, request.UserId, wallet.UserId);

                    throw new DomainException($"Wallet with ID {request.WalletId} does not belong to user with ID {request.UserId}");
                }

                wallet.Deposit(request.Amount);

                await _unitOfWork.CommitAsync(cancellationToken);

                _logger.LogInformation(
                    "[DepositToWalletUseCase] [CorrelationId: {CorrelationId}] Successfully deposited {Amount} to wallet {WalletId}. New balance: {NewBalance}",
                    correlationId, request.Amount, request.WalletId, wallet.Balance);

                return new DepositToWalletResponse
                {
                    UserId = request.UserId,
                    NewBalance = wallet.Balance,
                    DepositedAmount = request.Amount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[DepositToWalletUseCase] [CorrelationId: {CorrelationId}] Error depositing to wallet {WalletId} for user {UserId}",
                    correlationId, request.WalletId, request.UserId);

                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
