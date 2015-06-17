namespace ERY.Xle.Services.Implementation.Commands
{
	public class Rob : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerRob(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing to rob.");
			}
		}
	}
}
