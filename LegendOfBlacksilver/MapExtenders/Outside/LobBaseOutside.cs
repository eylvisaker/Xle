using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Outside
{
	public class LobBaseOutside : NullOutsideExtender
	{
		public override int StepSize
		{
			get { return 2; }
		}
	}
}
