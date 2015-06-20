namespace ERY.Xle.Services.Implementation.Commands
{
    public class Rob : Command
    {
        public override void Execute(GameState state)
        {
            if (GameState.MapExtender.PlayerRob(state) == false)
            {
                TextArea.PrintLine("\n\nNothing to rob.");
            }
        }
    }
}
