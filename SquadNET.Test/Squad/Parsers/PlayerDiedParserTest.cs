// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerDiedParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_died.json");
        private readonly IParser<PlayerDiedEventModel> Parser;

        public PlayerDiedParserTests()
        {
            Parser = GetService<IParser<PlayerDiedEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerDiedTestData() =>
            LoadTestData<PlayerDiedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerDiedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedVictimName, float expectedKillingDamage,
            string expectedKillerName, string expectedKillerEosId, ulong expectedKillerSteamId,
            string expectedKillerControllerId, string expectedWeapon)
        {
            PlayerDiedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedVictimName, result.VictimName);
            Assert.Equal(expectedKillingDamage, result.KillingDamage);
            Assert.Equal(expectedKillerName, result.KillerName);
            Assert.Equal(expectedKillerEosId, result.KillerEosId);
            Assert.Equal(expectedKillerSteamId, result.KillerSteamId);
            Assert.Equal(expectedKillerControllerId, result.KillerControllerId);
            Assert.Equal(expectedWeapon, result.Weapon);
        }

        private class PlayerDiedTestCase
        {
            [JsonPropertyOrder(7)]
            public string ExpectedKillerControllerId { get; set; }

            [JsonPropertyOrder(5)]
            public string ExpectedKillerEosId { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedKillerName { get; set; }

            [JsonPropertyOrder(6)]
            public ulong ExpectedKillerSteamId { get; set; }

            [JsonPropertyOrder(3)]
            public float ExpectedKillingDamage { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedVictimName { get; set; }

            [JsonPropertyOrder(8)]
            public string ExpectedWeapon { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}