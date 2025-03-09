// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class AdminBroadcastParser : IParser<AdminBroadcastEventModel>
    {
        private static readonly Regex AdminBroadcastRegex = RegexPatternHelper.GetRegex<AdminBroadcastEventModel>();

        public AdminBroadcastEventModel Parse(string input)
        {
            input = input.SanitizeInput();
            Match match = AdminBroadcastRegex.Match(input);

            if (!match.Success || match.Groups.Count < 5)
            {
                ParserLogger.LogInvalidInput(nameof(AdminBroadcastParser), input);
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Time", match.Groups[1].Value },
                { "ChainID", match.Groups[2].Value },
                { "Message", match.Groups[3].Value },
                { "From", match.Groups[4].Value }
            };

            return DictionaryModelConverter.ConvertDictionaryToModel<AdminBroadcastEventModel>(parsedValues);
        }
    }
}