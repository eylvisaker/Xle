using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class MorningStar : LobExhibit
	{
		public MorningStar()
			: base("Morning Star", Coin.Emerald)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.VaseOfSouls; }
		}
	}
}
