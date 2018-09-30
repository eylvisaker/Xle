using ERY.Xle;
using ERY.Xle.Data;
using ERY.Xle.LotA;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.XleTests.LegacyOfTheAncients;
using FluentAssertions;

namespace ERY.XleTests
{
    public class MuseumExhibitTests : LotaTest
    {
        Information information;

        public MuseumExhibitTests()
        {
            Player.Items.ClearStoryItems(); // remove the compendium the player started with.
            Player.StoryData = new LotaStory();
            
            information = new Information();
            information.GameState = GameState;
        
            (information.ShouldLevelUp()).Should().BeFalse();
        }

        void MarkExhibit(ExhibitIdentifier ex, int value)
        {
            var museum = Player.Story().Museum;
            museum[(int)ex] = value;
        }

        [Fact]
        public void PromoteToLevel2()
        {
            // mark weaponry as closed, others as visited.
            CompleteJadeExhibits();

            (information.ShouldLevelUp()).Should().BeTrue();
        }

        private void CompleteJadeExhibits()
        {
            MarkExhibit(ExhibitIdentifier.Thornberry, 1);
            MarkExhibit(ExhibitIdentifier.Weaponry, -1);
            MarkExhibit(ExhibitIdentifier.Fountain, 1);
        }

        [Fact]
        public void PromoteToLevel3()
        {
            Player.Level = 2;
            (information.ShouldLevelUp()).Should().BeFalse();

            CompleteJadeExhibits();

            // mark topaz coin exhibits as visited
            MarkExhibit(ExhibitIdentifier.NativeCurrency, 1);
            MarkExhibit(ExhibitIdentifier.HerbOfLife, 1);
            MarkExhibit(ExhibitIdentifier.PirateTreasure, 1);

            (information.ShouldLevelUp()).Should().BeFalse();
            Player.Story().BeenInDungeon = true;
            (information.ShouldLevelUp()).Should().BeTrue();
        }

        [Fact]
        public void PromoteToLevel4()
        {
            Player.Level = 3;
            (information.ShouldLevelUp()).Should().BeFalse();

            MarkExhibit(ExhibitIdentifier.StonesWisdom, 1);
            MarkExhibit(ExhibitIdentifier.Tapestry, 1);
            MarkExhibit(ExhibitIdentifier.LostDisplays, 1);

            (information.ShouldLevelUp()).Should().BeTrue();
        }

        [Fact]
        public void PromoteToLevel5()
        {
            Player.Level = 4;
            (information.ShouldLevelUp()).Should().BeFalse();

            MarkExhibit(ExhibitIdentifier.LostDisplays, 1);
            MarkExhibit(ExhibitIdentifier.KnightsTest, 1);
            Player.Items[LotaItem.MagicIce] = 1;

            (information.ShouldLevelUp()).Should().BeTrue();
        }

        [Fact]
        public void PromoteToLevel6()
        {
            Player.Level = 5;
            (information.ShouldLevelUp()).Should().BeFalse();

            Player.Story().FoundGuardianLeader = true;
            (information.ShouldLevelUp()).Should().BeTrue();
        }

        [Fact]
        public void PromoteToLevel7()
        {
            Player.Level = 6;

            for (int i = 0; i < 4; i++)
            {
                (information.ShouldLevelUp()).Should().BeFalse();
                Player.Items[LotaItem.GuardJewel]++; //  add a guard jewel.
            }

            (information.ShouldLevelUp()).Should().BeTrue();
        }

        [Fact]
        public void PromoteToLevel10()
        {
            Player.Level = 7;
            (information.ShouldLevelUp()).Should().BeFalse();

            Player.Items[LotaItem.GuardJewel] = 4;
            Player.Items[LotaItem.Compendium] = 1;

            (information.ShouldLevelUp()).Should().BeTrue();
        }

        [Fact]
        public void InformationLevelupWithCheatTest()
        {
            Player player = new Player();
            player.StoryData = new LotaStory();
            var gameState = new GameState { Player = player };

            var data = new XleData();
            LotaFactory factory = new LotaFactory(data);

            Information information = new Information();
            information.GameState = gameState;

            (information.ShouldLevelUp()).Should().BeFalse();

            for (int i = 2; i <= 7; i++)
            {
                factory.CheatLevel(player, i);
                (information.ShouldLevelUp()).Should().BeFalse();

                player.Level--;
                information.ShouldLevelUp().Should().BeTrue($"Failed to meet condition for level {i}");
            }

            factory.CheatLevel(player, 10);
            player.Level = 7;
            (information.ShouldLevelUp()).Should().BeTrue();
        }

    }
}
