using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.XleMapTypes.Extenders;



namespace ERY.Xle.XleMapTypes
{
	public class Castle : Town
	{
		public Castle() { }

		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "CastleTiles.png";
			}
		}

		double lastAnim;
		int cycles;

		protected override void CreateExtender()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullCastleExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			mBaseExtender = Extender;
		}

		protected override void AnimateTiles(Rectangle rectangle)
		{
			base.AnimateTiles(rectangle);

			if (TileSet != null)
				return;

			for (int j = rectangle.Top; j < rectangle.Bottom; j++)
			{
				for (int i = rectangle.Left; i < rectangle.Right; i++)
				{
					int tile = this[i, j];

					if ((tile / 16 == 2 && tile % 16 < 8))
					{
						tile = cycles % 8 + 0x20;

						this[i, j] = tile;
					}
				}
			}

			if (lastAnim + 150 > Timing.TotalMilliseconds)
				return;

			lastAnim = Timing.TotalMilliseconds;
			cycles++;

			for (int j = rectangle.Top; j < rectangle.Bottom; j++)
			{
				for (int i = rectangle.Left; i < rectangle.Right; i++)
				{
					int tile = this[i, j];

					if (tile % 16 >= 13 && tile / 16 < 2)
					{
						int tx = XleCore.random.Next(0x0D, 0x10);
						int ty = XleCore.random.Next(2);

						tile = ty * 0x10 + tx;

						if (!((tile & 0x0F) >= 0x0D && (tile & 0x10) >> 4 <= 0x01))
						{
							tile = 0x0F;
						}
						this[i, j] = tile;
					}
					else if ((tile >= 0x40 && tile < 0x43))
					{
						//tile = OriginalM(j, i);
						//tile -= cyclesDraw % 3;
						tile--;

						while (tile < 0x40)
							tile += 3;

						this[i, j] = tile;
					}
				}
			}
		}

		public override bool PlayerUse(Player player, int item)
		{
			switch (item)
			{
				case 4:			// Iron Key
				case 5:			// Copper Key
				case 6:			// Brass Key
				case 7:			// Stone Key
					return UseKey(player);

				case 8:				// magic seeds
					return UseMagicSeeds(player);

				case 12:				// magic ice
					return UseMagicIce(player);
			}

			return false;
		}

		private bool UseMagicIce(Player player)
		{
			XleCore.wait(250);

			var evt = GetEvent<XleEventTypes.MagicIce>(player, 1);

			if (evt == null)
				return false;

			for (int j = evt.Rectangle.Top; j < evt.Rectangle.Bottom; j++)
			{
				for (int i = evt.Rectangle.Left; i < evt.Rectangle.Right; i++)
				{
					int m = this[i, j];

					if (m % 16 >= 13 && m / 16 <= 2)
					{
						this[i, j] = m - 8;
					}
				}
			}

			return true;
		}

		private bool UseMagicSeeds(Player player)
		{
			XleCore.wait(150);

			g.invisible = true;
			g.AddBottom("You're invisible.");

			IsAngry = false;

			XleCore.wait(500);

			player.ItemCount(8, -1);

			return true;
		}

		private bool UseKey(Player player)
		{
			bool found = false;

			var door = GetEvent<XleEventTypes.Door>(player, 1);

			if (door != null && door.RequiredItem == player.Hold)
			{
				found = true;
				SoundMan.PlaySound(LotaSound.UnlockDoor);
				XleCore.wait(250);

				g.AddBottom("Unlock door");

				for (int j = door.Rectangle.Y; j < door.Rectangle.Bottom; j++)
				{
					for (int i = door.Rectangle.X; i < door.Rectangle.Right; i++)
					{
						this[i, j] = 0;
					}
				}
			}

			if (found == false)
			{
				g.AddBottom("");

				XleCore.wait(300 + 200 * player.Gamespeed);
				g.UpdateBottom("This key does nothing here.");
			}

			return true;
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
				g.AddBottom("The guard looks startled.");
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

		public ICastleExtender Extender { get; set; }
	}
}
