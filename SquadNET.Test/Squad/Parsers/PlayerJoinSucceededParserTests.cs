// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerJoinSucceededParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_join_succeeded.json");
        private readonly IParser<PlayerJoinSucceededEventModel> Parser;

        public PlayerJoinSucceededParserTests()
        {
            Parser = GetService<IParser<PlayerJoinSucceededEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerJoinSucceededTestData() =>
            LoadTestData<PlayerJoinSucceededTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerJoinSucceededTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID, string expectedPlayerSuffix)
        {
            PlayerJoinSucceededEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedPlayerSuffix, result.PlayerName);
        }

        private class PlayerJoinSucceededTestCase
        {
            [JsonPropertyOrder(3)]
            public string ExpectedChainID { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedPlayerSuffix { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}