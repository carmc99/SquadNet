namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^ID: ([0-9]+) \| SteamID: ([0-9]+) \| Since Disconnect: ([0-9]+)m\.([0-9]+)s \| Name: (.*)$")]
    public class PlayerDisconnectedInfo
    {
        public int Id { get; set; }
        public ulong SteamId { get; set; }
        public TimeSpan DisconnectedSince { get; set; }
        public string Name { get; set; }

        public bool Equals(
            PlayerDisconnectedInfo? other
        )
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && SteamId == other.SteamId && DisconnectedSince.Equals(other.DisconnectedSince) && Name == other.Name;
        }

        public override bool Equals(
            object? obj
        )
        {
            return ReferenceEquals(this, obj) || obj is PlayerDisconnectedInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, SteamId, DisconnectedSince, Name);
        }
    }
}
