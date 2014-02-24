using ERY.Xle.LoB.MapExtenders.Archives.Exhibits;
using ERY.Xle.XleMapTypes.Extenders;
using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
	class OwlArchive : NullMuseumExtender 
	{
		Dictionary<int, Exhibit> mExhibits = new Dictionary<int, Exhibit>();

		public OwlArchive()
		{

			mExhibits.Add(0x5D, new IslandCaverns());
		}


		public override Exhibit GetExhibitByTile(int tile)
		{
			if (mExhibits.ContainsKey(tile) == false)
				return null;

			return mExhibits[tile];
		}
	}
}
