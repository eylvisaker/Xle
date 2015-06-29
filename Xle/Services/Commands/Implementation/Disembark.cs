namespace ERY.Xle.Services.Commands.Implementation
{
    public class Disembark : Command
    {
        public override void Execute()
        {
            if (GameState.MapExtender.PlayerDisembark())
                return;

            TextArea.PrintLine("\nNothing to disembark.", XleColor.Yellow);
        }
    }
}
