// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class AdminBroadcastParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("admin_broadcast_messages.json");
        private readonly IParser<AdminBroadcastEventModel> Parser;

        public AdminBroadcastParserTests()
        {
            Parser = GetService<IParser<AdminBroadcastEventModel>>();
        }

        public static IEnumerable<object[]> GetAdminBroadcastTestData() =>
            LoadTestData<AdminBroadcastTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetAdminBroadcastTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID,
            string expectedMessage, string expectedFrom)
        {
            AdminBroadcastEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedMessage, result.Message);
            Assert.Equal(expectedFrom, result.From);
        }

        private class AdminBroadcastTestCase
        {
            [JsonPropertyOrder(3)]
            public string ExpectedChainID { get; set; }

            [JsonPropertyOrder(5)]
            public string ExpectedFrom { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedMessage { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}