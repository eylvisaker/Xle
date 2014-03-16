using ERY.Xle.LoB.MapExtenders.Archives.Exhibits;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
	class OwlArchive : LobArchiveExtenderBase 
	{
		Dictionary<int, Exhibit> mExhibits = new Dictionary<int, Exhibit>();

		public OwlArchive()
		{
			mExhibits.Add(0x5e, new MetalWork());
			mExhibits.Add(0x5f, new SingingCrystal());
			mExhibits.Add(0x5D, new IslandRetreat());

			mExhibits.Add(0x56, new GameOfHonor());
			mExhibits.Add(0x55, new StormingGear());
			mExhibits.Add(0x57, new TheWealthy());

			mExhibits.Add(0x58, new Mountains());
			mExhibits.Add(0x59, new MagicEtherium());
			mExhibits.Add(0x50, new VaseOfSouls());
		}


		public override Exhibit GetExhibitByTile(int tile)
		{
			if (mExhibits.ContainsKey(tile) == false)
				return null;

			return mExhibits[tile];
		}
		public override void NeedsCoinMessage(Player player, Exhibit ex)
		{
			XleCore.TextArea.PrintLine(ex.UseCoinMessage);
		}
	}
}
