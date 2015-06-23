namespace ERY.Xle.Services.Implementation.Commands
{
    public class Climb : Command
    {
        public override void Execute()
        {
            if (GameState.MapExtender.PlayerClimb(GameState) == false)
            {
                TextArea.PrintLine("\n\nNothing to climb");
            }
        }
    }
}
