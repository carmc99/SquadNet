// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ServerTickRateParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("server_tick_rate.json");
        private readonly IParser<ServerTickRateEventModel> Parser;

        public ServerTickRateParserTests()
        {
            Parser = GetService<IParser<ServerTickRateEventModel>>();
        }

        public static IEnumerable<object[]> GetServerTickRateTestData() =>
            LoadTestData<ServerTickRateTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetServerTickRateTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID, float expectedTickRate)
        {
            ServerTickRateEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedTickRate, result.TickRate);
        }

        private class ServerTickRateTestCase
        {
            public string ExpectedChainID { get; set; }
            public float ExpectedTickRate { get; set; }
            public string ExpectedTime { get; set; }
            public string Input { get; set; }
        }
    }
}