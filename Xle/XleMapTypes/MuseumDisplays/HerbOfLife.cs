using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class HerbOfLife : Exhibit
	{
		public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
		public override int ExhibitID { get { return 6; } }
		public override string LongName
		{
			get
			{
				return "The herb of life";
			}
		}

		public override void PlayerXamine(Player player)
		{
			ReadRawText(RawText);

			g.AddBottom();
			g.AddBottom("Do you want to eat the fruit?");
			g.AddBottom();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
			{
				SoundMan.PlaySound(LotaSound.Good);
				g.AddBottom("You feel a tingling sensation.", XleColor.Green);

				while (SoundMan.IsAnyPlaying())
				{
					XleCore.wait(10);
				}

				player.museum[ExhibitID] = 3;
			}
		}
	}
}
