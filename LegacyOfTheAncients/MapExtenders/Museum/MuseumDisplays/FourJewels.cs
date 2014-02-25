using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class FourJewels : LotaExhibit
	{
		public FourJewels() : base("Four Jewels", Coin.Ruby) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.FourJewels; } }

		public override AgateLib.Geometry.Color TitleColor
		{
			get { return XleColor.Yellow; }
		}
		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			int map = player.MapID;
			int x = player.X;
			int y = player.Y;
			Direction facing = player.FaceDirection;

			player.DungeonLevel = 0;
			player.SetMap(73, 0, 1);
			player.SetReturnLocation(map, x, y, facing);
		}

		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}
	}
}
