using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
	class TarmalonExtender : NullOutsideExtender
	{
		public override void OnLoad(GameState state)
		{
			state.Story().Invisible = false;

			XleCore.PlayerColor = XleColor.White;
		}

		public int Stormy
		{
			get { return TheMap.Stormy; }
			set { TheMap.Stormy = value; }
		}

	}
}
