using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class GameOfHonor : LobExhibit
	{
		public GameOfHonor()
			: base("Game Of Honor", Coin.RedGarnet)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.GameOfHonor; }
		}
	}
}
