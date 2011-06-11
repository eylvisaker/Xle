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

		public override bool PlayerUse(Player player, int item)
		{
			bool found = false;

			//switch (item)
			//{
			//    case 4:			// Iron Key
			//    case 5:			// Copper Key
			//    case 6:			// Brass Key
			//    case 7:			// Stone Key

					//for (j = -1; j < 3; j++)
					//{
					//    for (i = -1; i < 3; i++)
					//    {
					//        if (Lota.Map.CheckSpecial(player.X + i, player.Y + j) == 24 && found == false)
					//        {
					//            SpecialEvent dave = Lota.Map.GetSpecial(player.X + i, player.Y + j);

					//            if (dave.data[0] == player.Hold())
					//            {
					//                found = true;

					//                g.AddBottom(commandstring);
					//                SoundMan.PlaySound(LotaSound.UnlockDoor);
					//                wait(250);

					//                g.AddBottom("Unlock door");

					//                for (j = dave.sy; j < dave.sy + dave.sheight; j++)
					//                {
					//                    for (i = dave.sx; i < dave.sx + dave.swidth; i++)
					//                    {
					//                        int m = Lota.Map.M(j, i);

					//                        if ((m % 16 < 4 && m / 16 == 13) || (m % 16 >= 2 && m % 16 < 4 && m / 16 == 14))
					//                        {
					//                            Lota.Map.SetM(j, i, 0);
					//                        }

					//                    }
					//                }

					//                wait(750);

					//                break;
					//            }

					//        }
					//    }

					//    if (found) break;
					//}

			//        if (found)
			//        {
			//            noEffect = false;
			//        }

			//        if (noEffect)
			//        {
			//            g.AddBottom(commandstring);
			//            g.AddBottom("");

			//            Lota.wait(300 + 200 * player.Gamespeed);
			//            g.UpdateBottom("This key does nothing here.");

			//            noEffect = false;

			//        }
			//        break;
			//    case 8:				// magic seeds
			//        commandstring = "Eat Magic Seeds";

			//        g.AddBottom(commandstring);
			//        Lota.wait(150);

			//        g.invisible = true;
			//        g.AddBottom("You're invisible.");

			//        Lota.Map.IsAngry = false;

			//        Lota.wait(500);

			//        player.ItemCount(8, -1);
			//        noEffect = false;

			//        break;

			//    case 12:				// magic ice

			//        commandstring = "Throw magic ice";

			//        for (j = -1; j < 3; j++)
			//        {
			//            for (i = -1; i < 3; i++)
			//            {

			//                //if (Lota.Map.CheckSpecial(player.X + i, player.Y + j) == 22 && found == false)
			//                //{
			//                //    SpecialEvent dave = Lota.Map.GetSpecial(player.X + i, player.Y + j);
			//                //    found = true;

			//                //    g.AddBottom(commandstring);
			//                //    Lota.wait(500);

			//                //    for (j = dave.sy; j < dave.sy + dave.sheight; j++)
			//                //    {
			//                //        for (i = dave.sx; i < dave.sx + dave.swidth; i++)
			//                //        {
			//                //            int m = Lota.Map.M(j, i);

			//                //            if (m % 16 >= 13 && m / 16 <= 2)
			//                //            {
			//                //                Lota.Map.SetM(j, i, m - 8);
			//                //            }

			//                //        }
			//                //    }

			//                //    break;
			//                //}
			//            }

			//            if (found) break;
			//        }

			//        if (found)
			//        {
			//            return true;
			//        }

			//        break;
			//}

			return false;
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
