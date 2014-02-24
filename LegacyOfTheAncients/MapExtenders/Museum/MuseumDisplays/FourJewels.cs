using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class FourJewels : Exhibit
	{
		public FourJewels() : base("Four Jewels", Coin.Ruby) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.FourJewels; } }

		public override AgateLib.Geometry.Color TextColor
		{
			get { return XleColor.Yellow; }
		}
		public override void PlayerXamine(Player player)
		{
			base.PlayerXamine(player);

			int map = player.Map;
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
