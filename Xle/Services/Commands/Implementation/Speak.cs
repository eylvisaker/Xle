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
            foreach (var evt in GameState.MapExtender.EventsAt(1).Where(x => x.Enabled))
            {
                bool handled = evt.Speak();

                if (handled)
                    return;
            }

            if (GameState.MapExtender.PlayerSpeak() == false)
            {
                PrintNoResponseMessage();
            }
        }

        protected void PrintNoResponseMessage()
        {
            TextArea.PrintLine("\n\nNo response.");
        }
    }
}
