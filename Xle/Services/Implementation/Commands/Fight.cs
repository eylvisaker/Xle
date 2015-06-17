namespace ERY.Xle.Services.Implementation.Commands
{
	public class Fight : Command
	{
		public override void Execute(GameState state)
		{
			state.MapExtender.PlayerFight(state);
		}
	}
}
