using FCG.Domain.Enum;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;

namespace FCG.Application.UseCases.Users.RoleManagement
{
    public class RoleManagementUseCase : IRoleManagementUseCase
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleManagementUseCase(IReadOnlyUserRepository readOnlyUserRepository, IWriteOnlyUserRepository writeOnlyUserRepository, IUnitOfWork unitOfWork)
        {
            _readOnlyUserRepository = readOnlyUserRepository;
            _writeOnlyUserRepository = writeOnlyUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleManagementResponse> Handle(RoleManagementRequest request, CancellationToken cancellationToken)
        {
            var user = await _readOnlyUserRepository.GetByIdAsync(request.UserId);

            if (request.NewRole == Role.Admin)
                user?.PromoteToAdmin();
            else
                user?.DemoteToUser();

            await _writeOnlyUserRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RoleManagementResponse(user.Id, user.Name, user.Email, user.Role);
        }
    }
}
