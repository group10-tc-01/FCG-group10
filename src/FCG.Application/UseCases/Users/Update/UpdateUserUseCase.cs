using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using MediatR;

namespace FCG.Application.UseCases.Users.Update
{
    public class UpdateUserUseCase : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncrypter _passwordEncrypter;

        public UpdateUserUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IWriteOnlyUserRepository writeOnlyUserRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncrypter passwordEncrypter)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _writeOnlyUserRepository = writeOnlyUserRepository;
            _unitOfWork = unitOfWork;
            _passwordEncrypter = passwordEncrypter;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var userToUpdate = await _readOnlyUserRepository.GetByIdAsync(request.Id, cancellationToken);

            if (userToUpdate is null)
            {
                throw new NotFoundException($"Usuário com ID {request.Id} não encontrado para atualização.");
            }

            string hashedPassword = userToUpdate.Password.Value;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                {
                    throw new DomainException("A senha atual é obrigatória para alterar a senha.");
                }

                if (!_passwordEncrypter.IsValid(request.CurrentPassword, userToUpdate.Password.Value))
                {
                    throw new DomainException("A senha atual está incorreta.");
                }

                if (request.CurrentPassword == request.NewPassword)
                {
                    throw new DomainException("A nova senha deve ser diferente da senha atual.");
                }

                Password newPasswordVo = Password.Create(request.NewPassword);

                hashedPassword = _passwordEncrypter.Encrypt(newPasswordVo.Value);
            }


            userToUpdate.Update(hashedPassword);

            await _writeOnlyUserRepository.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                UpdatedAt = userToUpdate.UpdatedAt,
                Message = $"Usuário {userToUpdate.Name.Value} sua senha foi atualizado com sucesso!"
            };
        }
    }
}
