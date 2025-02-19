using System;
using System.Collections.Generic;
using System.Linq;
using SquadNET.Core;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListLevelsParser : ICommandParser<List<string>>
    {
        private const string Header = "List of available levels :";

        public List<string> Parse(string input)
        {
            input = input.SanitizeInput().Replace(Header, "");

            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(level => level.Trim())
                        .ToList();
        }
    }
}
