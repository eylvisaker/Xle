using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface ITownExtender : IMapExtender
	{
		void SpeakToGuard(GameState state, ref bool handled);
	}
}
