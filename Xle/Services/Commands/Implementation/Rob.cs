using AgateLib;

namespace Xle.Services.Commands.Implementation
{
    public interface IRob : ICommand { }

    public class Rob : Command, IRob
    {
        public override string Name
        {
            get { return "Rob"; }
        }

        public override void Execute()
        {
            foreach (var evt in GameState.MapExtender.EventsAt(1))
            {
                bool handled = evt.Rob();

                if (handled)
                    return;
            }

            PrintNothingToRobMessage();
        }

        protected void PrintNothingToRobMessage()
        {
            TextArea.PrintLine("\n\nNothing to rob.");
        }
    }
}
