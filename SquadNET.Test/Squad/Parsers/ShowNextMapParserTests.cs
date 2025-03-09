// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ShowNextMapParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("show_next_map.json");
        private readonly IParser<NextMapModel> Parser;

        public ShowNextMapParserTests()
        {
            Parser = GetService<IParser<NextMapModel>>();
        }

        public static IEnumerable<object[]> GetShowNextMapTestData() =>
            LoadTestData<ShowNextMapTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetShowNextMapTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedLevel, string expectedName)
        {
            NextMapModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedLevel, result.Level);
            Assert.Equal(expectedName, result.Name);
        }

        private class ShowNextMapTestCase
        {
            [JsonPropertyOrder(2)]
            public string ExpectedLevel { get; set; }

            [JsonPropertyOrder(3)]
            public string ExpectedName { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}