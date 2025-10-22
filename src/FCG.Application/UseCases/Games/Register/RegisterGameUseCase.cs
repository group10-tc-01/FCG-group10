using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Messages;

namespace FCG.Application.UseCases.Games.Register
{
    public class RegisterGameUseCase : IRegisterGameUseCase
    {
        private readonly IWriteOnlyGameRepository _writeOnlyGameRepository;
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterGameUseCase(IWriteOnlyGameRepository writeOnlyGameRepository, IReadOnlyGameRepository readOnlyGameRepository, IUnitOfWork unitOfWork)
        {
            _writeOnlyGameRepository = writeOnlyGameRepository;
            _readOnlyGameRepository = readOnlyGameRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterGameOutput> Handle(RegisterGameInput request, CancellationToken cancellationToken)
        {
            await ValidateIfGameAlreadyExistsAsync(request.Name);

            var game = Game.Create(request.Name, request.Description, request.Price, request.Category);
            await _writeOnlyGameRepository.AddAsync(game);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterGameOutput
            {
                Id = game.Id,
                Name = game.Name.Value
            };
        }

        private async Task ValidateIfGameAlreadyExistsAsync(string name)
        {
            var game = await _readOnlyGameRepository.GetByNameAsync(name);

            if (game is not null)
            {
                throw new ConflictException(string.Format(ResourceMessages.GameNameAlreadyExists, name));
            }
        }
    }
}