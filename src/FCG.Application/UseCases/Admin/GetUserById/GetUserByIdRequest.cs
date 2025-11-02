using MediatR;

namespace FCG.Application.UseCases.Admin.GetUserById
{
    public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
    {
        public Guid Id { get; init; }

        public GetUserByIdRequest(Guid id)
        {
            Id = id;
        }
    }
}