using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Parsers
{
    public class ListPlayersParser : ICommandParser<ListPlayerModel>
    {
        private const string ActivePlayersHeader = "----- Active Players -----";
        private const string DisconnectedPlayersHeader = "----- Recently Disconnected Players [Max of 15] -----";
        public ListPlayerModel Parse(
            string input
        )
        {
            input = input
                .Replace(ActivePlayersHeader, "")
                .Replace(DisconnectedPlayersHeader, "")
                .Replace("\r\n", "\n");
            string[] lines = input.Split("\n");

            ListPlayerModel result = new(
                [],
                []
            );

            foreach (string line in lines)
            {
                var match = RegexPatternHelper.GetRegex<PlayerConnectedInfo>().Match(line);
                if (match.Success)
                {
                    string squadIdStr = match.Groups[5].Value;
                    int? squadId = squadIdStr == "N/A" ? (int?)null : int.Parse(squadIdStr);

                    result.ActivePlayers.Add(
                        new PlayerConnectedInfo(
                            int.Parse(match.Groups[1].Value),
                            ulong.Parse(match.Groups[2].Value),
                            match.Groups[3].Value,
                            (TeamId)int.Parse(match.Groups[4].Value),
                            match.Groups[6].Value == "True",
                            match.Groups[7].Value,
                            squadId
                        )
                    );
                    continue;
                }

                match = RegexPatternHelper.GetRegex<PlayerDisconnectedInfo>().Match(line);
                if (!match.Success)
                {
                    continue;
                }

                result.DisconnectedPlayers.Add(
                    new PlayerDisconnectedInfo(
                        int.Parse(match.Groups[1].Value),
                        ulong.Parse(match.Groups[2].Value),
                        new TimeSpan(0, 0, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)),
                        match.Groups[5].Value
                    )
                );
            }

            return result;
        }
    }
}
