namespace ERY.Xle.Services.Implementation.Commands
{
	public class Open : Command 
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerOpen(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing opens.");

				XleCore.Wait(500);
			}
		}
	}
}
