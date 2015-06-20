namespace ERY.Xle.Services.Implementation.Commands
{
    public class Take : Command
    {
        private IXleGameControl gameControl;

        public Take(IXleGameControl gameControl)
        {
            this.gameControl = gameControl;
        }

        public override void Execute(GameState state)
        {
            if (state.MapExtender.PlayerTake(state) == false)
            {
                TextArea.PrintLine("\n\nNothing to take.");

                gameControl.Wait(500);
            }
        }
    }
}
