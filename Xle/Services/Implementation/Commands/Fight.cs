namespace ERY.Xle.Services.Implementation.Commands
{
    public class Fight : Command
    {
        public override void Execute(GameState state)
        {
            GameState.MapExtender.PlayerFight(state);
        }
    }
}
