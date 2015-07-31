using System.Linq;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Speak : Command
    {
        public override string Name
        {
            get { return "Speak"; }
        }

        public override void Execute()
        {
            if (SpeakToEvent())
                return;

            PrintNoResponseMessage();
        }

        protected bool SpeakToEvent()
        {
            foreach (var evt in GameState.MapExtender.EventsAt(1).Where(x => x.Enabled))
            {
                bool handled = evt.Speak();

                if (handled)
                    return true;
            }

            return false;
        }

        protected void PrintNoResponseMessage()
        {
            TextArea.PrintLine("\n\nNo response.");
        }
    }
}
