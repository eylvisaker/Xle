using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.TitleScreen
{
	class LobTitleScreen : IXleTitleScreen
	{
		public Player Player
		{
			get;
			set;
		}

		public void Run()
		{
			Player = new Player();

			Player.SetMap(1, 40, 40);
		}

		Player IXleTitleScreen.Player
		{
			get { return Player; }
		}

		void IXleTitleScreen.Run()
		{
			Run();
		}
	}
}
