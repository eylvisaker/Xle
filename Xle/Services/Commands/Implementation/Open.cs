using ERY.Xle.Services.Game;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Open : Command
    {
        public IXleGameControl GameControl { get; set; }

        public override string Name
        {
            get { return "Open"; }
        }

        public override void Execute()
        {
            if (GameState.MapExtender.PlayerOpen(GameState) == false)
            {
                TextArea.PrintLine("\n\nNothing opens.");

                GameControl.Wait(500);
            }
        }
    }
}
