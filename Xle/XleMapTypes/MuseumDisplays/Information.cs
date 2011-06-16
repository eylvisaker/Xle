using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Information : Exhibit
	{
		public Information() : base("Information", Coin.None) { }
		public override int ExhibitID { get { return 0; } }
		public override string CoinString
		{
			get { return string.Empty; }
		}

		public override void PlayerXamine(Player player)
		{
			bool[] bits = GetBitStatus(player.museum[0]);
			bool doneAnything = false;

			if (bits[0] == false)
			{
				// introductory text.  it does not follow through.
				ReadRawText(ExhibitInfo.Text[2]);
				return;
			}
			
			if (bits[1] == false && player.Item(15) == 0)
			{
				// lost compendium
				ReadRawText(ExhibitInfo.Text[3]);

				SetBit(player, 1);
				doneAnything = true;
			}
			doneAnything |= CheckSceptorCrown(player);

			doneAnything |= CheckLevelUp(player);

			doneAnything |= OfferIronKey(player);

			if (doneAnything == false) 
			{
				// You must do more before I can help you again.
				ReadRawText(ExhibitInfo.Text[1]);
			}
		}

		private bool CheckSceptorCrown(Player player)
		{
			if (player.Item(13) > 0 && player.Item(16) > 0)
			{
				// found scepter and crown
				ReadRawText(ExhibitInfo.Text[10]);

				// give magic ice.
				player.ItemCount(12, 1);
				return true;
			}
			else if ((player.museum[0] & 0x2) > 0)
			{
				if (player.Item(13) > 0)
				{
					// found scepter, give hint about crown
					ReadRawText(ExhibitInfo.Text[7]);

					this.SetBit(player, 2);
					return true;
				}
				else if (player.Item(16) > 0)
				{
					// found crown, give hint about sceptor
					ReadRawText(ExhibitInfo.Text[8]);


					this.SetBit(player, 2);
					return true;
				}
			}

			return false;
		}

		private bool OfferIronKey(Player player)
		{
			if (player.Level >= 3 && player.Item(4) == 0)
			{
				ReadRawText(ExhibitInfo.Text[5]);

				g.AddBottom();
				g.AddBottom("Do you accept the");
				g.AddBottom("Caretaker's offer?");

				if (XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
				{
					ReadRawText(ExhibitInfo.Text[6]);

					player.ItemCount(4, 1);
				}

				return true;
			}
			else
				return false;
		}

		private bool CheckLevelUp(Player player)
		{
			if (ShouldLevelUp(player))
			{
				ReadRawText(ExhibitInfo.Text[0]);
				int newLevel = TargetLevel(player);

				g.UpdateBottom(" Your level is now " + newLevel.ToString() + "!!", 1);

				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				player.Level = newLevel;

				return true;
			}
			else
				return false;
		}

		public bool ShouldLevelUp(Player player)
		{
			if (TargetLevel(player) > player.Level)
				return true;
			else
				return false;
		}

		private int TargetLevel(Player player)
		{
			// check if we have compendium and guard jewels
			if (player.Item(15) > 0 &&
				player.Item(14) >= 4)
			{
				return 10;
			}

			// check if we've gotten the four jewels
			if (player.Item(14) >= 4)
				return 7;

			// check if we've found the leader of the guardians
			if (player.guardian == 3)
				return 6;

			int jadeExhibits = CountExhibits(player, 2, 4);
			int topazExhibits = CountExhibits(player, 5, 7);
			int amethystExhibits = CountExhibits(player, 8, 9);
			int sapphireExhibits = CountExhibits(player, 10, 11);

			// check if we've viewed both sapphire visits 
			// (thus having completed the first two dungeons)
			// and check if we've returned the crown and sceptor.
			if (sapphireExhibits == 2 &&
				player.Item(12) > 0)
			{
				return 5;
			}

			// check if we've viewed both amethyst exhibits
			if (amethystExhibits == 2)
				return 4;

			// check if we've viewed all jade and topaz exhibits and been 
			// to the pirate's lair
			if (player.beenInDungeon &&
				jadeExhibits == 3 &&
				topazExhibits == 3)
			{
				return 3;
			}

			// check that we've seen at least four exhibits
			if (jadeExhibits + topazExhibits > 4)
				return 2;

			// geez, they've done nothing.
			return 1;
		}

		private static int CountExhibits(Player player, int start, int finish)
		{
			int ex = 0;

			for (int i = start; i <= finish; i++)
				ex += player.museum[i] != 0 ? 1 : 0;

			return ex;
		}

		private bool DoLevelUp(Player player)
		{
			return false;
		}

		private void SetBit(Player player, int p)
		{
			int val = 1 << p;

			player.museum[0] |= val;
		}

		private bool[] GetBitStatus(int value)
		{
			bool[] retval = new bool[32];

			for (int i = 0; i < 32; i++)
			{
				int test = 1 << i;

				retval[i] = (value & test) != 0;
			}

			return retval;
		}
	}
}
