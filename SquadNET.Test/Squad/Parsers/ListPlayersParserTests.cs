// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListPlayersParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_players.json");
        private readonly IParser<ListPlayerModel> Parser;

        public ListPlayersParserTests()
        {
            Parser = GetService<IParser<ListPlayerModel>>();
        }

        public static IEnumerable<object[]> GetListPlayersTestData() =>
            LoadTestData<ListPlayersTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListPlayersTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<PlayerConnectedEventModel> expectedActivePlayers,
            List<PlayerDisconnectedEventModel> expectedDisconnectedPlayers)
        {
            ListPlayerModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedActivePlayers.Count, result.ActivePlayers.Count);
            Assert.Equal(expectedDisconnectedPlayers.Count, result.DisconnectedPlayers.Count);

            for (int i = 0; i < expectedActivePlayers.Count; i++)
            {
                Assert.Equal(expectedActivePlayers[i].Id, result.ActivePlayers[i].Id);
                Assert.Equal(expectedActivePlayers[i].Name, result.ActivePlayers[i].Name);
                Assert.Equal(expectedActivePlayers[i].Team, result.ActivePlayers[i].Team);
                Assert.Equal(expectedActivePlayers[i].SquadId, result.ActivePlayers[i].SquadId);
                Assert.Equal(expectedActivePlayers[i].IsLeader, result.ActivePlayers[i].IsLeader);
                Assert.Equal(expectedActivePlayers[i].Role, result.ActivePlayers[i].Role);
                Assert.Equal(expectedActivePlayers[i].CreatorIds.EosId, result.ActivePlayers[i].CreatorIds.EosId);
                Assert.Equal(expectedActivePlayers[i].CreatorIds.SteamId, result.ActivePlayers[i].CreatorIds.SteamId);
            }

            for (int i = 0; i < expectedDisconnectedPlayers.Count; i++)
            {
                Assert.Equal(expectedDisconnectedPlayers[i].Id, result.DisconnectedPlayers[i].Id);
                Assert.Equal(expectedDisconnectedPlayers[i].Name, result.DisconnectedPlayers[i].Name);
                Assert.Equal(expectedDisconnectedPlayers[i].SteamId, result.DisconnectedPlayers[i].SteamId);
                Assert.Equal(expectedDisconnectedPlayers[i].DisconnectedSince, result.DisconnectedPlayers[i].DisconnectedSince);
            }
        }

        private class ListPlayersTestCase
        {
            public List<PlayerConnectedEventModel> ExpectedActivePlayers { get; set; }
            public List<PlayerDisconnectedEventModel> ExpectedDisconnectedPlayers { get; set; }
            public string Input { get; set; }
        }
    }
}