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

			// TODO: do this map
			//player.SetMap(72, xx, yy);
		}
	}
}
