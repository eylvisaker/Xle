namespace ERY.Xle.Services.Implementation.Commands
{
	public class ArmorCommand : Command
	{
		public override string Name
		{
			get { return "Armor"; }
		}
		public override void Execute(GameState state)
		{
			XleCore.TextArea.PrintLine("-choose above", XleColor.Cyan);

			state.Player.CurrentArmor = XleCore.PickArmor(state.Player.CurrentArmor);
		}
	}
}
