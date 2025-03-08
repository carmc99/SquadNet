// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;

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
            string input, string expectedTime, string expectedChainID, string expectedPlayerSuffix,
            string expectedPossessClassname, string expectedCreatorEosId, ulong expectedCreatorSteamId)
        {
            PlayerPossessEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedPlayerSuffix, result.PlayerSuffix);
            Assert.Equal(expectedPossessClassname, result.PossessClassname);
            Assert.Equal(expectedCreatorEosId, result.CreatorIds.EosId);
            Assert.Equal(expectedCreatorSteamId, result.CreatorIds.SteamId);
        }

        private class PlayerPossessTestCase
        {
            public string ExpectedChainID { get; set; }
            public string ExpectedCreatorEosId { get; set; }
            public ulong ExpectedCreatorSteamId { get; set; }
            public string ExpectedPlayerSuffix { get; set; }
            public string ExpectedPossessClassname { get; set; }
            public string ExpectedTime { get; set; }
            public string Input { get; set; }
        }
    }
}