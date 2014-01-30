using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.TitleScreen
{
	class LobTitleScreen : IXleTitleScreen
	{
		bool firstRun = false;

		public Player Player
		{
			get;
			set;
		}

		public void Run()
		{
			if (firstRun)
				Player = null;
			else
			{
				Player = new Player();

				Player.SetMap(1, 80, 80);

				Player.X = 80;
				Player.Y = 80;

			}

			firstRun = true;
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
