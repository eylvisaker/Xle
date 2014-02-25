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
		
		
		public NullMuseumExtender()
		{
		}
		public virtual Exhibit GetExhibitByTile(int tile)
		{
			throw new NotImplementedException();
			return null;
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
