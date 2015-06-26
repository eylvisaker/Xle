using ERY.Xle;
using ERY.Xle.Data;
using ERY.Xle.LotA;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

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
            player.StoryData = new LotaStory();
            var gameState = new GameState { Player = player };

            Information information = new Information();
            information.GameState = gameState;

            Assert.IsFalse(information.ShouldLevelUp());

            var museum = GameVariableExtensions.Story(player).Museum;

            // mark weaponry as closed, others as visited.
            museum[(int)ExhibitIdentifier.Thornberry] = 1;
            museum[(int)ExhibitIdentifier.Weaponry] = 10; 
            museum[(int)ExhibitIdentifier.Fountain] = 1;

            Assert.IsTrue(information.ShouldLevelUp());

            player.Level = 2;
            Assert.IsFalse(information.ShouldLevelUp());

            // mark topaz coin exhibits as visited
            museum[(int)ExhibitIdentifier.NativeCurrency] = 1;
            museum[(int)ExhibitIdentifier.HerbOfLife] = 1;
            museum[(int)ExhibitIdentifier.PirateTreasure] = 1;

            Assert.IsFalse(information.ShouldLevelUp());
            player.Story().BeenInDungeon = true;
            Assert.IsTrue(information.ShouldLevelUp());

            player.Level = 3;
            Assert.IsFalse(information.ShouldLevelUp());

            museum[(int)ExhibitIdentifier.StonesWisdom] = 1;
            museum[(int)ExhibitIdentifier.Tapestry] = 1;
            museum[(int)ExhibitIdentifier.LostDisplays] = 1;

            Assert.IsTrue(information.ShouldLevelUp());

            player.Level = 4;
            Assert.IsFalse(information.ShouldLevelUp());

            museum[(int)ExhibitIdentifier.KnightsTest] = 1;
            player.Items[LotaItem.MagicIce] = 1;

            Assert.IsTrue(information.ShouldLevelUp());

            player.Level = 5;
            Assert.IsFalse(information.ShouldLevelUp());

            player.Story().FoundGuardianLeader = true;
            Assert.IsTrue(information.ShouldLevelUp());

            player.Level = 6;

            for (int i = 0; i < 4; i++)
            {
                Assert.IsFalse(information.ShouldLevelUp());
                player.Items[LotaItem.GuardJewel]++; //  add a guard jewel.
            }
            Assert.IsTrue(information.ShouldLevelUp());

            player.Level = 7;
            Assert.IsFalse(information.ShouldLevelUp());

            player.Items[LotaItem.Compendium] = 1; // give the compendium
            Assert.IsTrue(information.ShouldLevelUp());
        }

        [TestMethod]
        public void InformationLevelupWithCheatTest()
        {
            Player player = new Player();
            player.StoryData = new LotaStory();
            var gameState = new GameState { Player = player };

            var data = new XleData();
            LotaFactory factory = new LotaFactory(data);

            Information information = new Information();
            information.GameState = gameState;

            Assert.IsFalse(information.ShouldLevelUp());

            for (int i = 2; i <= 7; i++)
            {
                factory.CheatLevel(player, i);
                Assert.IsFalse(information.ShouldLevelUp());

                player.Level--;
                Assert.IsTrue(information.ShouldLevelUp());
            }

            factory.CheatLevel(player, 10);
            player.Level = 7;
            Assert.IsTrue(information.ShouldLevelUp());
        }

    }
}
