using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class KnightsTest : LotaExhibit
	{
		public KnightsTest() : base("The Test", Coin.Sapphire) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.KnightsTest; } }

		public override string LongName
		{
			get
			{
				return "A test for knights";
			}
		}
		public override void RunExhibit(Player player)
		{
			ReadRawText(RawText);

			int map = player.MapID;
			int x = player.X;
			int y = player.Y;
			Direction facing = player.FaceDirection;

			player.DungeonLevel = 7;
			player.SetMap(72, 0, 1);
			player.SetReturnLocation(map, x, y, facing);
		}

		public override bool StaticBeforeCoin
		{
			get { return false; }
		}
	}
}
