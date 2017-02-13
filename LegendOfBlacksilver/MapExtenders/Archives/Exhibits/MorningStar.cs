﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	public class MorningStar : LobExhibit
	{
		public MorningStar()
			: base("Morning Star", Coin.Emerald)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.MorningStar; }
		}

		public override bool IsClosed
		{
			get { return Story.ClosedMorningStar; }
		}

		public override void RunExhibit()
		{
			base.RunExhibit();

			TextArea.PrintLine();
			TextArea.PrintLine("Do you want to borrow this item?");
			TextArea.PrintLine();

			if (0 == QuickMenu.QuickMenuYesNo())
			{
				TextArea.PrintLine();
				TextArea.PrintLine();
				TextArea.PrintLine("Fight bravely.");

				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				Player.AddWeapon(9, 4);
				Story.ClosedMorningStar = true;
			}
			else
				ReturnGem();
		}
	}
}
