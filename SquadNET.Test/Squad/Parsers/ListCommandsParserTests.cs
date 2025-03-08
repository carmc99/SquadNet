// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;

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
            List<CommandModel> result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedCommands.Count, result.Count);

            for (int i = 0; i < expectedCommands.Count; i++)
            {
                Assert.Equal(expectedCommands[i].Name, result[i].Name);
                Assert.Equal(expectedCommands[i].ParameterDescription, result[i].ParameterDescription);
                Assert.Equal(expectedCommands[i].Description, result[i].Description);
            }
        }

        private class ListCommandsTestCase
        {
            public List<CommandModel> ExpectedCommands { get; set; }
            public string Input { get; set; }
        }
    }
}