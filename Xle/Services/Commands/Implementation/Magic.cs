namespace ERY.Xle.Services.Commands.Implementation
{
    public class Magic : Command
    {
        public override void Execute()
        {
            GameState.MapExtender.PlayerMagic(GameState);
        }
    }
}
