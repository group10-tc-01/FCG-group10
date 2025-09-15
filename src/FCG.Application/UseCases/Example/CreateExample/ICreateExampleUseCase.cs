using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Example.CreateExample
{
    [ExcludeFromCodeCoverage(Justification = "Example code, will be removed")]
    public interface ICreateExampleUseCase : IRequestHandler<CreateExampleInput, CreateExampleOutput> { }
}
