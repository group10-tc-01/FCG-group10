using MediatR;

namespace FCG.Application.UseCases.Example.CreateExample
{
    public class CreateExampleInput : IRequest<CreateExampleOutput>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
