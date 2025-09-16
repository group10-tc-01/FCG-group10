using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Example.CreateExample
{
    [ExcludeFromCodeCoverage(Justification = "Example code, will be removed")]
    public class CreateExampleInput : IRequest<CreateExampleOutput>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
