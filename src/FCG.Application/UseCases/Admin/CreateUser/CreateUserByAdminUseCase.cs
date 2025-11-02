using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Admin.CreateUser
{
    public class CreateUserByAdminUseCase : ICreateUserByAdminUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IWriteOnlyWalletRepository _writeOnlyWalletRepository;
        private readonly IWriteOnlyLibraryRepository _writeOnlyLibraryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly ILogger<CreateUserByAdminUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CreateUserByAdminUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IWriteOnlyUserRepository writeOnlyUserRepository,
            IWriteOnlyWalletRepository writeOnlyWalletRepository,
            IWriteOnlyLibraryRepository writeOnlyLibraryRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncrypter passwordEncrypter,
            ILogger<CreateUserByAdminUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _writeOnlyUserRepository = writeOnlyUserRepository;
            _writeOnlyWalletRepository = writeOnlyWalletRepository;
            _writeOnlyLibraryRepository = writeOnlyLibraryRepository;
            _unitOfWork = unitOfWork;
            _passwordEncrypter = passwordEncrypter;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<CreateUserByAdminResponse> Handle(CreateUserByAdminRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[CreateUserByAdminUseCase] [CorrelationId: {CorrelationId}] Admin creating new user: {Email} with role: {Role}",
                correlationId, request.Email, request.Role);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingUser = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

                if (existingUser != null)
                {
                    _logger.LogWarning(
                        "[CreateUserByAdminUseCase] [CorrelationId: {CorrelationId}] Email already in use: {Email}",
                        correlationId, request.Email);

                    throw new DuplicateEmailException(ResourceMessages.EmailAlreadyInUse);
                }

                var hashedPassword = _passwordEncrypter.Encrypt(request.Password);

                var user = User.Create(request.Name, request.Email, hashedPassword, request.Role);

                var wallet = Wallet.Create(user.Id);
                var library = Library.Create(user.Id);

                await PersistEntitiesAsync(user, wallet, library, cancellationToken);

                _logger.LogInformation(
                    "[CreateUserByAdminUseCase] [CorrelationId: {CorrelationId}] Successfully created user: {UserId} - {Email} with role: {Role}",
                    correlationId, user.Id, user.Email.Value, user.Role);

                return new CreateUserByAdminResponse
                {
                    Id = user.Id,
                    Name = user.Name.Value,
                    Email = user.Email.Value,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[CreateUserByAdminUseCase] [CorrelationId: {CorrelationId}] Error creating user: {Email}",
                    correlationId, request.Email);

                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task PersistEntitiesAsync(User user, Wallet wallet, Library library, CancellationToken cancellationToken)
        {
            await _writeOnlyUserRepository.AddAsync(user);
            await _writeOnlyWalletRepository.AddAsync(wallet);
            await _writeOnlyLibraryRepository.AddAsync(library);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
