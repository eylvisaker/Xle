namespace ERY.Xle.Services.Implementation.Commands
{
	public class WeaponCommand : Command
	{
		public override string Name
		{
			get { return "Weapon"; }
		}
		public override void Execute(GameState state)
		{
			XleCore.TextArea.PrintLine("-choose above", XleColor.Cyan);

			state.Player.CurrentWeapon = XleCore.PickWeapon(state.Player.CurrentWeapon);
		}
	}
}
