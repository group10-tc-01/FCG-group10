using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class ExampleTest
    {
        [Fact]
        public void Given_ValidExample_When_Instantiate_Then_ShouldCreateExample()
        {
            var exampleBuilder = ExampleBuilder.Build();

            var example = Example.Create(exampleBuilder.Name, exampleBuilder.Description);

            example.Should().NotBeNull();
            example.Id.Should().NotBe(Guid.Empty);
            example.Name.Should().Be(exampleBuilder.Name);
            example.Description.Should().Be(exampleBuilder.Description);
        }
    }
}
