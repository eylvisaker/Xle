using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ERY.Xle.Services.Game;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Take : Command
    {
        public IXleGameControl GameControl { get; set; }

        public override string Name
        {
            get { return "Take"; }
        }

        IEnumerable<EventExtender> EventsAt(int borderSize)
        {
            return GameState.MapExtender.EventsAt(borderSize);
        }

        public override void Execute()
        {
            foreach (var evt in EventsAt(1).Where(x => x.Enabled))
            {
                if (evt.Take())
                    return;
            }

            TextArea.PrintLine("\n\nNothing to take.");

            GameControl.Wait(500);
        }
    }
}
