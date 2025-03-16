// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListPlayersParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_players.json");
        private readonly IParser<ListPlayerModel> Parser;

        public ListPlayersParserTests()
        {
            Parser = GetService<IParser<ListPlayerModel>>();
            Console.WriteLine(TestDataFile);
        }

        public static IEnumerable<object[]> GetListPlayersTestData() =>
            LoadTestData<ListPlayersTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListPlayersTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input,
            List<PlayerConnectedModel> expectedActivePlayers,
            List<PlayerDisconnectedModel> expectedDisconnectedPlayers)
        {
            // Act
            ListPlayerModel result = Parser.Parse(input);

            // Assert
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
                Assert.Equal(expectedDisconnectedPlayers[i].CreatorIds.EosId, result.DisconnectedPlayers[i].CreatorIds.EosId);
                Assert.Equal(expectedDisconnectedPlayers[i].CreatorIds.SteamId, result.DisconnectedPlayers[i].CreatorIds.SteamId);
                Assert.Equal(expectedDisconnectedPlayers[i].DisconnectedSince, result.DisconnectedPlayers[i].DisconnectedSince);
            }
        }

        private class ListPlayersTestCase
        {
            [JsonPropertyOrder(2)]
            public List<PlayerConnectedModel> ExpectedActivePlayers { get; set; }

            [JsonPropertyOrder(3)]
            public List<PlayerDisconnectedModel> ExpectedDisconnectedPlayers { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}