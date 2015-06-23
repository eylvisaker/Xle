using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;

using ERY.Xle.Data;
using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.LotA.MapExtenders.Dungeons;
using ERY.Xle.LotA.MapExtenders.Fortress;
using ERY.Xle.LotA.MapExtenders.Museum;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.LotA.MapExtenders.Outside;
using ERY.Xle.LotA.MapExtenders.Towns;
using ERY.Xle.LotA.TitleScreen;
using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA
{
    public class LotaFactory : XleGameFactory
    {
        private XleData data;

        public LotaFactory(
            XleData data)
        {
            this.data = data;
        }

        public override string GameTitle
        {
            get
            {
                return "Legacy of the Ancients";
            }
        }

        public override IXleSerializable CreateStoryData()
        {
            return new LotaStory();
        }
        public override void LoadSurfaces()
        {
            Font = FontSurface.BitmapMonospace("font.png", new Size(16, 16));
            Font.StringTransformer = StringTransformer.ToUpper;

            Character = new Surface("character.png");
            Monsters = new Surface("OverworldMonsters.png");

            Lota3DSurfaces.LoadSurfaces();

            foreach (var exinfo in data.ExhibitInfo.Values)
            {
                exinfo.LoadImage();
            }
        }

        public override Map3DSurfaces GetMap3DSurfaces(Map3D map)
        {
            if (map is Museum)
                return Lota3DSurfaces.Museum;

            if (map.MapID == 71 || map.MapID == 73)
                return Lota3DSurfaces.DungeonBlue;
            else
                return Lota3DSurfaces.DungeonBrown;
        }

        public override void CheatLevel(Player player, int level)
        {
            if (level < 0) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");
            if (level == 8) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");
            if (level == 9) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");
            if (level > 10) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");

            var story = player.Story();

            ClearStoryItems(player);
            ClearMuseumCoins(player);

            Array.Clear(story.Museum, 0, story.Museum.Length);

            player.Level = level;

            player.Items[LotaItem.JadeCoin] = 2;
            player.Items[LotaItem.GoldArmband] = 1;
            player.Items[LotaItem.Compendium] = 1;

            player.Attribute.Reset();


            if (level >= 2)
            {
                player.Items.ClearCoins();
                player.Items[LotaItem.Compendium] = 0;
                player.Attribute[Attributes.dexterity] = 32;
                player.Attribute[Attributes.endurance] = 32;

                story.Museum[(int)ExhibitIdentifier.Thornberry] = 1;
                story.Museum[(int)ExhibitIdentifier.Weaponry] = 10; // mark weaponry as closed.
                story.Museum[(int)ExhibitIdentifier.Fountain] = 1;

                player.AddWeapon(3, 3);
                player.AddArmor(2, 3);
            }
            if (level >= 3)
            {
                story.Museum[(int)ExhibitIdentifier.NativeCurrency] = 1;
                story.Museum[(int)ExhibitIdentifier.HerbOfLife] = 1;
                story.Museum[(int)ExhibitIdentifier.PirateTreasure] = 1;

                player.Items[LotaItem.HealingHerb] = 40;
                story.BeenInDungeon = true;
            }
            if (level >= 4)
            {
                story.Museum[(int)ExhibitIdentifier.LostDisplays] = 1;
                story.Museum[(int)ExhibitIdentifier.Tapestry] = 1;
                story.Museum[(int)ExhibitIdentifier.StonesWisdom] = 1;

                story.PirateComplete = true;

                player.Attribute[Attributes.intelligence] = 35;
                player.Attribute[Attributes.strength] = 25;

                player.Items[LotaItem.SapphireCoin] = 1;
                player.Items[LotaItem.StoneKey] = 1;
            }
            if (level >= 5)
            {
                story.Museum[(int)ExhibitIdentifier.KnightsTest] = 1;
                story.ArmakComplete = true;

                player.Items[LotaItem.SapphireCoin] = 0;
                player.Items[LotaItem.MagicIce] = 1;
                player.Items[LotaItem.IronKey] = 1;

                player.Attribute[Attributes.strength] = 40;
            }
            if (level >= 6)
            {
                story.FoundGuardianLeader = true;
                player.Items[LotaItem.RubyCoin] = 1;

                player.Items[LotaItem.CopperKey] = 1;
                player.Items[LotaItem.BrassKey] = 1;
            }
            if (level >= 7)
            {
                story.FourJewelsComplete = true;
                player.Attribute[Attributes.strength] = 50;

                player.Items[LotaItem.RubyCoin] = 0;
                player.Items[LotaItem.GuardJewel] = 4;
            }
            if (level == 10)
            {
                player.Items[LotaItem.Compendium] = 1;
            }

            player.HP = player.MaxHP;
        }

        private static void ClearMuseumCoins(Player player)
        {
            player.Items.ClearCoins();
        }

        private static void ClearStoryItems(Player player)
        {
            player.Items.ClearStoryItems();
        }

        public override void SetGameSpeed(GameState state, int speed)
        {
            base.SetGameSpeed(state, speed);

            if (speed != 1)
                state.GameSpeed.OutsideStepTime = 400;
        }

        public override int MailItemID
        {
            get { return (int)LotaItem.Mail; }
        }
        public override int HealingItemID
        {
            get { return (int)LotaItem.HealingHerb; }
        }
        public override int ClimbingGearItemID
        {
            get { return (int)LotaItem.ClimbingGear; }
        }
    }
}
