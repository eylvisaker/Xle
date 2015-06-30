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
using ERY.XleTests.LegacyOfTheAncients;

namespace ERY.XleTests
{
    [TestClass]
    public class MuseumExhibitTests : LotaTest
    {
        Player player = new Player();
        GameState gameState;
        Information information;

        [TestInitialize]
        public void Initialize()
        {
            player.Items.ClearStoryItems(); // remove the compendium the player started with.
            player.StoryData = new LotaStory();
            
            gameState = new GameState { Player = player };
        
            information = new Information();
            information.GameState = gameState;
        
            Assert.IsFalse(information.ShouldLevelUp());
        }

        void MarkExhibit(ExhibitIdentifier ex, int value)
        {
            var museum = player.Story().Museum;
            museum[(int)ex] = value;
        }

        [TestMethod]
        public void PromoteToLevel2()
        {
            // mark weaponry as closed, others as visited.
            CompleteJadeExhibits();

            Assert.IsTrue(information.ShouldLevelUp());
        }

        private void CompleteJadeExhibits()
        {
            MarkExhibit(ExhibitIdentifier.Thornberry, 1);
            MarkExhibit(ExhibitIdentifier.Weaponry, -1);
            MarkExhibit(ExhibitIdentifier.Fountain, 1);
        }

        [TestMethod]
        public void PromoteToLevel3()
        {
            player.Level = 2;
            Assert.IsFalse(information.ShouldLevelUp());

            CompleteJadeExhibits();

            // mark topaz coin exhibits as visited
            MarkExhibit(ExhibitIdentifier.NativeCurrency, 1);
            MarkExhibit(ExhibitIdentifier.HerbOfLife, 1);
            MarkExhibit(ExhibitIdentifier.PirateTreasure, 1);

            Assert.IsFalse(information.ShouldLevelUp());
            player.Story().BeenInDungeon = true;
            Assert.IsTrue(information.ShouldLevelUp());
        }

        [TestMethod]
        public void PromoteToLevel4()
        {
            player.Level = 3;
            Assert.IsFalse(information.ShouldLevelUp());

            MarkExhibit(ExhibitIdentifier.StonesWisdom, 1);
            MarkExhibit(ExhibitIdentifier.Tapestry, 1);
            MarkExhibit(ExhibitIdentifier.LostDisplays, 1);

            Assert.IsTrue(information.ShouldLevelUp());
        }

        [TestMethod]
        public void PromoteToLevel5()
        {
            player.Level = 4;
            Assert.IsFalse(information.ShouldLevelUp());

            MarkExhibit(ExhibitIdentifier.LostDisplays, 1);
            MarkExhibit(ExhibitIdentifier.KnightsTest, 1);
            player.Items[LotaItem.MagicIce] = 1;

            Assert.IsTrue(information.ShouldLevelUp());
        }

        [TestMethod]
        public void PromoteToLevel6()
        {
            player.Level = 5;
            Assert.IsFalse(information.ShouldLevelUp());

            player.Story().FoundGuardianLeader = true;
            Assert.IsTrue(information.ShouldLevelUp());
        }

        [TestMethod]
        public void PromoteToLevel7()
        {
            player.Level = 6;

            for (int i = 0; i < 4; i++)
            {
                Assert.IsFalse(information.ShouldLevelUp());
                player.Items[LotaItem.GuardJewel]++; //  add a guard jewel.
            }

            Assert.IsTrue(information.ShouldLevelUp());
        }

        [TestMethod]
        public void PromoteToLevel10()
        {
            player.Level = 7;
            Assert.IsFalse(information.ShouldLevelUp());

            player.Items[LotaItem.GuardJewel] = 4;
            player.Items[LotaItem.Compendium] = 1;

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
                Assert.IsTrue(information.ShouldLevelUp(), "Failed to meet condition for level {0}", i);
            }

            factory.CheatLevel(player, 10);
            player.Level = 7;
            Assert.IsTrue(information.ShouldLevelUp());
        }

    }
}
