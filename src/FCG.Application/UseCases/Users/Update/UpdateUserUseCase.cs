using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly ILogger<UpdateUserUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILoggedUser _loggedUser;


        public UpdateUserUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncrypter passwordEncrypter,
            ILogger<UpdateUserUseCase> logger,
            ICorrelationIdProvider correlationIdProvider,
            ILoggedUser loggedUser)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _unitOfWork = unitOfWork;
            _passwordEncrypter = passwordEncrypter;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
            _loggedUser = loggedUser;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();
            var userId = await _loggedUser.GetLoggedUserAsync();

            _logger.LogInformation(
                "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Updating user: {UserId}",
                correlationId, request.Id);

            var userToUpdate = await _readOnlyUserRepository.GetByIdAsync(userId.Id, cancellationToken);

            if (userToUpdate is null)
            {
                _logger.LogWarning(
                    "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] User not found: {UserId}",
                    correlationId, request.Id);

                throw new NotFoundException(string.Format(ResourceMessages.UserNotFoundForUpdate, request.Id));
            }

            string hashedPassword = userToUpdate.Password.Value;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                {
                    throw new DomainException(ResourceMessages.CurrentPasswordRequired);
                }

                if (!_passwordEncrypter.IsValid(request.CurrentPassword, userToUpdate.Password.Value))
                {
                    _logger.LogWarning(
                        "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Invalid current password for user: {UserId}",
                        correlationId, request.Id);

                    throw new DomainException(ResourceMessages.CurrentPasswordIncorrect);
                }

                if (request.CurrentPassword == request.NewPassword)
                {
                    throw new DomainException(ResourceMessages.NewPasswordMustBeDifferent);
                }

                Password newPasswordVo = Password.Create(request.NewPassword);
                hashedPassword = _passwordEncrypter.Encrypt(newPasswordVo.Value);

                _logger.LogInformation(
                    "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Password updated for user: {UserId}",
                    correlationId, request.Id);
            }

            userToUpdate.Update(hashedPassword);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Successfully updated user: {UserId}",
                correlationId, userToUpdate.Id);

            return new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                UpdatedAt = userToUpdate.UpdatedAt
            };
        }
    }
}
