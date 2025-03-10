// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerConnectedParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_connected.json");
        private readonly IParser<PlayerConnectedEventModel> Parser;

        public PlayerConnectedParserTests()
        {
            Parser = GetService<IParser<PlayerConnectedEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerConnectedTestData() =>
            LoadTestData<PlayerConnectedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerConnectedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedName, string expectedIP,
            string expectedEosId, ulong expectedSteamId)
        {
            PlayerConnectedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Name);
            Assert.Equal(expectedIP, result.IP);
            Assert.Equal(expectedEosId, result.CreatorIds.EosId);
            Assert.Equal(expectedSteamId, result.CreatorIds.SteamId);
        }

        private class PlayerConnectedTestCase
        {
            [JsonPropertyOrder(4)]
            public string ExpectedEosId { get; set; }

            [JsonPropertyOrder(3)]
            public string ExpectedIP { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedName { get; set; }

            [JsonPropertyOrder(5)]
            public ulong ExpectedSteamId { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}