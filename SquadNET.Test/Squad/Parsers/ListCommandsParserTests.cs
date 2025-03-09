// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListCommandsParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_commands.json");
        private readonly IParser<List<CommandModel>> Parser;

        public ListCommandsParserTests()
        {
            Parser = GetService<IParser<List<CommandModel>>>();
        }

        public static IEnumerable<object[]> GetListCommandsTestData() =>
            LoadTestData<ListCommandsTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListCommandsTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<CommandModel> expectedCommands)
        {
            // Act
            List<CommandModel> result = Parser.Parse(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCommands.Count, result.Count);

            foreach (CommandModel expectedCommand in expectedCommands)
            {
                CommandModel resultCommand = result.FirstOrDefault(c => c.Name == expectedCommand.Name);
                Assert.NotNull(resultCommand);
                Assert.Equal(expectedCommand.ParameterDescription, resultCommand.ParameterDescription);
            }
        }

        private class ListCommandsTestCase
        {
            [JsonPropertyOrder(2)]
            public List<CommandModel> ExpectedCommands { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}