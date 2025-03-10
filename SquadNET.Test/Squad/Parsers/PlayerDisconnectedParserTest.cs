// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class PlayerDisconnectedParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("player_disconnected.json");
        private readonly IParser<PlayerDisconnectedEventModel> Parser;

        public PlayerDisconnectedParserTests()
        {
            Parser = GetService<IParser<PlayerDisconnectedEventModel>>();
        }

        public static IEnumerable<object[]> GetPlayerDisconnectedTestData() =>
            LoadTestData<PlayerDisconnectedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetPlayerDisconnectedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedRemoteAddress, string expectedConnectionName,
            string expectedDriver, bool expectedIsServer, string expectedPlayerController,
            string expectedOwner, string expectedEosId)
        {
            PlayerDisconnectedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedRemoteAddress, result.RemoteAddress);
            Assert.Equal(expectedConnectionName, result.ConnectionName);
            Assert.Equal(expectedDriver, result.Driver);
            Assert.Equal(expectedIsServer, result.IsServer);
            Assert.Equal(expectedPlayerController, result.PlayerController);
            Assert.Equal(expectedOwner, result.Owner);
            Assert.Equal(expectedEosId, result.EosId);
        }

        private class PlayerDisconnectedTestCase
        {
            [JsonPropertyOrder(3)]
            public string ExpectedConnectionName { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedDriver { get; set; }

            [JsonPropertyOrder(8)]
            public string ExpectedEosId { get; set; }

            [JsonPropertyOrder(5)]
            public bool ExpectedIsServer { get; set; }

            [JsonPropertyOrder(7)]
            public string ExpectedOwner { get; set; }

            [JsonPropertyOrder(6)]
            public string ExpectedPlayerController { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedRemoteAddress { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}