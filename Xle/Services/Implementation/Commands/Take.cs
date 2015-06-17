namespace ERY.Xle.Services.Implementation.Commands
{
	public class Take : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerTake(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing to take.");

				XleCore.Wait(500);
			}
		}
	}
}
