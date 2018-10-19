using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class DarkWand : LobExhibit
	{
		public DarkWand()
			: base("Dark Wand", Coin.YellowDiamond)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.DarkWand; }
		}
	}
}
