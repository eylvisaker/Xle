using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class SingingCrystal : LobExhibit
	{
		public SingingCrystal()
			: base("Singing Crystal", Coin.BlueGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.VaseOfSouls; }
		}
	}
}
