// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;

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
            public int ExpectedA2sPlayerCount { get; set; }
            public string ExpectedCurrentLayer { get; set; }
            public string ExpectedGameVersion { get; set; }
            public double ExpectedMatchTimeout { get; set; }
            public int ExpectedMaxPlayers { get; set; }
            public string ExpectedNextLayer { get; set; }
            public int ExpectedPlayerCount { get; set; }
            public int ExpectedPublicQueue { get; set; }
            public int ExpectedPublicQueueLimit { get; set; }
            public int ExpectedReserveQueue { get; set; }
            public int ExpectedReserveSlots { get; set; }
            public string ExpectedServerName { get; set; }
            public string ExpectedTeamOne { get; set; }
            public string ExpectedTeamTwo { get; set; }
            public string Input { get; set; }
        }
    }
}