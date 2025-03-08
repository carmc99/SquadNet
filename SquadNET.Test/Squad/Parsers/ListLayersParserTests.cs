// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListLayersParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_layers.json");
        private readonly IParser<List<LayerModel>> Parser;

        public ListLayersParserTests()
        {
            Parser = GetService<IParser<List<LayerModel>>>();
        }

        public static IEnumerable<object[]> GetListLayersTestData() =>
            LoadTestData<ListLayersTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListLayersTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<LayerModel> expectedLayers)
        {
            List<LayerModel> result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedLayers.Count, result.Count);

            for (int i = 0; i < expectedLayers.Count; i++)
            {
                Assert.Equal(expectedLayers[i].Name, result[i].Name);
            }
        }

        private class ListLayersTestCase
        {
            public List<LayerModel> ExpectedLayers { get; set; }
            public string Input { get; set; }
        }
    }
}