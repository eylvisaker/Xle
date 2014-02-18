using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.LotA.TitleScreen;
using ERY.Xle.XleMapTypes;
using ERY.Xle.XleMapTypes.Extenders;
using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	public class LotaFactory : XleGameFactory
	{
		public override string GameTitle
		{
			get
			{
				return "Legacy of the Ancients";
			}
		}

		public override void LoadSurfaces()
		{
			Font = FontSurface.BitmapMonospace("font.png", new Size(16, 16));
			Font.StringTransformer = StringTransformer.ToUpper;

			Character = new Surface("character.png");
			Monsters = new Surface("OverworldMonsters.png");

			MuseumBackdrop = new Surface("Museum/MuseumBackdrop.png");
			MuseumWall = new Surface("Museum/MuseumWall.png");
			MuseumDoor = new Surface("Museum/MuseumDoor.png");
			MuseumSidePassage = new Surface("Museum/MuseumSidePassage.png");
			MuseumExhibitFrame = new Surface("Museum/ExhibitFrame.png");
			MuseumExhibitStatic = new Surface("Museum/ExhibitStatic.png");
			MuseumCloseup = new Surface("Museum/MuseumCloseup.png");
			MuseumExtras = new Surface("Museum/MuseumExtras.png");

			DungeonBlueBackdrop = new Surface("Dungeon/Blue/DungeonBackdrop.png");
			DungeonBlueSidePassage = new Surface("Dungeon/Blue/DungeonSidePassage.png");
			DungeonBlueWall = new Surface("Dungeon/Blue/DungeonWall.png");
			DungeonBlueExtras = new Surface("Dungeon/Blue/DungeonExtras.png");

			foreach (var exinfo in XleCore.ExhibitInfo.Values)
			{
				exinfo.LoadImage();
			}
		}

		public override IXleTitleScreen CreateTitleScreen()
		{
			return new LotaTitleScreen();
		}

		public override IDungeonExtender CreateMapExtender(Dungeon theMap) 
		{
			switch (theMap.MapID)
			{
				case 71: return new PirateExtender();
				case 72: return new ArmakExtender();
				case 73: return new FourJewelsExtender();
			}

			return base.CreateMapExtender(theMap);
		}

		public override void CheatLevel(Player player, int level)
		{
			if (level < 0) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");
			if (level == 8) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");
			if (level == 9) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");
			if (level > 10) throw new ArgumentOutOfRangeException("level", "Level must be 1-7 or 10.");

			player.Items.ClearStoryItems();
			player.Items.ClearCoins();

			Array.Clear(player.museum, 0, player.museum.Length);

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

				player.museum[(int)ExhibitIdentifier.Thornberry] = 1;
				player.museum[(int)ExhibitIdentifier.Weaponry] = 10; // mark weaponry as closed.
				player.museum[(int)ExhibitIdentifier.Fountain] = 1;
			}
			if (level >= 3)
			{
				player.museum[(int)ExhibitIdentifier.NativeCurrency] = 1;
				player.museum[(int)ExhibitIdentifier.HerbOfLife] = 1;
				player.museum[(int)ExhibitIdentifier.PirateTreasure] = 1;

				player.Variables["BeenInDungeon"] = 1;
			}
			if (level >= 4)
			{
				player.museum[(int)ExhibitIdentifier.LostDisplays] = 1;
				player.museum[(int)ExhibitIdentifier.Tapestry] = 1;
				player.museum[(int)ExhibitIdentifier.StonesWisdom] = 1;

				player.Variables["PirateComplete"] = 1;

				player.Attribute[Attributes.intelligence] = 35;
				player.Attribute[Attributes.strength] = 25;

				player.Items[LotaItem.SapphireCoin] = 1;
			}
			if (level >= 5)
			{
				player.museum[(int)ExhibitIdentifier.KnightsTest] = 1;
				player.Variables["ArmakComplete"] = 1;

				player.Items[LotaItem.SapphireCoin] = 0;
				player.Items[LotaItem.MagicIce] = 1;

				player.Attribute[Attributes.strength] = 40;
			}
			if (level >= 6)
			{
				player.Variables["Guardian"] = 3;
				player.Items[LotaItem.RubyCoin] = 1;
			}
			if (level >= 7)
			{
				player.Variables["FourJewelComplete"] = 1;
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
	}
}
