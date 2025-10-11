using FCG.Application.UseCases.Users.Update.UsersDTO;
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
            var userToUpdate = await _readOnlyUserRepository.GetByIdAsync(request.Id);

            if (userToUpdate is null)
            {
                throw new NotFoundException($"Usuário com ID {request.Id} não encontrado para atualização.");
            }
            string hashedPassword = userToUpdate.Password.Value;
            if (!string.IsNullOrEmpty(request.Password))
            {
                Password newPassword = Password.Create(request.Password);
                hashedPassword = _passwordEncrypter.Encrypt(newPassword.Value);
            }
            userToUpdate.Update(
                hashedPassword
            );

            await _writeOnlyUserRepository.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                UpdatedAt = userToUpdate.UpdatedAt,
                Message = $"Usuário {userToUpdate.Name.Value} atualizado com sucesso!"
            };
        }
    }
}