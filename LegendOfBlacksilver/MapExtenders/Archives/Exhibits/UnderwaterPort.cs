using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class UnderwaterPort : LobExhibit
	{
		public UnderwaterPort()
			: base("Underwater Port", Coin.YellowDiamond)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.VaseOfSouls; }
		}
	}
}
