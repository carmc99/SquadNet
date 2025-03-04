﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: Player:(.+) ActualDamage=([0-9.]+) from (.+) \(Online IDs:([^|]+)\| Player Controller ID: ([^ ]+)\)caused by ([A-z_0-9-]+)_C")]
    public class PlayerDamagedEventModel : ISquadEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string VictimName { get; set; }
        public float Damage { get; set; }
        public string AttackerName { get; set; }
        public string AttackerController { get; set; }
        public string Weapon { get; set; }
        public Dictionary<string, string> AttackerIDs { get; set; } = [];
    }
}
