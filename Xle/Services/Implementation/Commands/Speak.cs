namespace ERY.Xle.Services.Implementation.Commands
{
	public class Speak : Command
	{
		public override void Execute(GameState state)
		{
			if (state.MapExtender.PlayerSpeak(state) == false)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No response.");
			}
		}
	}
}
