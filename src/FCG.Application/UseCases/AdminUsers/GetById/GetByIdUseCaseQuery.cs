using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using MediatR;
public record GetByIdUserQuery : IRequest<UserDetailResponse>
{
    public Guid Id { get; init; }

    public GetByIdUserQuery(Guid id)
    {
        Id = id;
    }
}