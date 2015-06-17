namespace ERY.Xle.Services.Implementation.Commands
{
	public class Climb : Command 
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerClimb(state) == false)
			{
				XleCore.TextArea.PrintLine("\n\nNothing to climb");
			}
		}
	}
}
