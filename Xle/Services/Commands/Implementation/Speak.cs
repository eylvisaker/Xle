using System.Linq;
using System.Threading.Tasks;

namespace Xle.Services.Commands.Implementation
{
    public interface ISpeak : ICommand { }
    public class Speak : Command, ISpeak
    {
        public override string Name
        {
            get { return "Speak"; }
        }

        public override async Task Execute()
        {
            if (await SpeakToEvent())
                return;

            await PrintNoResponseMessage();
        }

        protected async Task<bool> SpeakToEvent()
        {
            foreach (var evt in GameState.MapExtender.EventsAt(1).Where(x => x.Enabled))
            {
                bool handled = await evt.Speak();

                if (handled)
                    return true;
            }

            return false;
        }

        protected async Task PrintNoResponseMessage()
        {
            await TextArea.PrintLine("\n\nNo response.");
        }
    }
}
