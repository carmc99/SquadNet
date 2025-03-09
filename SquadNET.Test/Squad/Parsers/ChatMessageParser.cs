// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ChatMessageParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("chat_messages.json");
        private readonly IParser<ChatMessageEventModel> Parser;

        public ChatMessageParserTests()
        {
            Parser = GetService<IParser<ChatMessageEventModel>>();
        }

        public static IEnumerable<object[]> GetChatMessagesTestData() =>
            LoadTestData<ChatMessageTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetChatMessagesTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedChannel, string expectedEosId,
            ulong expectedSteamId, string expectedPlayerName, string expectedMessage)
        {
            ChatMessageEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedChannel, result.Channel.ToString());
            Assert.Equal(expectedEosId, result.CreatorIds.EosId);
            Assert.Equal(expectedSteamId, result.CreatorIds.SteamId);
            Assert.Equal(expectedPlayerName, result.PlayerName);
            Assert.Equal(expectedMessage, result.Message);
        }

        private class ChatMessageTestCase
        {
            [JsonPropertyOrder(2)]
            public string ExpectedChannel { get; set; }

            [JsonPropertyOrder(3)]
            public string ExpectedEosId { get; set; }

            [JsonPropertyOrder(6)]
            public string ExpectedMessage { get; set; }

            [JsonPropertyOrder(5)]
            public string ExpectedPlayerName { get; set; }

            [JsonPropertyOrder(4)]
            public ulong ExpectedSteamId { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}