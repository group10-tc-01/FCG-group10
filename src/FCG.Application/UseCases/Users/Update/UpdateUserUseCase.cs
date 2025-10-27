using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using FCG.Messages;

namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncrypter _passwordEncrypter;

        public UpdateUserUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncrypter passwordEncrypter)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _unitOfWork = unitOfWork;
            _passwordEncrypter = passwordEncrypter;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var userToUpdate = await _readOnlyUserRepository.GetByIdAsync(request.Id, cancellationToken);

            if (userToUpdate is null)
            {
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
                    throw new DomainException(ResourceMessages.CurrentPasswordIncorrect);
                }

                if (request.CurrentPassword == request.NewPassword)
                {
                    throw new DomainException(ResourceMessages.NewPasswordMustBeDifferent);
                }

                Password newPasswordVo = Password.Create(request.NewPassword);
                hashedPassword = _passwordEncrypter.Encrypt(newPasswordVo.Value);
            }

            userToUpdate.Update(hashedPassword);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                UpdatedAt = userToUpdate.UpdatedAt
            };
        }
    }
}
