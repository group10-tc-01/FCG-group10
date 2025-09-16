using FCG.FunctionalTests.Extensions;
using FCG.FunctionalTests.Fixtures.Example;
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

        public CreateExampleFixture CreateExample => GetOrCreateFixture<CreateExampleFixture>();


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
