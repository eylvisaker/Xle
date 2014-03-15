using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.LoB.MapExtenders;
using ERY.Xle.LoB.MapExtenders.Archives;
using ERY.Xle.LoB.MapExtenders.Castle;
using ERY.Xle.LoB.MapExtenders.Citadel;
using ERY.Xle.LoB.MapExtenders.Dungeon;
using ERY.Xle.LoB.MapExtenders.Labyrinth;
using ERY.Xle.LoB.MapExtenders.Outside;
using ERY.Xle.LoB.MapExtenders.Temples;
using ERY.Xle.LoB.MapExtenders.Towns;
using ERY.Xle.LoB.TitleScreen;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB
{
	public class LobFactory : XleGameFactory
	{
		Dictionary<string, Type> mExtenders = new Dictionary<string, Type>();

		public LobFactory()
		{
			FillExtenderDictionaries();
		}
		
		public override IXleSerializable CreateStoryData()
		{
			return new LobStory();
		}

		private void FillExtenderDictionaries()
		{
			mExtenders["castle"] = typeof(DurekCastle);
			mExtenders["citadel1"] = typeof(CitadelGround);
			mExtenders["citadel2"] = typeof(CitadelUpper);
			mExtenders["labyrinth1"] = typeof(LabyrinthBase);
			mExtenders["labyrinth2"] = typeof(LabyrinthUpper);
		}

		public override string GameTitle
		{
			get
			{
				return "Legend of Blacksilver";
			}
		}

		public override void LoadSurfaces()
		{
			Font = FontSurface.BitmapMonospace("font.png", new Size(16, 16));
			Font.StringTransformer = StringTransformer.ToUpper;

			Character = new Surface("character.png");

			Lob3DSurfaces.LoadSurfaces();

			foreach (var exinfo in XleCore.Data.ExhibitInfo.Values)
			{
				try
				{
					exinfo.LoadImage();
				}
				catch(System.IO.FileNotFoundException)
				{
					System.Diagnostics.Debug.Print("Image " + exinfo.ImageFile + " not found.");
				}
			}
		}

		public override IXleTitleScreen CreateTitleScreen()
		{
			return new LobTitleScreen();
		}

		public override Maps.Map3DSurfaces GetMap3DSurfaces(Map3D map3D)
		{
			if (map3D is Museum)
				return Lob3DSurfaces.Archives;
			else
				return Lob3DSurfaces.DungeonBlue;
		}
		public override IMuseumExtender CreateMapExtender(Museum museum)
		{
			if (museum.MapID == 53)
				return new OwlArchive();
			else if (museum.MapID == 54)
				return new HawkArchive();

			return base.CreateMapExtender(museum);
		}
		public override IOutsideExtender CreateMapExtender(Outside outside)
		{
			if (outside.MapID < 5)
				return new Thalen();
			else
				return new Maelbane();
		}
		public override IDungeonExtender CreateMapExtender(Dungeon theMap)
		{
			switch(theMap.MapID)
			{
				case 61: return new IslandCaverns();
				case 62: return new TaragasMines();
				case 63: return new MarthbaneTunnels();
				case 64: return new PitsOfBlackmire();
				case 65: return new DeathspireChasm();

				default: return base.CreateMapExtender(theMap);
			}
		}
		public override ICastleExtender CreateMapExtender(Castle castle)
		{
			string ext = castle.ExtenderName.ToLowerInvariant();

			if (mExtenders.ContainsKey(ext))
				return (ICastleExtender)Activator.CreateInstance(mExtenders[ext]);

			return base.CreateMapExtender(castle);
		}
		public override ITempleExtender CreateMapExtender(Temple town)
		{
			return new LobTempleExtender();
		}
		public override ITownExtender CreateMapExtender(Town town)
		{
			if (town.MapID <= 20)
				return new ThalenTownExtender();
			else
				return new MaelbaneTownExtender();
		}

		public override void SetGameSpeed(GameState state, int Gamespeed)
		{
			base.SetGameSpeed(state, Gamespeed);

			state.GameSpeed.CastleOpenChestSoundTime = 300;
		}

		public override int MailItemID
		{
			get { return (int)LobItem.Package; }
		}
		public override int HealingItemID
		{
			get { return (int)LobItem.LifeElixir; }
		}
		public override int ClimbingGearItemID
		{
			get { return (int)LobItem.ClimbingGear; }
		}
	}
}
