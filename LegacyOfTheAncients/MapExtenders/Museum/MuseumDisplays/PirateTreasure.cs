using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class PirateTreasure : LotaExhibit
	{
		public PirateTreasure() : base("Pirate Treasure", Coin.Topaz) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.PirateTreasure; } }

		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}
		public override void RunExhibit(Player player)
		{
			if (CheckOfferReread(player))
			{
				ReadRawText(ExhibitInfo.Text[1]);
			}

			XleCore.TextArea.PrintLine("Would you like to go");
			XleCore.TextArea.PrintLine("to the pirate's lair?");
			XleCore.TextArea.PrintLine();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
			{
				ReadRawText(ExhibitInfo.Text[2]);

				ColorStringBuilder b = new ColorStringBuilder();

				b.AddText("A priceless sapphire!", XleColor.White);

				for (int i = 0; i < 8; i++)
				{
					XleCore.Wait(50);

					b.SetColor(12 + i, XleColor.Cyan);
					g.UpdateBottom(b, 2);
				}

				XleCore.WaitForKey();

				player.SetMap(2, 100, 34);
			}
		}
	}
}
