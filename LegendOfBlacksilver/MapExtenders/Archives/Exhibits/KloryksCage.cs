using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class KloryksCage : LobExhibit
	{
		public KloryksCage()
			: base("Kloryk's Cage", Coin.WhiteDiamond)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.KloryksCage; }
		}
	}
}
