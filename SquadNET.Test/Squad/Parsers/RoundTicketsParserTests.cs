// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class RoundTicketsParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("round_tickets.json");
        private readonly IParser<RoundTicketsEventModel> Parser;

        public RoundTicketsParserTests()
        {
            Parser = GetService<IParser<RoundTicketsEventModel>>();
        }

        public static IEnumerable<object[]> GetRoundTicketsTestData() =>
            LoadTestData<RoundTicketsTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetRoundTicketsTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID, int expectedTeam,
            string expectedSubfaction, string expectedFaction, string expectedAction,
            int expectedTickets, string expectedLayer, string expectedLevel)
        {
            RoundTicketsEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedTeam, result.Team);
            Assert.Equal(expectedSubfaction, result.Subfaction);
            Assert.Equal(expectedFaction, result.Faction);
            Assert.Equal(expectedAction, result.Action);
            Assert.Equal(expectedTickets, result.Tickets);
            Assert.Equal(expectedLayer, result.Layer);
            Assert.Equal(expectedLevel, result.Level);
        }

        private class RoundTicketsTestCase
        {
            [JsonPropertyOrder(7)]
            public string ExpectedAction { get; set; }

            [JsonPropertyOrder(3)]
            public string ExpectedChainID { get; set; }

            [JsonPropertyOrder(6)]
            public string ExpectedFaction { get; set; }

            [JsonPropertyOrder(9)]
            public string ExpectedLayer { get; set; }

            [JsonPropertyOrder(10)]
            public string ExpectedLevel { get; set; }

            [JsonPropertyOrder(5)]
            public string ExpectedSubfaction { get; set; }

            [JsonPropertyOrder(4)]
            public int ExpectedTeam { get; set; }

            [JsonPropertyOrder(8)]
            public int ExpectedTickets { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}