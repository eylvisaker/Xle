using System.Linq;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.Services.Game;

namespace Xle.Services.Commands.Implementation
{
    public interface IOpen : ICommand { }

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
                if (evt.Open())
                    return true;
            }

            return false;
        }
    }
}
