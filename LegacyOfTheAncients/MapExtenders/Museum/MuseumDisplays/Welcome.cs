using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
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
		public override string InsertCoinText
		{
			get { return "Tarmalon Museum!"; }
		}

		public void PlayGoldArmbandMessage()
		{
			ReadRawText(ExhibitInfo.Text[2]);
		}
	}

}
