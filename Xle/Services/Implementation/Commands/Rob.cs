namespace ERY.Xle.Services.Implementation.Commands
{
    public class Rob : Command
    {
        public override void Execute()
        {
            if (GameState.MapExtender.PlayerRob(GameState) == false)
            {
                TextArea.PrintLine("\n\nNothing to rob.");
            }
        }
    }
}
