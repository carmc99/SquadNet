// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListLevelsParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_levels.json");
        private readonly IParser<List<LevelModel>> Parser;

        public ListLevelsParserTests()
        {
            Parser = GetService<IParser<List<LevelModel>>>();
        }

        public static IEnumerable<object[]> GetListLevelsTestData() =>
            LoadTestData<ListLevelsTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListLevelsTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<LevelModel> expectedLevels)
        {
            List<LevelModel> result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedLevels.Count, result.Count);

            for (int i = 0; i < expectedLevels.Count; i++)
            {
                Assert.Equal(expectedLevels[i].Name, result[i].Name);
            }
        }

        private class ListLevelsTestCase
        {
            [JsonPropertyOrder(2)]
            public List<LevelModel> ExpectedLevels { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}