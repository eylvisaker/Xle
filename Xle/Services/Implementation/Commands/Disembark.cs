namespace ERY.Xle.Services.Implementation.Commands
{
	public class Disembark : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerDisembark(state))
				return;

			XleCore.TextArea.PrintLine("\nNothing to disembark.", XleColor.Yellow);
		}
	}
}
