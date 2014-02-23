using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class HerbOfLife : Exhibit
	{
		public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
		public override ExhibitIdentifier ExhibitID { get { return ExhibitIdentifier.HerbOfLife; } }
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

				while (SoundMan.IsPlaying(LotaSound.Good))
				{
					XleCore.Wait(10);
				}

				player.museum[(int)ExhibitID] = 3;
			}
		}
	}
}
