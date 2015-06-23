namespace ERY.Xle.Services.Implementation.Commands
{
    public class Xamine : Command
    {
        public override void Execute()
        {
            GameState.MapExtender.PlayerXamine(GameState);
        }
    }
}
