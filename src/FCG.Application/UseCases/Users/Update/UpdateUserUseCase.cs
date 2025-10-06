using FCG.Application.UseCases.Users.Update.UsersDTO;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCG.Application.UseCases.Users.Update
{
    // A interface do MediatR deve ser IRequestHandler
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

            if (userToUpdate.Email.Value != request.Email)
            {
                var existingUserWithNewEmail = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

                if (existingUserWithNewEmail is not null && existingUserWithNewEmail.Id != userToUpdate.Id)
                {
                    throw new DuplicateEmailException($"O e-mail '{request.Email}' já está em uso por outro usuário.");
                }
            }
            string hashedPassword = userToUpdate.Password.Value;
            if (!string.IsNullOrEmpty(request.Password))
            {
                hashedPassword = _passwordEncrypter.Encrypt(request.Password);
            }

            Role newRole = Enum.TryParse<Role>(request.Role, true, out var roleResult) ? roleResult : userToUpdate.Role;

            userToUpdate.Update(
                request.Name,
                request.Email,
                hashedPassword,
                newRole
            );

            await _writeOnlyUserRepository.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                Name = userToUpdate.Name.Value,
                Email = userToUpdate.Email.Value,
                Role = userToUpdate.Role.ToString(),
                UpdatedAt = userToUpdate.UpdatedAt,
                Message = $"Usuário {userToUpdate.Name.Value} atualizado com sucesso!"
            };
        }
    }
}