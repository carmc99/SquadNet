﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogNet: Join succeeded: (.+)")]
    public class PlayerJoinSucceeded
    {
        public RawData Raw { get; set; }
        public string Time { get; set; }
        public int ChainID { get; set; }
        public string PlayerSuffix { get; set; }
        // Se combinan los datos del jugador obtenidos previamente (por ejemplo, playercontroller, ip)
        public string PlayerController { get; set; }
        public string IP { get; set; }
        /// <summary>
        /// Propiedades adicionales derivadas de los IDs.
        /// </summary>
        public Dictionary<string, string> AdditionalIDs { get; set; } = [];
    }
}
