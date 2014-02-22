using AgateLib.Geometry;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public class NullMapExtender : IMapExtender
	{
		public XleMap TheMap { get; set; }

		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return -1;
		}

		public virtual void OnLoad(GameState state)
		{ }



		public virtual void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			throw new NotImplementedException();
		}


		public virtual IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return (IEventExtender)Activator.CreateInstance(defaultExtender);
		}


		public virtual int StepSize
		{
			get { return 1; }
		}


		public virtual void PlayerUse(Player player, int item, ref bool handled)
		{
		}
	}
}
