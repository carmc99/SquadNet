// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerUnPossessParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_unpossess.json");
        private readonly IParser<PlayerUnPossessEventModel> Parser;

        public PlayerUnPossessParserTests()
        {
            Parser = GetService<IParser<PlayerUnPossessEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerUnPossessTestData() =>
            LoadTestData<PlayerUnPossessTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerUnPossessTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID,
            string expectedPlayerName, string expectedPlayerEosId, ulong expectedPlayerSteamId)
        {
            PlayerUnPossessEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedPlayerName, result.PlayerName);
            Assert.Equal(expectedPlayerEosId, result.PlayerIds.EosId);
            Assert.Equal(expectedPlayerSteamId, result.PlayerIds.SteamId);
        }

        private class PlayerUnPossessTestCase
        {
            [JsonPropertyOrder(3)]
            public string ExpectedChainID { get; set; }

            [JsonPropertyOrder(5)]
            public string ExpectedPlayerEosId { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedPlayerName { get; set; }

            [JsonPropertyOrder(6)]
            public ulong ExpectedPlayerSteamId { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}