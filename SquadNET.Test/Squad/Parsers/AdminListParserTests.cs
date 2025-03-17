// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class AdminListParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("admin_list.json");
        private readonly IParser<AdminListModel> Parser;

        public AdminListParserTests()
        {
            Parser = GetService<IParser<AdminListModel>>();
        }

        public static IEnumerable<object[]> GetAdminListTestData() =>
            LoadTestData<AdminListTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetAdminListTestData))]
        public void GivenConfigFile_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, List<GroupModel> expectedGroups, List<AdminModel> expectedAdmins)
        {
            AdminListModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedGroups.Count, result.Groups.Count);
            Assert.Equal(expectedAdmins.Count, result.Admins.Count);

            foreach (GroupModel expectedGroup in expectedGroups)
            {
                Assert.Contains(result.Groups, g =>
                    g.GroupID.Trim() == expectedGroup.GroupID.Trim() &&
                    g.GroupPerms.Trim() == expectedGroup.GroupPerms.Trim());
            }

            foreach (AdminModel expectedAdmin in expectedAdmins)
            {
                Assert.Contains(result.Admins, a =>
                    a.AdminID.Trim() == expectedAdmin.AdminID.Trim() &&
                    a.GroupID.Trim() == expectedAdmin.GroupID.Trim());
            }
        }

        private class AdminListTestCase
        {
            [JsonPropertyOrder(3)]
            public List<AdminModel> ExpectedAdmins { get; set; }

            [JsonPropertyOrder(2)]
            public List<GroupModel> ExpectedGroups { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}