using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	public class TheWealthy : LobExhibit
	{
		public TheWealthy()
			: base("The Wealthy", Coin.RedGarnet)
		{ }

		public Random Random { get; set; }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.TheWealthy; }
		}

		public override void RunExhibit()
		{
			base.RunExhibit();

			TextArea.PrintLine();
			TextArea.PrintLine("Do you want some gold?");
			TextArea.PrintLine();

			if (0 == QuickMenu.QuickMenuYesNo())
			{
				int amount = (int)(400 + Math.Pow(Player.Level, 1.35) + Random.Next(100));

				TextArea.PrintLine();
				TextArea.PrintLine();
				TextArea.PrintLine("Gold + " + amount);
				TextArea.PrintLine();
				TextArea.PrintLine();

				Player.Gold += amount;

				SoundMan.PlaySoundSync(LotaSound.VeryGood);
			}
		}
	}
}
