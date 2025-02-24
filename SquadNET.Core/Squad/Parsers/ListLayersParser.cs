using System;
using System.Collections.Generic;
using System.Linq;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListLayersParser : IParser<List<LayerInfo>>
    {
        private const string Header = "List of available layers :";

        public List<LayerInfo> Parse(string input)
        {
            input = input.SanitizeInput().Replace(Header, "");

            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(layer => new LayerInfo { Name = layer.Trim() })
                        .ToList();
        }
    }
}
