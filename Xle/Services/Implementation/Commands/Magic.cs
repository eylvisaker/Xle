namespace ERY.Xle.Services.Implementation.Commands
{
    public class Magic : Command
    {
        public override void Execute(GameState state)
        {
            GameState.MapExtender.PlayerMagic(state);
        }
    }
}
