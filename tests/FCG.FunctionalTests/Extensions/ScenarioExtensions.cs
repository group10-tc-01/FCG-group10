﻿using Reqnroll;

namespace FCG.FunctionalTests.Extensions
{
    public static class ScenarioExtensions
    {
        public static T GetScenario<T>(this ScenarioContext scenarioContext, string keyName)
        {
            try
            {
                return (T)scenarioContext[keyName];
            }
            catch
            {
                return default!;
            }
        }
    }
}
