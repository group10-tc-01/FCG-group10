using FCG.FunctionalTests.Extensions;
using FCG.FunctionalTests.Fixtures.Admin;
using FCG.FunctionalTests.Fixtures.Authentication;
using FCG.FunctionalTests.Fixtures.Games;
using FCG.FunctionalTests.Fixtures.Users;
using Reqnroll;

namespace FCG.FunctionalTests.Fixtures
{
    public class FixtureManager
    {
        private readonly ScenarioContext _scenarioContext;

        public FixtureManager(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public LoginFixture Login => GetOrCreateFixture<LoginFixture>();
        public LogoutFixture Logout => GetOrCreateFixture<LogoutFixture>();
        public RefreshTokenFixture RefreshToken => GetOrCreateFixture<RefreshTokenFixture>();
        public RegisterGameFixture RegisterGame => GetOrCreateFixture<RegisterGameFixture>();
        public RegisterUserFixture RegisterUser => GetOrCreateFixture<RegisterUserFixture>();
        public UpdateUserFixture UpdateUser => GetOrCreateFixture<UpdateUserFixture>();
        public GetAllUsersFixture GetAllUsers => GetOrCreateFixture<GetAllUsersFixture>();
        public GetUserByIdFixture GetUserById => GetOrCreateFixture<GetUserByIdFixture>();
        public RoleManagementFixture RoleManagement => GetOrCreateFixture<RoleManagementFixture>();

        private T GetOrCreateFixture<T>() where T : new()
        {
            var key = typeof(T).Name;
            var fixture = _scenarioContext.GetScenario<T>(key);

            if (fixture == null)
            {
                fixture = new T();
                _scenarioContext[key] = fixture;
            }
            return fixture;
        }
    }
}
