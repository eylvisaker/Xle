namespace ERY.Xle.Services.Implementation.Commands
{
	public class Hold : Command
	{
		public override void Execute(GameState state)
		{
			Use.ChooseHeldItem(state);
		}
	}
}
