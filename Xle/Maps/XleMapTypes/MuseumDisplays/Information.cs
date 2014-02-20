﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Information : Exhibit
	{
		Player player;

		public Information() : base("Information", Coin.None) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.Information; } }
		public override string CoinString
		{
			get { return string.Empty; }
		}


		int ExhibitState
		{
			get { return player.museum[0]; }
			set { player.museum[0] = value; }
		}
		private void SetBit(int index, bool value)
		{
			int val = 1 << index;

			if (value)
				ExhibitState |= val;
			else
				ExhibitState = ~(~ExhibitState | val);
		}

		private bool GetBit(int index)
		{
			int val = 1 << index;

			return (ExhibitState & val) != 0;
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



		bool FirstUse
		{
			get { return GetBit(0); }
			set { SetBit(0, value); }
		}
		bool LostCompendiumText
		{
			get { return GetBit(1); }
			set { SetBit(1, value); }
		}
		bool SceptorCrownHint
		{
			get { return GetBit(2); }
			set { SetBit(2, value); }
		}
		public override void PlayerXamine(Player player)
		{
			this.player = player;

			bool[] bits = GetBitStatus(player.museum[0]);
			bool doneAnything = false;

			if (FirstUse == false)
			{
				// introductory text.  it does not follow through.
				ReadRawText(ExhibitInfo.Text[2]);
				FirstUse = true;
				return;
			}
			
			if (bits[1] == false && player.Item(15) == 0)
			{
				// lost compendium
				ReadRawText(ExhibitInfo.Text[3]);

				LostCompendiumText = true;
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
			// If the player has not accepted the caretakers offer and
			// received the iron key, skip any crown/sceptor checks.
			if (player.Item(4) == 0)
				return false;

			if (player.Item(13) > 0 && player.Item(16) > 0)
			{
				// found scepter and crown
				ReadRawText(ExhibitInfo.Text[10]);

				// give magic ice.
				player.ItemCount(12, 1);

				// remove sceptor and crown from inventory
				player.ItemCount(13, -100);
				player.ItemCount(16, -100);

				return true;
			}
			else if (SceptorCrownHint == false)
			{
				if (player.Item(13) > 0)
				{
					// found scepter, give hint about crown
					ReadRawText(ExhibitInfo.Text[7]);

					SceptorCrownHint = true;
					return true;
				}
				else if (player.Item(16) > 0)
				{
					// found crown, give hint about sceptor
					ReadRawText(ExhibitInfo.Text[8]);

					SceptorCrownHint = true;
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
			if (player.Variables["Guardian"] == 3)
				return 6;

			int jadeExhibits = CountExhibits(player, 2, 4);
			int topazExhibits = CountExhibits(player, 5, 7);
			int amethystExhibits = CountExhibits(player, 8, 9);
			int sapphireExhibits = CountExhibits(player, 10, 11);

			// check if we've viewed both sapphire visits 
			// (thus having completed the first two dungeons
			// TODO - this won't work if the player dies in Armak!)
			// and check if we've returned the crown and sceptor
			// and received the magic ice.
			if (sapphireExhibits == 2 &&
				player.Items[LotaItem.MagicIce] > 0)
			{
				return 5;
			}

			// check if we've viewed both amethyst exhibits and opened
			// the lost displays.
			if (amethystExhibits == 2 && sapphireExhibits == 1)
				return 4;

			// check if we've viewed all jade and topaz exhibits and been 
			// to the pirate's lair
			if (player.Variables.ContainsKey("BeenInDungeon") &&
				jadeExhibits == 3 &&
				topazExhibits == 3)
			{
				return 3;
			}

			// check that we've seen all the jade coin exhibits and we've closed
			// down the weaponry exhibit.
			if (jadeExhibits == 3 && player.museum[2] >= 10)
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
	}
}