// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class SquadCreatedMessageParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("squad_created.json");
        private readonly IParser<SquadCreatedEventModel> Parser;

        public SquadCreatedMessageParserTests()
        {
            Parser = GetService<IParser<SquadCreatedEventModel>>();
        }

        public static IEnumerable<object[]> GetSquadCreatedTestData() =>
            LoadTestData<SquadCreatedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetSquadCreatedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedPlayerName, int expectedSquadId,
            string expectedSquadName, string expectedTeamName,
            string expectedCreatorEosId, ulong expectedCreatorSteamId)
        {
            SquadCreatedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedPlayerName, result.PlayerName);
            Assert.Equal(expectedSquadId, result.SquadId);
            Assert.Equal(expectedSquadName, result.SquadName);
            Assert.Equal(expectedTeamName, result.TeamName);
            Assert.Equal(expectedCreatorEosId, result.CreatorIds.EosId);
            Assert.Equal(expectedCreatorSteamId, result.CreatorIds.SteamId);
        }

        private class SquadCreatedTestCase
        {
            public string ExpectedCreatorEosId { get; set; }
            public ulong ExpectedCreatorSteamId { get; set; }
            public string ExpectedPlayerName { get; set; }
            public int ExpectedSquadId { get; set; }
            public string ExpectedSquadName { get; set; }
            public string ExpectedTeamName { get; set; }
            public string Input { get; set; }
        }
    }
}