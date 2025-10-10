using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;

namespace FCG.Application.UseCases.Games.Register
{
    public class RegisterGameUseCase : IRegisterGameUseCase
    {
        private readonly IWriteOnlyGameRepository _writeOnlyGameRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterGameUseCase(IWriteOnlyGameRepository writeOnlyGameRepository, IUnitOfWork unitOfWork)
        {
            _writeOnlyGameRepository = writeOnlyGameRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterGameOutput> Handle(RegisterGameInput request, CancellationToken cancellationToken)
        {
            var game = Game.Create(request.Name, request.Description, request.Price, request.Category);
            await _writeOnlyGameRepository.AddAsync(game);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterGameOutput
            {
                Id = game.Id,
                Name = game.Name.Value
            };
        }
    }
}