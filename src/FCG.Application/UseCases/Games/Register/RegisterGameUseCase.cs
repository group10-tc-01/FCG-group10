using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Services;
using FCG.Messages;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Games.Register
{
    public class RegisterGameUseCase : IRegisterGameUseCase
    {
        private readonly IWriteOnlyGameRepository _writeOnlyGameRepository;
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterGameUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public RegisterGameUseCase(
            IWriteOnlyGameRepository writeOnlyGameRepository, 
            IReadOnlyGameRepository readOnlyGameRepository, 
            IUnitOfWork unitOfWork,
            ILogger<RegisterGameUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _writeOnlyGameRepository = writeOnlyGameRepository;
            _readOnlyGameRepository = readOnlyGameRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<RegisterGameOutput> Handle(RegisterGameInput request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[RegisterGameUseCase] [CorrelationId: {CorrelationId}] Registering new game: {GameName}",
                correlationId, request.Name);

            await ValidateIfGameAlreadyExistsAsync(request.Name, correlationId);

            var category = Enum.Parse<GameCategory>(request.Category, true);
            var game = Game.Create(request.Name, request.Description, request.Price, category);
            await _writeOnlyGameRepository.AddAsync(game);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[RegisterGameUseCase] [CorrelationId: {CorrelationId}] Successfully registered game: {GameId} - {GameName}",
                correlationId, game.Id, game.Name.Value);

            return new RegisterGameOutput
            {
                Id = game.Id,
                Name = game.Name.Value
            };
        }

        private async Task ValidateIfGameAlreadyExistsAsync(string name, string correlationId)
        {
            var game = await _readOnlyGameRepository.GetByNameAsync(name);

            if (game is not null)
            {
                _logger.LogWarning(
                    "[RegisterGameUseCase] [CorrelationId: {CorrelationId}] Game name already exists: {GameName}",
                    correlationId, name);

                throw new ConflictException(string.Format(ResourceMessages.GameNameAlreadyExists, name));
            }
        }
    }
}