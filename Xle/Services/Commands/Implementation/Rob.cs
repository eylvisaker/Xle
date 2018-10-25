using AgateLib;
using System.Threading.Tasks;

namespace Xle.Services.Commands.Implementation
{
    public interface IRob : ICommand { }

    public class Rob : Command, IRob
    {
        public override string Name
        {
            get { return "Rob"; }
        }

        public override async Task Execute()
        {
            foreach (var evt in GameState.MapExtender.EventsAt(1))
            {
                bool handled = await evt.Rob();

                if (handled)
                    return;
            }

            await PrintNothingToRobMessage();
        }

        protected Task PrintNothingToRobMessage()
        {
            return TextArea.PrintLine("\n\nNothing to rob.");
        }
    }
}
