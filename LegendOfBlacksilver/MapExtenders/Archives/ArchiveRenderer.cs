using ERY.Xle.Maps.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
	class ArchiveRenderer : MuseumRenderer
	{
		protected override AgateLib.Geometry.Rectangle ExhibitCloseupRect
		{
			get
			{
				var retval = base.ExhibitCloseupRect;

				retval.Y -= 16;

				return retval;
			}
		}
	}
}
