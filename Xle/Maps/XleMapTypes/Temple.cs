using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes
{
	public class Temple : Town
	{
		ITempleExtender Extender { get; set; }

		protected override IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullTempleExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}
	}
}
