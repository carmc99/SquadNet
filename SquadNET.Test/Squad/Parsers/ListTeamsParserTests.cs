// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListTeamsParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_teams.json");
        private readonly IParser<List<TeamModel>> Parser;

        public ListTeamsParserTests()
        {
            Parser = GetService<IParser<List<TeamModel>>>();
        }

        public static IEnumerable<object[]> GetListTeamsTestData() =>
            LoadTestData<ListTeamsTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListTeamsTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<TeamModel> expectedTeams)
        {
            List<TeamModel> result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTeams.Count, result.Count);

            for (int i = 0; i < expectedTeams.Count; i++)
            {
                Assert.Equal(expectedTeams[i].Id, result[i].Id);
                Assert.Equal(expectedTeams[i].Name, result[i].Name);
            }
        }

        private class ListTeamsTestCase
        {
            [JsonPropertyOrder(2)]
            public List<TeamModel> ExpectedTeams { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}