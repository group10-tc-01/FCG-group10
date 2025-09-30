using FCG.Application.UseCases.UsersDTO;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using MediatR;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;

namespace FCG.Application.UseCases.Users.RegisterUser
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IWriteOnlyUserRepository writeOnlyUserRepository,
            IUnitOfWork unitOfWork)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _writeOnlyUserRepository = writeOnlyUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new DuplicateEmailException("Email já está em uso.");
            }

            var user = User.Create(
                request.Name,
                request.Email,
                request.Password,
                Role.User
            );

            await _writeOnlyUserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterUserResponse
            {
                Name = user.Name.Value,
                Message = $"Usuário {user.Name.Value} registrado com sucesso!"
            };
        }
    }
}