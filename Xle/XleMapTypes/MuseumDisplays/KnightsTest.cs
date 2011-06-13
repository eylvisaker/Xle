using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class KnightsTest : Exhibit
	{
		public KnightsTest() : base("The Test", Coin.Sapphire) { }
		public override int ExhibitID { get { return 11; } }

		public override string LongName
		{
			get
			{
				return "A test for knights";
			}
		}
		public override void PlayerXamine(Player player)
		{
			ReadRawText(RawText);

			player.DungeonLevel = 7;
			player.SetMap(72, 0, 1);
		}
	}
}
