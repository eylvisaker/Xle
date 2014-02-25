using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LoB
{
	public class LobStory : IXleSerializable
	{
		public LobStory()
		{
			VisitedArchive = new int[17];
		}

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.WritePublicProperties(this);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			info.ReadPublicProperties(this);
		}

		public int[] VisitedArchive { get; set; }

		public bool ProcuredSingingCrystal { get; set; }
	}

	public static class LobStoryExtensions
	{
		public static LobStory Story(this GameState state)
		{
			return (LobStory)state.Player.StoryData;
		}

		public static LobStory Story(this Player player)
		{
			return (LobStory)player.StoryData;
		}
	}
}
