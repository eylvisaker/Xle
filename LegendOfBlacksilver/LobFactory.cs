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
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Data;

namespace ERY.Xle.LoB
{
	public class LobFactory : XleGameFactory
	{
		Dictionary<string, Type> mExtenders = new Dictionary<string, Type>();

		public LobFactory()
		{
			FillExtenderDictionaries();
		}

        public XleData Data { get; set; }

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
			Monsters = new Surface("OverworldMonsters.png");

			Lob3DSurfaces.LoadSurfaces();

			foreach (var exinfo in Data.ExhibitInfo.Values)
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

		public override Maps.Map3DSurfaces GetMap3DSurfaces(Map3D map3D)
		{
			if (map3D is Museum)
				return Lob3DSurfaces.Archives;
			else
				return Lob3DSurfaces.IslandCaverns;
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
