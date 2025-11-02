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

            var userToUpdate = await _loggedUser.GetLoggedUserAsync();

            _logger.LogInformation(
                "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Updating password for user: {UserId}",
                correlationId, userToUpdate.Id);

            if (!_passwordEncrypter.IsValid(request.CurrentPassword, userToUpdate.Password.Value))
            {
                _logger.LogWarning(
                    "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Invalid current password for user: {UserId}",
                    correlationId, userToUpdate.Id);

                throw new DomainException(ResourceMessages.CurrentPasswordIncorrect);
            }

            Password newPassword = Password.Create(request.NewPassword);
            string hashedPassword = _passwordEncrypter.Encrypt(newPassword.Value);

            _logger.LogInformation(
                "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Password updated for user: {UserId}",
                correlationId, userToUpdate.Id);

            userToUpdate.Update(hashedPassword);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[UpdateUserUseCase] [CorrelationId: {CorrelationId}] Successfully updated password for user: {UserId}",
                correlationId, userToUpdate.Id);

            return new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                UpdatedAt = userToUpdate.UpdatedAt
            };
        }
    }
}
