using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB
{
	static class Lob
	{
		public static LobStory Story { get { return XleCore.GameState. Story(); } }
	}
}
