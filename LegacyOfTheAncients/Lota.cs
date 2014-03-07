using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	static class Lota
	{
		public static LotaStory Story { get { return XleCore.GameState. Story(); } }
	}
}
