namespace ERY.Xle.Services.Commands.Implementation
{
    public class Xamine : Command
    {
        public override void Execute()
        {
            GameState.MapExtender.PlayerXamine();
        }
    }
}
