using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListCommandsParser : ICommandParser<List<CommandInfo>>
    {
        public List<CommandInfo> Parse(string input)
        {
            input = input.SanitizeInput();
            string[] lines = input.Split('\n');

            if (lines.Length <= 1)
            {
                return [];
            }

            List<CommandInfo> commands = [];

            foreach (string line in lines[1..])
            {
                Match match = RegexPatternHelper.GetRegex<CommandInfo>().Match(line);
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

                CommandInfo command = DictionaryModelConverter.ConvertDictionaryToModel<CommandInfo>(parsedValues);
                commands.Add(command); 
            }

            return commands;
        }
    }
}
