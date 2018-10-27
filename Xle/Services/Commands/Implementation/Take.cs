using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Services.Game;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;

namespace Xle.Services.Commands.Implementation
{
    public interface ITake : ICommand { }

    public class Take : Command, ITake
    {
        public IXleGameControl GameControl { get; set; }

        public override string Name
        {
            get { return "Take"; }
        }

        IEnumerable<IEventExtender> EventsAt(int borderSize)
        {
            return GameState.MapExtender.EventsAt(borderSize);
        }

        public override async Task Execute()
        {
            foreach (var evt in EventsAt(1).Where(x => x.Enabled))
            {
                if (await evt.Take())
                    return;
            }

            await TextArea.PrintLine("\n\nNothing to take.");

            await GameControl.WaitAsync(500);
        }
    }
}
