// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using System.Text.Json;

internal class ServerInformationParser : IParser<ServerInformationInfo>
{
    public ServerInformationInfo Parse(string input)
    {
        input = input.SanitizeInput();

        Dictionary<string, JsonElement> rawData =
            JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(input);

        Dictionary<string, string> data = [];
        foreach (KeyValuePair<string, JsonElement> kvp in rawData)
        {
            JsonElement element = kvp.Value;
            string stringValue = element.ValueKind == JsonValueKind.String
                ? element.GetString()
                : element.GetRawText();
            data[kvp.Key] = stringValue;
        }

        Dictionary<string, string> parsedValues = [];
        string GetValueSafely(string key) => data.TryGetValue(key, out string val) ? val : string.Empty;

        parsedValues["ServerName"] = GetValueSafely("ServerName_s");
        parsedValues["MaxPlayers"] = GetValueSafely("MaxPlayers");
        parsedValues["PublicQueueLimit"] = GetValueSafely("PublicQueueLimit_I");
        parsedValues["ReserveSlots"] = GetValueSafely("PlayerReserveCount_I");
        parsedValues["PlayerCount"] = GetValueSafely("PlayerCount_I");
        parsedValues["A2sPlayerCount"] = GetValueSafely("PlayerCount_I");
        parsedValues["PublicQueue"] = GetValueSafely("PublicQueue_I");
        parsedValues["ReserveQueue"] = GetValueSafely("ReservedQueue_I");
        parsedValues["CurrentLayer"] = GetValueSafely("MapName_s");
        parsedValues["NextLayer"] = GetValueSafely("NextLayer_s");
        parsedValues["TeamOne"] = GetValueSafely("TeamOne_s");
        parsedValues["TeamTwo"] = GetValueSafely("TeamTwo_s");
        parsedValues["MatchTimeout"] = GetValueSafely("MatchTimeout_d");
        parsedValues["GameVersion"] = GetValueSafely("GameVersion_s");

        string playtimeStr = GetValueSafely("PLAYTIME_I");
        int playtime = 0;
        if (int.TryParse(playtimeStr, out int parsedPlaytime))
        {
            playtime = parsedPlaytime;
        }
        DateTime matchStart = GetMatchStartTimeByPlaytime(playtime);
        parsedValues["MatchStartTime"] = matchStart.ToString("o");

        ServerInformationInfo info =
            DictionaryModelConverter.ConvertDictionaryToModel<ServerInformationInfo>(parsedValues);

        return info;
    }

    /// <summary>
    /// Calcula la hora de inicio del match a partir de los segundos transcurridos (playtime).
    /// </summary>
    private DateTime GetMatchStartTimeByPlaytime(int playtime)
    {
        return DateTime.UtcNow.AddSeconds(-playtime);
    }
}