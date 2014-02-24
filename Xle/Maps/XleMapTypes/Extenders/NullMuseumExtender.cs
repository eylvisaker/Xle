using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullMuseumExtender : NullMapExtender, IMuseumExtender
	{
		public new Museum TheMap { get { return (Museum)base.TheMap;  } }

		public int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			throw new NotImplementedException();
		}

		public void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{

			fontColor = XleColor.White;

			boxColor = XleColor.Gray;
			innerColor = XleColor.Yellow;
			vertLine = 15 * 16;
		}
		
		
		Dictionary<int, MuseumDisplays.Exhibit> mExhibits = new Dictionary<int, MuseumDisplays.Exhibit>();

		public NullMuseumExtender()
		{
			mExhibits.Add(0x50, new MuseumDisplays.Information());
			mExhibits.Add(0x51, new MuseumDisplays.Welcome());
			mExhibits.Add(0x52, new MuseumDisplays.Weaponry());
			mExhibits.Add(0x53, new MuseumDisplays.Thornberry());
			mExhibits.Add(0x54, new MuseumDisplays.Fountain());
			mExhibits.Add(0x55, new MuseumDisplays.PirateTreasure());
			mExhibits.Add(0x56, new MuseumDisplays.HerbOfLife());
			mExhibits.Add(0x57, new MuseumDisplays.NativeCurrency());
			mExhibits.Add(0x58, new MuseumDisplays.StonesWisdom());
			mExhibits.Add(0x59, new MuseumDisplays.Tapestry());
			mExhibits.Add(0x5A, new MuseumDisplays.LostDisplays());
			mExhibits.Add(0x5B, new MuseumDisplays.KnightsTest());
			mExhibits.Add(0x5C, new MuseumDisplays.FourJewels());
			mExhibits.Add(0x5D, new MuseumDisplays.Guardian());
			mExhibits.Add(0x5E, new MuseumDisplays.Pegasus());
			mExhibits.Add(0x5F, new MuseumDisplays.AncientArtifact());
		}
		public virtual Exhibit GetExhibitByTile(int tile)
		{
			if (mExhibits.ContainsKey(tile) == false)
				return null;

			return mExhibits[tile];
		}
	}
}
