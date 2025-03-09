// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class ListSquadsParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("list_squads.json");
        private readonly IParser<List<SquadModel>> Parser;

        public ListSquadsParserTests()
        {
            Parser = GetService<IParser<List<SquadModel>>>();
        }

        public static IEnumerable<object[]> GetListSquadsTestData() =>
            LoadTestData<ListSquadsTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetListSquadsTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<SquadModel> expectedSquads)
        {
            List<SquadModel> result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedSquads.Count, result.Count);

            for (int i = 0; i < expectedSquads.Count; i++)
            {
                Assert.Equal(expectedSquads[i].Id, result[i].Id);
                Assert.Equal(expectedSquads[i].TeamId, result[i].TeamId);
                Assert.Equal(expectedSquads[i].TeamName, result[i].TeamName);
                Assert.Equal(expectedSquads[i].Name, result[i].Name);
                Assert.Equal(expectedSquads[i].Size, result[i].Size);
                Assert.Equal(expectedSquads[i].IsLocked, result[i].IsLocked);
                Assert.Equal(expectedSquads[i].CreatorName, result[i].CreatorName);
                Assert.Equal(expectedSquads[i].CreatorIds.EosId, result[i].CreatorIds.EosId);
                Assert.Equal(expectedSquads[i].CreatorIds.SteamId, result[i].CreatorIds.SteamId);
            }
        }

        private class ListSquadsTestCase
        {
            [JsonPropertyOrder(2)]
            public List<SquadModel> ExpectedSquads { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}