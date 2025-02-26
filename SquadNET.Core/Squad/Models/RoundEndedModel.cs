

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogGameState: Match State Changed from InProgress to WaitingPostMatch")]
    public class RoundEndedModel : IEventData
    {
        public string Time { get; set; }
        public RoundWinnerModel Winner { get; set; }
        public RoundTicketsModel Loser { get; set; }
    }
}
