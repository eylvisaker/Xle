namespace ERY.Xle.Services.Implementation.Commands
{
	public class Xamine : Command
	{
		public override void Execute(GameState state)
		{
			state.MapExtender.PlayerXamine(state);
		}
	}
}
