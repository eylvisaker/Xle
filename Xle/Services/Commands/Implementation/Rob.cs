namespace Xle.Services.Commands.Implementation
{
    public class Rob : Command
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
