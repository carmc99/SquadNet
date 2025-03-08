// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ShowCurrentMapParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("show_current_map.json");
        private readonly IParser<CurrentMapModel> Parser;

        public ShowCurrentMapParserTests()
        {
            Parser = GetService<IParser<CurrentMapModel>>();
        }

        public static IEnumerable<object[]> GetShowCurrentMapTestData() =>
            LoadTestData<ShowCurrentMapTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetShowCurrentMapTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedLevel, string expectedName)
        {
            CurrentMapModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedLevel, result.Level);
            Assert.Equal(expectedName, result.Name);
        }

        private class ShowCurrentMapTestCase
        {
            public string ExpectedLevel { get; set; }
            public string ExpectedName { get; set; }
            public string Input { get; set; }
        }
    }
}