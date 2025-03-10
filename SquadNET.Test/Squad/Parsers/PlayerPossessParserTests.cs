// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerPossessParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_possess.json");
        private readonly IParser<PlayerPossessEventModel> Parser;

        public PlayerPossessParserTests()
        {
            Parser = GetService<IParser<PlayerPossessEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerPossessTestData() =>
            LoadTestData<PlayerPossessTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerPossessTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, int expectedChainID, string expectedPlayerName,
            string expectedPossessClassname, string expectedCreatorEosId, ulong expectedCreatorSteamId)
        {
            PlayerPossessEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedPlayerName, result.PlayerName);
            Assert.Equal(expectedPossessClassname, result.PossessClassname);
            Assert.Equal(expectedCreatorEosId, result.CreatorIds.EosId);
            Assert.Equal(expectedCreatorSteamId, result.CreatorIds.SteamId);
        }

        private class PlayerPossessTestCase
        {
            [JsonPropertyOrder(3)]
            public int ExpectedChainID { get; set; }

            [JsonPropertyOrder(6)]
            public string ExpectedCreatorEosId { get; set; }

            [JsonPropertyOrder(7)]
            public ulong ExpectedCreatorSteamId { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedPlayerName { get; set; }

            [JsonPropertyOrder(5)]
            public string ExpectedPossessClassname { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}