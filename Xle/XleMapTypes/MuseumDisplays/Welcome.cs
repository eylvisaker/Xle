﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.MuseumDisplays
{
	class Welcome : Exhibit
	{
		public Welcome() : base("Welcome", Coin.None) { }
		public override int ExhibitID { get { return 1; } }
		public override string LongName
		{
			get { return "Welcome to the famed"; }
		}
		public override string CoinString
		{
			get { return "Tarmalon Museum!"; }
		}

		public void PlayGoldArmbandMessage(Player player)
		{
			ReadRawText(ExhibitInfo.Text[2]);
		}
	}

}