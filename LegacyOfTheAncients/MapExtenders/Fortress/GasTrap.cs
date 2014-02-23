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
			if (state.Player.Y > TheEvent.Y + 2)
				return;

			handled = true;
			TheEvent.Enabled = false;

			MapData data = state.Map.ReadMapData(TheEvent.Rectangle);

			var ta = XleCore.TextArea;

			ta.Clear(true);
			ta.PrintLine();
			ta.PrintLine("Doors slam shut...");
			AddDoors(state);

			XleCore.Wait(1000);

			ta.PrintLine();
			ta.PrintLine("Gas fills the room...");
			AddGas(state);

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

			state.Map.WriteMapData(data, TheEvent.Rectangle.Location);
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

		private void AddGas(GameState state)
		{
			var gasGroup = state.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Special2);

			int startx = StartX(state);

			for (int j = TheEvent.Rectangle.Y + 1; j < TheEvent.Rectangle.Bottom - 1; j++)
			{
				for (int i = startx; i < startx + 3; i++)
				{
					state.Map[i, j] = gasGroup.RandomTile();
				}
			}
		}

		private void AddDoors(GameState state)
		{
			var doorGroup = state.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Door);

			int startx = StartX(state);

			for (int i = startx; i < startx + 3; i++)
			{
				int tile = doorGroup.Tiles[(i - startx) % doorGroup.Tiles.Count];

				state.Map[i, TheEvent.Y] = tile;
				state.Map[i, TheEvent.Rectangle.Bottom - 1] = tile;
			}
		}

		private int StartX(GameState state)
		{
			int x = TheEvent.X;

			if (state.Player.X > TheEvent.X + 4)
				x = TheEvent.X + 5;

			return x;
		}
	}
}
