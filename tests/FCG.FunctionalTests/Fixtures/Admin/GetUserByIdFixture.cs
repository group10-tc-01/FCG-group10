using FCG.Application.UseCases.Admin.GetById;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Repositories.UserRepository;
using Moq;

namespace FCG.FunctionalTests.Fixtures.Admin
{
    public class GetUserByIdFixture
    {
        public GetUserByIdFixture()
        {
            var testUser = UserBuilder.Build();

            var mockRepository = new Mock<IReadOnlyUserRepository>();

            mockRepository.Setup(repo => repo.GetByIdWithDetailsAsync(testUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(testUser);

            GetUserByIdUseCase = new GetUserByIdUseCase(mockRepository.Object);
            GetUserByIdRequest = new GetUserByIdRequest(testUser.Id);
        }

        public GetUserByIdUseCase GetUserByIdUseCase { get; }
        public GetUserByIdRequest GetUserByIdRequest { get; }
    }
}
