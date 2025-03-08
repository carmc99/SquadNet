// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    public class CreatorOnlineModel
    {
        public CreatorOnlineModel(string eosId, ulong steamId)
        {
            EosId = eosId ?? throw new ArgumentNullException(nameof(eosId));
            SteamId = steamId;
        }

        public string EosId { get; private set; }
        public ulong SteamId { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="CreatorOnlineModel"/> from a string of identifiers in the format 'EOS: steam:'.
        /// </summary>
        /// <param name="onlineIds">String of identifiers in the format 'EOS: XXXX steam: XXXX'.</param>
        /// <returns>An instance of <see cref="CreatorOnlineModel"/> with the parsed values.</returns>

        public static CreatorOnlineModel FromString(string onlineIds)
        {
            if (string.IsNullOrWhiteSpace(onlineIds))
            {
                throw new ArgumentException("Online IDs string cannot be null or empty.", nameof(onlineIds));
            }

            string eosId = string.Empty;
            ulong steamId = 0;

            foreach (string part in onlineIds.Split('|'))
            {
                string[] segments = part.Split(':', 2, StringSplitOptions.TrimEntries);
                if (segments.Length != 2) continue;

                if (segments[0].Equals("EOS", StringComparison.OrdinalIgnoreCase))
                {
                    eosId = segments[1];
                }
                else if (segments[0].Equals("steam", StringComparison.OrdinalIgnoreCase) && ulong.TryParse(segments[1], out ulong parsedSteamId))
                {
                    steamId = parsedSteamId;
                }
            }

            return new CreatorOnlineModel(eosId, steamId);
        }

        public bool Equals(CreatorOnlineModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EosId == other.EosId && SteamId == other.SteamId;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is CreatorOnlineModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EosId, SteamId);
        }

        public override string ToString()
        {
            return $"EOS: {EosId}, Steam: {SteamId}";
        }
    }
}