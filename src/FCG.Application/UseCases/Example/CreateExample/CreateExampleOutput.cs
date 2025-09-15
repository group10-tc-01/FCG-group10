using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Example.CreateExample
{
    [ExcludeFromCodeCoverage(Justification = "Example code, will be removed")]
    public class CreateExampleOutput
    {
        public CreateExampleDto Example { get; init; } = new CreateExampleDto();
    }
}
