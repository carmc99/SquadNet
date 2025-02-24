using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class ListPlayersParser : IParser<ListPlayerModel>
    {
        private const string ActivePlayersHeader = "----- Active Players -----";
        private const string DisconnectedPlayersHeader = "----- Recently Disconnected Players [Max of 15] -----";

        public ListPlayerModel Parse(string input)
        {
            input = input
                .SanitizeInput()
                .Replace(ActivePlayersHeader, "")
                .Replace(DisconnectedPlayersHeader, "");

            string[] lines = input.Split('\n');
            ListPlayerModel result = new([], []);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                PlayerConnectedInfo playerConnectedInfo = ParsePlayerConnected(line);
                PlayerDisconnectedInfo playerDisconnectedInfo = ParsePlayerDisconnected(line);

                if (playerConnectedInfo != null)
                {
                    result.ActivePlayers.Add(playerConnectedInfo);
                }

                if (playerDisconnectedInfo != null)
                {
                    result.DisconnectedPlayers.Add(playerDisconnectedInfo);
                }
            }

            return result;
        }

        private PlayerConnectedInfo ParsePlayerConnected(string line)
        {
            Match match = RegexPatternHelper.GetRegex<PlayerConnectedInfo>().Match(line);
            if (!match.Success || match.Groups.Count < 9)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Id", match.Groups[1].Value },
                { "Name", match.Groups[4].Value },
                { "Team", match.Groups[5].Value },
                { "SquadId", match.Groups[6].Value },
                { "IsLeader", match.Groups[7].Value },
                { "Role", match.Groups[8].Value }
            };

            // Extraer los identificadores EOS y Steam
            string eosId = match.Groups[2].Value;
            ulong steamId = ulong.Parse(match.Groups[3].Value);
            CreatorOnlineIds creatorIds = new(eosId, steamId);

            PlayerConnectedInfo result = DictionaryModelConverter.ConvertDictionaryToModel<PlayerConnectedInfo>(parsedValues);
            result.CreatorIds = creatorIds;

            return result;
        }

        private PlayerDisconnectedInfo ParsePlayerDisconnected(string line)
        {
            Match match = RegexPatternHelper.GetRegex<PlayerDisconnectedInfo>().Match(line);
            if (!match.Success || match.Groups.Count < 6)
            {
                return null;
            }

            Dictionary<string, string> parsedValues = new()
            {
                { "Id", match.Groups[1].Value },
                { "SteamId", match.Groups[2].Value },
                { "Minutes", match.Groups[3].Value },
                { "Seconds", match.Groups[4].Value },
                { "Name", match.Groups[5].Value }
            };

            PlayerDisconnectedInfo result = DictionaryModelConverter.ConvertDictionaryToModel<PlayerDisconnectedInfo>(parsedValues);

            return result;
        }
    }
}