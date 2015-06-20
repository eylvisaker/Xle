namespace ERY.Xle.Services.Implementation.Commands
{
    public class Open : Command
    {
        private IXleGameControl gameControl;

        public Open(IXleGameControl gameControl)
        {
            this.gameControl = gameControl;
        }

        public override void Execute(GameState state)
        {
            if (GameState.MapExtender.PlayerOpen(state) == false)
            {
                TextArea.PrintLine("\n\nNothing opens.");

                gameControl.Wait(500);
            }
        }
    }
}
