namespace ERY.Xle.Services.Commands.Implementation
{
    public class Fight : Command
    {
        public override void Execute()
        {
            GameState.MapExtender.PlayerFight(GameState);
        }
    }
}
