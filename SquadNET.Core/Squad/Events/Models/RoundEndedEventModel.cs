﻿namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogGameState: Match State Changed from InProgress to WaitingPostMatch")]
    public class RoundEndedEventModel : ISquadEventData
    {
        public string Time { get; set; }
        public RoundWinnerEventModel Winner { get; set; }
        public RoundTicketsEventModel Loser { get; set; }
    }
}
