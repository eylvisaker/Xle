namespace ERY.Xle.Services.Implementation.Commands
{
    public class Climb : Command
    {
        public override void Execute(GameState state)
        {
            if (GameState.MapExtender.PlayerClimb(state) == false)
            {
                TextArea.PrintLine("\n\nNothing to climb");
            }
        }
    }
}
