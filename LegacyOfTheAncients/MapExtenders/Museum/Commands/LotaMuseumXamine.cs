﻿using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Maps.Museums;
using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients.MapExtenders.Museum.Commands
{
    [Transient("LotaMuseumXamine")]
    public class LotaMuseumXamine : Xamine
    {
        MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (InteractWithDisplay())
                return;

            TextArea.PrintLine("You are in an ancient museum.");
        }

        private bool InteractWithDisplay()
        {
            return Museum.InteractWithDisplay();
        }
    }
}