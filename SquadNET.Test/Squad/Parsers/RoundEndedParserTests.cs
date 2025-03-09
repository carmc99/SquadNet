// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class RoundEndedParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("round_ended.json");
        private readonly IParser<RoundEndedEventModel> Parser;

        public RoundEndedParserTests()
        {
            Parser = GetService<IParser<RoundEndedEventModel>>();
        }

        public static IEnumerable<object[]> GetRoundEndedTestData() =>
            LoadTestData<RoundEndedTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetRoundEndedTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime)
        {
            RoundEndedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
        }

        private class RoundEndedTestCase
        {
            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}