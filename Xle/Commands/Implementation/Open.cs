﻿using AgateLib;
using System.Linq;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.Game;

namespace Xle.Commands.Implementation
{
    public interface IOpen : ICommand { }

    [Transient("Open")]
    public class Open : Command, IOpen
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

        public override async Task Execute()
        {
            if (await OpenEvent())
                return;

            await TextArea.PrintLine("\n\nNothing opens.");

            await GameControl.WaitAsync(500);
        }

        protected async Task<bool> OpenEvent()
        {
            foreach (var evt in MapExtender.EventsAt(1).Where(x => x.Enabled))
            {
                if (await evt.Open())
                    return true;
            }

            return false;
        }
    }
}
