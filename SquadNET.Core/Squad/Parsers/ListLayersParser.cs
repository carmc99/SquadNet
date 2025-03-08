using System;
using System.Collections.Generic;
using System.Linq;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListLayersParser : IParser<List<LayerModel>>
    {
        private const string Header = "List of available layers :";

        public List<LayerModel> Parse(string input)
        {
            input = input.SanitizeInput().Replace(Header, "");

            return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(layer => new LayerModel { Name = layer.Trim() })
                        .ToList();
        }
    }
}
