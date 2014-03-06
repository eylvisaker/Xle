using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class HerbOfLife : LotaExhibit
	{
		public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.HerbOfLife; } }
		public override string LongName
		{
			get
			{
				return "The herb of life";
			}
		}

		public override void RunExhibit(Player player)
		{
			ReadRawText(RawText);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to eat the fruit?");
			XleCore.TextArea.PrintLine();

			if (XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
			{
				SoundMan.PlaySound(LotaSound.Good);
				XleCore.TextArea.PrintLine("You feel a tingling sensation.", XleColor.Green);

				while (SoundMan.IsPlaying(LotaSound.Good))
				{
					XleCore.Wait(10);
				}

				player.Story().EatenJutonFruit = true;
			}
		}
	}
}
