using MediatR;

namespace FCG.Application.UseCases.Example.CreateExample
{
    public interface ICreateExampleUseCase : IRequestHandler<CreateExampleInput, CreateExampleOutput> { }
}
