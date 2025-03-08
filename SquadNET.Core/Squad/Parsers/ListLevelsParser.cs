using System;
using System.Collections.Generic;
using System.Linq;
using SquadNET.Core;
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
