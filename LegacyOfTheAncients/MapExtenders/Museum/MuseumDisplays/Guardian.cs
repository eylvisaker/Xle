using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Guardian : LotaExhibit
	{
		public Guardian() : base("Guardian", Coin.Turquoise) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Guardian; } }

		public override bool IsClosed(Player player)
		{
			if (Lota.Story.HasGuardianPassword)
				return true;

			return false;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			Lota.Story.HasGuardianPassword = true;
		}
	}
}
