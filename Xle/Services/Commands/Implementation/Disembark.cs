namespace ERY.Xle.Services.Commands.Implementation
{
    public class Disembark : Command
    {
        public override void Execute()
        {
            if (GameState.MapExtender.PlayerDisembark(GameState))
                return;

            TextArea.PrintLine("\nNothing to disembark.", XleColor.Yellow);
        }
    }
}
