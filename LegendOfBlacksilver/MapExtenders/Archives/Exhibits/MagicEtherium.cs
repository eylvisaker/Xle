using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class MagicEtherium : LobExhibit
	{
		public MagicEtherium()
			: base("Magic Etherium", Coin.AmethystGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.MagicEtherium; }
		}
	}
}
