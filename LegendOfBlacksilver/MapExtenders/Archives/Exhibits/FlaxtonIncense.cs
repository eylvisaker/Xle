using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class FlaxtonIncense : LobExhibit
	{
		public FlaxtonIncense()
			: base("Flaxton Incense", Coin.WhiteDiamond)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.VaseOfSouls; }
		}
	}
}
