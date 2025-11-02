using FCG.Application.UseCases.Users.Register.UsersDTO.FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IWriteOnlyWalletRepository _writeOnlyWalletRepository;
        private readonly IWriteOnlyLibraryRepository _writeOnlyLibraryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly ILogger<RegisterUserUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public RegisterUserUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IWriteOnlyUserRepository writeOnlyUserRepository,
            IWriteOnlyWalletRepository writeOnlyWalletRepository,
            IWriteOnlyLibraryRepository writeOnlyLibraryRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncrypter passwordEncrypter,
            ILogger<RegisterUserUseCase> logger,
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

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[RegisterUserUseCase] [CorrelationId: {CorrelationId}] Registering new user: {Email}",
                correlationId, request.Email);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingUser = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

                if (existingUser != null)
                {
                    _logger.LogWarning(
                        "[RegisterUserUseCase] [CorrelationId: {CorrelationId}] Email already in use: {Email}",
                        correlationId, request.Email);

                    throw new DuplicateEmailException(ResourceMessages.EmailAlreadyInUse);
                }

                var hashedPassword = _passwordEncrypter.Encrypt(request.Password);

                var user = User.Create(request.Name, request.Email, hashedPassword, Role.User);

                var wallet = Wallet.Create(user.Id);
                var library = Domain.Entities.Library.Create(user.Id);

                await PersistEntitiesAsync(user, wallet, library, cancellationToken);

                _logger.LogInformation(
                    "[RegisterUserUseCase] [CorrelationId: {CorrelationId}] Successfully registered user: {UserId} - {Email}",
                    correlationId, user.Id, user.Email.Value);

                return new RegisterUserResponse
                {
                    Name = user.Name.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[RegisterUserUseCase] [CorrelationId: {CorrelationId}] Error registering user: {Email}",
                    correlationId, request.Email);

                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task PersistEntitiesAsync(User user, Wallet wallet, Domain.Entities.Library library, CancellationToken cancellationToken)
        {
            await _writeOnlyUserRepository.AddAsync(user);
            await _writeOnlyWalletRepository.AddAsync(wallet);
            await _writeOnlyLibraryRepository.AddAsync(library);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}