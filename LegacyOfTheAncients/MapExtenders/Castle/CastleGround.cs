using ERY.Xle.XleEventTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleGround : NullCastleExtender
	{
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (y >= TheMap.Height)
				return 16;
			else
				return base.GetOutsideTile(playerPoint, x, y);
		}

		public override void PlayerUse(Player player, int item, ref bool handled)
		{

			switch (item)
			{
				case 4:			// Iron Key
				case 5:			// Copper Key
				case 6:			// Brass Key
				case 7:			// Stone Key
					handled = UseKey(player);
					break;

				case 8:				// magic seeds
					handled = UseMagicSeeds(player);
					break;

				case 12:				// magic ice
					handled = UseMagicIce(player);
					break;
			}
		}
		private bool UseMagicIce(Player player)
		{
			XleCore.Wait(250);

			var evt = TheMap.GetEvent<XleEventTypes.MagicIce>(player, 1);

			if (evt == null)
				return false;

			for (int j = evt.Rectangle.Top; j < evt.Rectangle.Bottom; j++)
			{
				for (int i = evt.Rectangle.Left; i < evt.Rectangle.Right; i++)
				{
					int m = TheMap[i, j];

					if (m % 16 >= 13 && m / 16 <= 2)
					{
						TheMap[i, j] = m - 8;
					}
				}
			}

			return true;
		}

		private bool UseMagicSeeds(Player player)
		{
			XleCore.Wait(150);

			g.invisible = true;
			g.AddBottom("You're invisible.");

			((IHasGuards)TheMap).IsAngry = false;

			XleCore.Wait(500);

			player.ItemCount(8, -1);

			return true;
		}

		private bool UseKey(Player player)
		{
			bool found = false;

			var door = TheMap.GetEvent<XleEventTypes.Door>(player, 1);

			if (door != null && door.RequiredItem == player.Hold)
			{
				found = true;

				g.AddBottom("Unlock door");
			}

			if (found == false)
			{
				g.AddBottom("");

				XleCore.Wait(300 + 200 * player.Gamespeed);
				g.UpdateBottom("This key does nothing here.");
			}

			return true;
		}

		public override void SpeakToGuard(GameState gameState, ref bool handled)
		{
			if (!g.invisible && !g.guard)
			{

			}
			else if (g.invisible)
			{
				g.AddBottom("");
				g.AddBottom("The guard looks startled.");
				handled = true;
			}
			else if (g.guard)  // for fortress
			{

			}
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is Door)
				return new CastleDoor();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}
