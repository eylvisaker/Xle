using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class MuseumExtender : MapExtender
	{
		public new Museum TheMap { get { return (Museum)base.TheMap;  } }

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			throw new NotImplementedException();
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.DarkGray;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.MapAreaWidth = 23;
		}
		
		
		public MuseumExtender()
		{
		}
		public virtual Exhibit GetExhibitByTile(int tile)
		{
			throw new NotImplementedException();
		}

		public virtual void CheckExhibitStatus(GameState state)
		{
		}
		public virtual void InteractWithDisplay(Player player)
		{
			
		}

		public virtual void NeedsCoinMessage(Player player, Exhibit ex)
		{
		}
		public virtual void PrintUseCoinMessage(Player player, Exhibit ex)
		{
		}
	}
}
