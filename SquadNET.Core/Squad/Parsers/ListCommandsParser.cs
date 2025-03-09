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

            List<CommandModel> commands = [];

            foreach (string line in lines)
            {
                Match match = RegexPatternHelper.GetRegex<CommandModel>().Match(line);
                if (!match.Success)
                {
                    ParserLogger.LogInvalidInput(nameof(ListCommandsParser), input);
                    continue;
                }

                Dictionary<string, string> parsedValues = new()
                {
                    { "Name", match.Groups[1].Value.Trim() },
                    { "ParameterDescription", match.Groups[2].Success ? match.Groups[2].Value.Trim() : string.Empty },
                    { "Description", match.Groups[3].Value.Trim('(', ')').Trim() }
                };

                CommandModel command = DictionaryModelConverter.ConvertDictionaryToModel<CommandModel>(parsedValues);
                commands.Add(command);
            }

            return commands;
        }
    }
}