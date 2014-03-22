using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes.Extenders;

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

		protected override IMapExtender CreateExtenderImpl()
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
		

		protected override void PlayOpenRoofSound(Roof roof)
		{
			// do nothing here
		}
		protected override void PlayCloseRoofSound(Roof roof)
		{
			// do nothing here
		}

		public new ICastleExtender Extender { get; set; }
	}
}
