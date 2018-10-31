using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Game;

namespace Xle.Blacksilver.TitleScreen
{
	public class LobTitleScreen : IXleTitleScreen
	{
		static bool firstRun = false;

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

				Player.MapID = 1;
				Player.X = 126;
				Player.Y = 52;

				Player.StoryData = new LobStory();

				Player.Items[LobItem.FalconFeather] = 1;
			}

			firstRun = true;
		}

		Player IXleTitleScreen.Player
		{
			get { return Player; }
		}
	}
}
