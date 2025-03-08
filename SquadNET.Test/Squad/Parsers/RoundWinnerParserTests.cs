// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class RoundWinnerParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("round_winner.json");
        private readonly IParser<RoundWinnerEventModel> Parser;

        public RoundWinnerParserTests()
        {
            Parser = GetService<IParser<RoundWinnerEventModel>>();
        }

        public static IEnumerable<object[]> GetRoundWinnerTestData() =>
            LoadTestData<RoundWinnerTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetRoundWinnerTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID,
            string expectedWinner, string expectedLayer)
        {
            RoundWinnerEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedWinner, result.Winner);
            Assert.Equal(expectedLayer, result.Layer);
        }

        private class RoundWinnerTestCase
        {
            public string ExpectedChainID { get; set; }
            public string ExpectedLayer { get; set; }
            public string ExpectedTime { get; set; }
            public string ExpectedWinner { get; set; }
            public string Input { get; set; }
        }
    }
}