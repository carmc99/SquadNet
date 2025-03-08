// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListCommandsParser : IParser<List<CommandModel>>
    {
        public List<CommandModel> Parse(string input)
        {
            input = input.SanitizeInput();
            string[] lines = input.Split('\n');

            if (lines.Length <= 1)
            {
                return [];
            }

            List<CommandModel> commands = [];

            foreach (string line in lines[1..])
            {
                Match match = RegexPatternHelper.GetRegex<CommandModel>().Match(line);
                if (!match.Success)
                {
                    continue;
                }

                Dictionary<string, string> parsedValues = new()
                {
                    { "Name", match.Groups[1].Value },
                    { "ParameterDescription", match.Groups[2].Value },
                    { "Description", match.Groups[3].Value.Trim('(', ')') }
                };

                CommandModel command = DictionaryModelConverter.ConvertDictionaryToModel<CommandModel>(parsedValues);
                commands.Add(command);
            }

            return commands;
        }
    }
}