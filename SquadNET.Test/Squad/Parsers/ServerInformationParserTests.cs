// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ServerInformationParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("server_information.json");
        private readonly IParser<ServerInformationModel> Parser;

        public ServerInformationParserTests()
        {
            Parser = GetService<IParser<ServerInformationModel>>();
        }

        public static IEnumerable<object[]> GetServerInformationTestData() =>
            LoadTestData<ServerInformationTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetServerInformationTestData))]
        public void GivenJsonInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedServerName, int expectedMaxPlayers, int expectedPublicQueueLimit,
            int expectedReserveSlots, int expectedPlayerCount, int expectedA2sPlayerCount, int expectedPublicQueue,
            int expectedReserveQueue, string expectedCurrentLayer, string expectedNextLayer, string expectedTeamOne,
            string expectedTeamTwo, double expectedMatchTimeout, string expectedGameVersion)
        {
            ServerInformationModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedServerName, result.ServerName);
            Assert.Equal(expectedMaxPlayers, result.MaxPlayers);
            Assert.Equal(expectedPublicQueueLimit, result.PublicQueueLimit);
            Assert.Equal(expectedReserveSlots, result.ReserveSlots);
            Assert.Equal(expectedPlayerCount, result.PlayerCount);
            Assert.Equal(expectedA2sPlayerCount, result.A2sPlayerCount);
            Assert.Equal(expectedPublicQueue, result.PublicQueue);
            Assert.Equal(expectedReserveQueue, result.ReserveQueue);
            Assert.Equal(expectedCurrentLayer, result.CurrentLayer);
            Assert.Equal(expectedNextLayer, result.NextLayer);
            Assert.Equal(expectedTeamOne, result.TeamOne);
            Assert.Equal(expectedTeamTwo, result.TeamTwo);
            Assert.Equal(expectedMatchTimeout, result.MatchTimeout);
            Assert.Equal(expectedGameVersion, result.GameVersion);
        }

        private class ServerInformationTestCase
        {
            [JsonPropertyOrder(7)]
            public int ExpectedA2sPlayerCount { get; set; }

            [JsonPropertyOrder(10)]
            public string ExpectedCurrentLayer { get; set; }

            [JsonPropertyOrder(15)]
            public string ExpectedGameVersion { get; set; }

            [JsonPropertyOrder(14)]
            public double ExpectedMatchTimeout { get; set; }

            [JsonPropertyOrder(3)]
            public int ExpectedMaxPlayers { get; set; }

            [JsonPropertyOrder(11)]
            public string ExpectedNextLayer { get; set; }

            [JsonPropertyOrder(6)]
            public int ExpectedPlayerCount { get; set; }

            [JsonPropertyOrder(8)]
            public int ExpectedPublicQueue { get; set; }

            [JsonPropertyOrder(4)]
            public int ExpectedPublicQueueLimit { get; set; }

            [JsonPropertyOrder(9)]
            public int ExpectedReserveQueue { get; set; }

            [JsonPropertyOrder(5)]
            public int ExpectedReserveSlots { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedServerName { get; set; }

            [JsonPropertyOrder(12)]
            public string ExpectedTeamOne { get; set; }

            [JsonPropertyOrder(13)]
            public string ExpectedTeamTwo { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}