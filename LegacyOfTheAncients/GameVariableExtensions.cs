using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	static class GameVariableExtensions
	{
		public static LotaStory Story(this Player player)
		{
			return (LotaStory)player.StoryData;
		}
		public static LotaStory Story(this GameState state)
		{
			return state.Player.Story();
		}
	}
}
