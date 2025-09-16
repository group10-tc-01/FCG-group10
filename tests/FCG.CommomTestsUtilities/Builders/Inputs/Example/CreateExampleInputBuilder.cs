using Bogus;
using FCG.Application.UseCases.Example.CreateExample;

namespace FCG.CommomTestsUtilities.Builders.Inputs.Example
{
    public class CreateExampleInputBuilder
    {
        public static CreateExampleInput Build()
        {
            return new Faker<CreateExampleInput>().RuleFor(e => e.Name, f => f.Name.FullName())
                                                   .RuleFor(e => e.Description, f => f.Lorem.Sentence())
                                                   .Generate();
        }
    }
}
