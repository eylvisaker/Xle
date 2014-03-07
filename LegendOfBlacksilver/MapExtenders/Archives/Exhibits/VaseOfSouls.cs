using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class VaseOfSouls : LobExhibit
	{
		public VaseOfSouls()
			: base("Vase of Souls", Coin.AmethystGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.VaseOfSouls; }
		}

		public override bool IsClosed(Player player)
		{
			return Lob.Story.ClosedVaseOfSouls;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			ReturnGem(player);

			Lob.Story.ClosedVaseOfSouls = true;
		}
	}
}
