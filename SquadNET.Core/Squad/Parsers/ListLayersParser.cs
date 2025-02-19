using System;
using System.Collections.Generic;
using System.Linq;
using SquadNET.Core;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListLayersParser : ICommandParser<List<string>>
    {
        private const string Header = "List of available layers :";

        public List<string> Parse(string input)
        {
            input = input.SanitizeInput().Replace(Header, "");

            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(layer => layer.Trim())
                        .ToList();
        }
    }
}
