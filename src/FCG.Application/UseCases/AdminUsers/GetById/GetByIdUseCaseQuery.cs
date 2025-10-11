using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using MediatR;
using System;
public record GetByIdUserQuery : IRequest<GetUserByIdResponse>
{
    public Guid Id { get; init; }

    public GetByIdUserQuery(Guid id)
    {
        Id = id;
    }
}