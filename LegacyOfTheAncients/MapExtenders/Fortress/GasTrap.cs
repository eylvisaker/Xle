using AgateLib.DisplayLib;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
	class GasTrap : NullEventExtender
	{
		public override void StepOn(GameState state, ref bool handled)
		{
			handled = true;
			TheEvent.Enabled = false;

			var ta = XleCore.TextArea;

			ta.Clear(true);
			ta.PrintLine();
			ta.PrintLine("Doors slam shut...");

			AddDoorsAndGas(state);

			XleCore.Wait(1000);

			ta.PrintLine();
			ta.PrintLine("Gas fills the room...");

			XleCore.Wait(3500);

			ta.PrintLine();
			ta.PrintLine("You fall asleep.");
			ta.PrintLine();

			XleCore.Wait(3000);

			XleCore.Wait(4000, DrawBlankScreen);

			RemoveWeaponsAndArmor(state);

			state.Player.X = 25;
			state.Player.Y = 45;

			XleCore.Wait(3500);
		}

		private void RemoveWeaponsAndArmor(GameState state)
		{

		}

		private void DrawBlankScreen()
		{
			Display.BeginFrame();
			Display.Clear(XleColor.Gray);
			Display.EndFrame();
		}

		private void AddDoorsAndGas(GameState state)
		{
		}
	}
}
