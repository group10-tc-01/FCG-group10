using FCG.Application.UseCases.Users.GetById.GetUserDTO;
using MediatR;
using System;
public record GetByIdUserQuery : IRequest<UserIdListResponse>
{
    public Guid Id { get; init; }

    public GetByIdUserQuery(Guid id)
    {
        Id = id;
    }
}