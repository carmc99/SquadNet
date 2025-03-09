// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerDamagedParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_damaged.json");
        private readonly IParser<PlayerDamagedEventModel> Parser;

        public PlayerDamagedParserTests()
        {
            Parser = GetService<IParser<PlayerDamagedEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerDamagedTestData() =>
            LoadTestData<PlayerDamagedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerDamagedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID,
            string expectedVictimName, float expectedDamage, string expectedAttackerName,
            string expectedAttackerPlayerController, string expectedWeapon,
            string expectedAttackerEosId, ulong expectedAttackerSteamId)
        {
            PlayerDamagedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedVictimName, result.VictimName);
            Assert.Equal(expectedDamage, result.Damage);
            Assert.Equal(expectedAttackerName, result.AttackerName);
            Assert.Equal(expectedAttackerPlayerController, result.AttackerPlayerController);
            Assert.Equal(expectedWeapon, result.Weapon);
            Assert.Equal(expectedAttackerEosId, result.AttackerIds.EosId);
            Assert.Equal(expectedAttackerSteamId, result.AttackerIds.SteamId);
        }

        private class PlayerDamagedTestCase
        {
            [JsonPropertyOrder(9)]
            public string ExpectedAttackerEosId { get; set; }

            [JsonPropertyOrder(6)]
            public string ExpectedAttackerName { get; set; }

            [JsonPropertyOrder(7)]
            public string ExpectedAttackerPlayerController { get; set; }

            [JsonPropertyOrder(10)]
            public ulong ExpectedAttackerSteamId { get; set; }

            [JsonPropertyOrder(3)]
            public string ExpectedChainID { get; set; }

            [JsonPropertyOrder(5)]
            public float ExpectedDamage { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedVictimName { get; set; }

            [JsonPropertyOrder(8)]
            public string ExpectedWeapon { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}