using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	public class Welcome : LotaExhibit
	{
		public Welcome() : base("Welcome", Coin.None) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Welcome; } }
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
