// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;

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
            string expectedPlayerSuffix, string expectedPlayerEosId, ulong expectedPlayerSteamId)
        {
            PlayerUnPossessEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedPlayerSuffix, result.PlayerSuffix);
            Assert.Equal(expectedPlayerEosId, result.PlayerIds.EosId);
            Assert.Equal(expectedPlayerSteamId, result.PlayerIds.SteamId);
        }

        private class PlayerUnPossessTestCase
        {
            public string ExpectedChainID { get; set; }
            public string ExpectedPlayerEosId { get; set; }
            public ulong ExpectedPlayerSteamId { get; set; }
            public string ExpectedPlayerSuffix { get; set; }
            public string ExpectedTime { get; set; }
            public string Input { get; set; }
        }
    }
}