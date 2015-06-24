using ERY.Xle.Services.Game;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Take : Command
    {
        private IXleGameControl gameControl;

        public Take(IXleGameControl gameControl)
        {
            this.gameControl = gameControl;
        }

        public override void Execute()
        {
            if (GameState.MapExtender.PlayerTake(GameState) == false)
            {
                TextArea.PrintLine("\n\nNothing to take.");

                gameControl.Wait(500);
            }
        }
    }
}
