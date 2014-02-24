using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.LoB.MapExtenders;
using ERY.Xle.LoB.MapExtenders.Castle;
using ERY.Xle.LoB.MapExtenders.Citadel;
using ERY.Xle.LoB.MapExtenders.Labyrinth;
using ERY.Xle.LoB.MapExtenders.Outside;
using ERY.Xle.LoB.TitleScreen;
using ERY.Xle.XleMapTypes;
using ERY.Xle.XleMapTypes.Extenders;
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

		public override IOutsideExtender CreateMapExtender(XleMapTypes.Outside outside)
		{
			return new LobBaseOutside();
		}
		public override IDungeonExtender CreateMapExtender(XleMapTypes.Dungeon theMap)
		{
			return base.CreateMapExtender(theMap);
		}
		public override ICastleExtender CreateMapExtender(XleMapTypes.Castle castle)
		{
			string ext = castle.ExtenderName.ToLowerInvariant();

			if (mExtenders.ContainsKey(ext))
				return (ICastleExtender)Activator.CreateInstance(mExtenders[ext]);

			return base.CreateMapExtender(castle);
		}
	}
}
