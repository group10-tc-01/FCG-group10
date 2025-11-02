using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Admin.RoleManagement
{
    public class RoleManagementUseCase : IRoleManagementUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleManagementUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public RoleManagementUseCase(
            IReadOnlyUserRepository readOnlyUserRepository,
            IUnitOfWork unitOfWork,
            ILogger<RoleManagementUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<RoleManagementResponse> Handle(RoleManagementRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[RoleManagementUseCase] [CorrelationId: {CorrelationId}] Updating user role - UserId: {UserId}, NewRole: {NewRole}",
                correlationId, request.UserId, request.NewRole);

            var user = await _readOnlyUserRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException($"Usuário com ID {request.UserId} não encontrado.");

            if (request.NewRole == Role.Admin)
                user.PromoteToAdmin();

            if (request.NewRole == Role.User)
                user.DemoteToUser();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[RoleManagementUseCase] [CorrelationId: {CorrelationId}] Successfully updated user role - UserId: {UserId}, NewRole: {NewRole}",
                correlationId, user.Id, user.Role);

            return new RoleManagementResponse(user.Id, user.Name, user.Email, user.Role);
        }
    }
}
