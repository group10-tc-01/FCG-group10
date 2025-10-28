using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;

namespace FCG.FunctionalTests.Fixtures.Admin
{
    public class GetAllUsersFixture
    {
        public GetAllUsersFixture()
        {
            var readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<GetAllUsersUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            Setup();

            GetAllUsersUseCase = new GetAllUsersUseCase(readOnlyUserRepository, logger, correlationIdProvider);
            GetAllUserCaseRequest = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };
        }

        public GetAllUsersUseCase GetAllUsersUseCase { get; }
        public GetAllUserCaseRequest GetAllUserCaseRequest { get; }

        private static void Setup()
        {
            var users = new List<FCG.Domain.Entities.User>
            {
                UserBuilder.Build(),
                UserBuilder.Build()
            };
            ReadOnlyUserRepositoryBuilder.SetupGetAllUsersAsync(users);
        }
    }
}
