using FCG.Application.UseCases.Admin.GetUserById;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Services;
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
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<GetUserByIdUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            mockRepository.Setup(repo => repo.GetByIdWithDetailsAsync(testUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(testUser);

            GetUserByIdUseCase = new GetUserByIdUseCase(mockRepository.Object, logger, correlationIdProvider);
            GetUserByIdRequest = new GetUserByIdRequest(testUser.Id);
        }

        public GetUserByIdUseCase GetUserByIdUseCase { get; }
        public GetUserByIdRequest GetUserByIdRequest { get; }
    }
}
