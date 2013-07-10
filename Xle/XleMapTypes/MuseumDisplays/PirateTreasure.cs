using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class PirateTreasure : Exhibit
	{
		public PirateTreasure() : base("Pirate Treasure", Coin.Topaz) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.PirateTreasure; } }

		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}
		public override void PlayerXamine(Player player)
		{
			if (CheckOfferReread(player))
			{
				ReadRawText(ExhibitInfo.Text[1]);
			}

			g.AddBottom("Would you like to go");
			g.AddBottom("to the pirate's lair?");
			g.AddBottom();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
			{
				ReadRawText(ExhibitInfo.Text[2]);

				ColorStringBuilder b = new ColorStringBuilder();

				b.AddText("A priceless sapphire!", XleColor.White);

				for (int i = 0; i < 8; i++)
				{
					XleCore.wait(50);

					b.SetColor(12 + i, XleColor.Cyan);
					g.UpdateBottom(b, 2);
				}

				XleCore.WaitForKey();

				player.SetMap(2, 100, 34);
			}
		}
	}
}
