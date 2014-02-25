using ERY.Xle;
using ERY.Xle.LotA;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.XleMapTypes.MuseumDisplays;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests
{
	[TestClass]
	public class MuseumExhibitTests
	{
		[TestMethod]
		public void InformationLevelupTest()
		{
			Player player = new Player();
			player.Items.ClearStoryItems(); // remove the compendium the player started with.
			
			Information information = new Information();

			Assert.IsFalse(information.ShouldLevelUp(player));

			var museum = GameVariableExtensions.Story(player).Museum;

			museum[(int)ExhibitIdentifier.Thornberry] = 1;
			museum[(int)ExhibitIdentifier.Weaponry] = 10; // mark weaponry as closed.
			museum[(int)ExhibitIdentifier.Fountain] = 1;

			Assert.IsTrue(information.ShouldLevelUp(player));

			player.Level = 2;
			Assert.IsFalse(information.ShouldLevelUp(player));

			museum[(int)ExhibitIdentifier.NativeCurrency] = 1;
			museum[(int)ExhibitIdentifier.HerbOfLife] = 1;
			museum[(int)ExhibitIdentifier.PirateTreasure] = 1;

			Assert.IsFalse(information.ShouldLevelUp(player));
			player.Story().BeenInDungeon = true;
			Assert.IsTrue(information.ShouldLevelUp(player));

			player.Level = 3;
			Assert.IsFalse(information.ShouldLevelUp(player));

			museum[(int)ExhibitIdentifier.StonesWisdom] = 1;
			museum[(int)ExhibitIdentifier.Tapestry] = 1;
			museum[(int)ExhibitIdentifier.LostDisplays] = 1;

			Assert.IsTrue(information.ShouldLevelUp(player));

			player.Level = 4;
			Assert.IsFalse(information.ShouldLevelUp(player));

			museum[(int)ExhibitIdentifier.KnightsTest] = 1;
			player.Items[LotaItem.MagicIce] = 1; 

			Assert.IsTrue(information.ShouldLevelUp(player));

			player.Level = 5;
			Assert.IsFalse(information.ShouldLevelUp(player));

			player.Story().FoundGuardianLeader = true;
			Assert.IsTrue(information.ShouldLevelUp(player));

			player.Level = 6;

			for (int i = 0; i < 4; i++)
			{
				Assert.IsFalse(information.ShouldLevelUp(player));
				player.Items[LotaItem.GuardJewel]++; //  add a guard jewel.
			}
			Assert.IsTrue(information.ShouldLevelUp(player));

			player.Level = 7;
			Assert.IsFalse(information.ShouldLevelUp(player));

			player.Items[LotaItem.Compendium] = 1; // give the compendium
			Assert.IsTrue(information.ShouldLevelUp(player));

		}
		[TestMethod]
		public void InformationLevelupWithCheatTest()
		{
			Player player = new Player();
			XleCore.GameState.Player = player;
			
			Information information = new Information();

			Assert.IsFalse(information.ShouldLevelUp(player));

			for (int i = 2; i <= 7; i++)
			{
				XleCore.CheatLevel(i);
				Assert.IsFalse(information.ShouldLevelUp(player));

				player.Level--;
				Assert.IsTrue(information.ShouldLevelUp(player));
			}

			XleCore.CheatLevel(10);
			player.Level = 7;
			Assert.IsTrue(information.ShouldLevelUp(player));

		}
	}
}
