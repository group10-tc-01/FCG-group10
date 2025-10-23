using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using MediatR;

namespace FCG.Application.UseCases.AdminUsers.GetById
{
    public class GetByIdUserQuery : IRequest<UserDetailResponse>
    {
        public Guid Id { get; init; }

        public GetByIdUserQuery(Guid id)
        {
            Id = id;
        }
    }
}