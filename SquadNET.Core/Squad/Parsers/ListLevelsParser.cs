// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListLevelsParser : IParser<List<LevelModel>>
    {
        private const string Header = "List of available levels :";

        public List<LevelModel> Parse(string input)
        {
            input = input.SanitizeInput().Replace(Header, "");

            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(level => new LevelModel { Name = level.Trim() })
                        .ToList();
        }
    }
}