using FCG.Domain.Entities;
using FCG.Domain.Repositories.ExampleRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.ExampleRepository
{
    public class WriteOnlyExampleRepositoryBuilder
    {
        private static readonly Mock<IWriteOnlyExampleRepository> _mock = new Mock<IWriteOnlyExampleRepository>();

        public static IWriteOnlyExampleRepository Build() => _mock.Object;

        public static void SetupAddSync()
        {
            _mock.Setup(repo => repo.AddAsync(It.IsAny<Example>())).Returns(Task.CompletedTask);
        }
    }
}
