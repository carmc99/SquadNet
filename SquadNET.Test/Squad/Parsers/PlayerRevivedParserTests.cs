// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerRevivedParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_revived.json");
        private readonly IParser<PlayerRevivedEventModel> Parser;

        public PlayerRevivedParserTests()
        {
            Parser = GetService<IParser<PlayerRevivedEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerRevivedTestData() =>
            LoadTestData<PlayerRevivedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerRevivedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID,
            string expectedReviverName, string expectedVictimName,
            string expectedReviverEosId, ulong expectedReviverSteamId,
            string expectedVictimEosId, ulong expectedVictimSteamId)
        {
            PlayerRevivedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedReviverName, result.ReviverName);
            Assert.Equal(expectedVictimName, result.VictimName);
            Assert.Equal(expectedReviverEosId, result.ReviverIds.EosId);
            Assert.Equal(expectedReviverSteamId, result.ReviverIds.SteamId);
            Assert.Equal(expectedVictimEosId, result.VictimIds.EosId);
            Assert.Equal(expectedVictimSteamId, result.VictimIds.SteamId);
        }

        private class PlayerRevivedTestCase
        {
            public string ExpectedChainID { get; set; }
            public string ExpectedReviverEosId { get; set; }
            public string ExpectedReviverName { get; set; }
            public ulong ExpectedReviverSteamId { get; set; }
            public string ExpectedTime { get; set; }
            public string ExpectedVictimEosId { get; set; }
            public string ExpectedVictimName { get; set; }
            public ulong ExpectedVictimSteamId { get; set; }
            public string Input { get; set; }
        }
    }
}