using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;



namespace ERY.Xle.XleMapTypes
{
	public class Castle : Town
	{
		public Castle() { }

		public override IEnumerable<string> AvailableTilesets
		{
			get
			{
				yield return "CastleTiles.png";
			}
		}

		public override void AfterExecuteCommand(Player player, KeyCode cmd)
		{
			/*
			if (setLastTime && tile % 16 >= 13 && tile / 16 < 2)
			{
				tx = Lota.random.Next(0x0D, 0x10);
				ty = Lota.random.Next(2);

				tile = ty * 0x10 + tx;

				if (!((tile & 0x0F) >= 0x0D && (tile & 0x10) >> 4 <= 0x01))
				{
					int qweruio = 1;
					tile = 0x0F;
				}
				this[i, j] = tile;
			}
			else if (setLastTime && (tile / 16 == 2 && tile % 16 < 8))
			{
				tile = cyclesDraw % 8 + 0x20;

				this[i, j] = tile;
			}
			else if (setLastTime && (tile >= 0x40 && tile < 0x43))
			{
				//tile = OriginalM(j, i);
				//tile -= cyclesDraw % 3;
				tile--;

				while (tile < 0x40)
					tile += 3;

				this[i, j] = tile;
			}
			 * */
		}

		public override bool PlayerOpen(Player player)
		{
			XleEvent evt = this.GetEvent(player, 1);

			if (evt == null)
				return false;

			return evt.Open(player);

		}

		public override bool PlayerTake(Player player)
		{
			XleEvent evt = this.GetEvent(player, 1);

			if (evt == null)
				return false;

			return evt.Take(player);
		}

		protected override void SpeakToGuard(Player player)
		{
			g.AddBottom("");

			if (!g.invisible && !g.guard)
			{
				g.AddBottom("The guard ignores you.");
			}
			else if (g.invisible)
			{
				if (XleCore.random.Next(1000) < 800)
					g.AddBottom("The guard looks startled.");
				else
				{
					g.AddBottom("The guard looks startled,");
					g.AddBottom("and starts popping prozac pills.");
				}
			}
			else if (g.guard)  // for fortress
			{

			}
		}
		protected override void OpenRoof(Roof roof)
		{
		}
		protected override void CloseRoof(Roof roof)
		{
		}

		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();

			retval.Add("Armor");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Magic");
			retval.Add("Open");
			retval.Add("Pass");
			retval.Add("Speak");
			retval.Add("Take");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}
	}
}
