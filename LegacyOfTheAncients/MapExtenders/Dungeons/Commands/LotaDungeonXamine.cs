﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Dungeons.Commands;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Dungeons.Commands
{
    [ServiceName("LotaDungeonXamine")]
    public class LotaDungeonXamine : DungeonXamine
    {
        private bool printLevelDuringXamine;

        public XleOptions Options { get; set; }

        protected override bool PrintLevelDuringXamine
        {
            get { return Options.EnhancedGameplay; }
        }
    }
}