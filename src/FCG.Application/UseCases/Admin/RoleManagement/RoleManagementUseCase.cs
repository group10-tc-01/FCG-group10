using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;

namespace FCG.Application.UseCases.Admin.RoleManagement
{
    public class RoleManagementUseCase : IRoleManagementUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleManagementUseCase(IReadOnlyUserRepository readOnlyUserRepository, IUnitOfWork unitOfWork)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleManagementResponse> Handle(RoleManagementRequest request, CancellationToken cancellationToken)
        {
            var user = await _readOnlyUserRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException($"Usuário com ID {request.UserId} não encontrado.");

            if (request.NewRole == Role.Admin)
                user.PromoteToAdmin();

            if (request.NewRole == Role.User)
                user.DemoteToUser();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RoleManagementResponse(user.Id, user.Name, user.Email, user.Role);
        }
    }
}
