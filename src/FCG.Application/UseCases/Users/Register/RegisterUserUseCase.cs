using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;

namespace FCG.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncrypter _passwordEncrypter;

        public RegisterUserUseCase(
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

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _readOnlyUserRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new DuplicateEmailException("Email já está em uso.");
            }

            var hashedPassword = _passwordEncrypter.Encrypt(request.Password);

            var user = User.Create(
                request.Name,
                request.Email,
                hashedPassword,
                Role.User
            );
            var wallet = Wallet.Create(user.Id);

            await _writeOnlyUserRepository.AddAsync(user, wallet);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterUserResponse
            {
                Name = user.Name.Value,
                Message = $"Usuário {user.Name.Value} registrado com sucesso!"
            };
        }
    }
}