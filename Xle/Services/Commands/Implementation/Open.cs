﻿using System.Linq;

using Xle.Maps;
using Xle.Services.Game;

namespace Xle.Services.Commands.Implementation
{
    public class Open : Command
    {
        public IXleGameControl GameControl { get; set; }

        protected IMapExtender MapExtender
        {
            get { return GameState.MapExtender; }
        }

        public override string Name
        {
            get { return "Open"; }
        }

        public override void Execute()
        {
            if (OpenEvent())
                return;

            TextArea.PrintLine("\n\nNothing opens.");

            GameControl.Wait(500);
        }

        protected bool OpenEvent()
        {
            foreach (var evt in MapExtender.EventsAt(1).Where(x => x.Enabled))
            {
                if (evt.Open())
                    return true;
            }

            return false;
        }
    }
}
