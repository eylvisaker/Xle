using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class Mountains : LobExhibit
	{
		public Mountains()
			: base("Mountains", Coin.AmethystGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.VaseOfSouls; }
		}
	}
}
