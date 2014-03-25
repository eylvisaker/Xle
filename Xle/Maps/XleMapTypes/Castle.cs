using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.Extenders;

namespace ERY.Xle.Maps.XleMapTypes
{
	public class Castle : Town
	{
		public Castle() { }

		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "CastleTiles.png";
			}
		}

		protected override MapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new CastleExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			base.Extender = Extender;

			return Extender;
		}
		
		public new CastleExtender Extender { get; set; }
	}
}
