// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Test.Squad.Core;
using System.Text.Json.Serialization;

namespace SquadNET.Tests.Squad.Parsers
{
    public class DeployableDamageParserTests : SquadTestBase
    {
        private static readonly string TestDataFile = GetTestDataFilePath("deployable_damaged.json");
        private readonly IParser<DeployableDamagedEventModel> Parser;

        public DeployableDamageParserTests()
        {
            Parser = GetService<IParser<DeployableDamagedEventModel>>();
        }

        public static IEnumerable<object[]> GetDeployableDamageTestData() =>
            LoadTestData<DeployableDamageTestCase>(TestDataFile);

        [Theory]
        [MemberData(nameof(GetDeployableDamageTestData))]
        public void GivenLogInput_WhenParseIsCalled_ThenResultShouldBeValid(
            string input, string expectedTime, string expectedChainID,
            string expectedDeployable, float expectedDamage, string expectedWeapon,
            string expectedAttackerName, string expectedDamageType, float expectedHealthRemaining)
        {
            DeployableDamagedEventModel result = Parser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(expectedTime, result.Time);
            Assert.Equal(expectedChainID, result.ChainID);
            Assert.Equal(expectedDeployable, result.Deployable);
            Assert.Equal(expectedDamage, result.Damage);
            Assert.Equal(expectedWeapon, result.Weapon);
            Assert.Equal(expectedAttackerName, result.AttackerName);
            Assert.Equal(expectedDamageType, result.DamageType);
            Assert.Equal(expectedHealthRemaining, result.HealthRemaining);
        }

        private class DeployableDamageTestCase
        {
            [JsonPropertyOrder(7)]
            public string ExpectedAttackerName { get; set; }

            [JsonPropertyOrder(3)]
            public string ExpectedChainID { get; set; }

            [JsonPropertyOrder(5)]
            public float ExpectedDamage { get; set; }

            [JsonPropertyOrder(8)]
            public string ExpectedDamageType { get; set; }

            [JsonPropertyOrder(4)]
            public string ExpectedDeployable { get; set; }

            [JsonPropertyOrder(9)]
            public float ExpectedHealthRemaining { get; set; }

            [JsonPropertyOrder(2)]
            public string ExpectedTime { get; set; }

            [JsonPropertyOrder(6)]
            public string ExpectedWeapon { get; set; }

            [JsonPropertyOrder(1)]
            public string Input { get; set; }
        }
    }
}