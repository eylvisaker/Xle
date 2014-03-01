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

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.DarkGray;
			scheme.FrameHighlightColor = XleColor.Yellow;

			scheme.VerticalLinePosition = 15 * 16;
		}
		
		
		public NullMuseumExtender()
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


		public virtual bool PlayerHasCoin(Player player, Exhibit ex)
		{
			return true;
		}
		public virtual void NeedsCoinMessage(Player player, Exhibit ex)
		{
		}
		public virtual void UseCoin(Player player, Exhibit ex)
		{
		}
		public virtual void PrintUseCoinMessage(Player player, Exhibit ex)
		{
		}
	}
}
