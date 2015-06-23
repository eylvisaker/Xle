namespace ERY.Xle.Services.Implementation.Commands
{
    public class Fight : Command
    {
        public override void Execute()
        {
            GameState.MapExtender.PlayerFight(GameState);
        }
    }
}
