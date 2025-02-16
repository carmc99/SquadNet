﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: ADMIN COMMAND: Message broadcasted <(.+)> from (.+)")]
    public class AdminBroadcast
    {
        public RawData Raw { get; set; }
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
    }
}
