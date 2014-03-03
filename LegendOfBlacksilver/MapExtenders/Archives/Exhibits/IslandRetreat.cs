using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class IslandRetreat : LobExhibit
	{
		public IslandRetreat() : base("Island Retreat", Coin.BlueGem) { }

		public override string LongName
		{
			get
			{
				return "An island retreat";
			}
		}
		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.IslandRetreat; }
		}

		public override bool RequiresCoin(Player player)
		{
			if (HasBeenVisited(player))
				return false;

			return base.RequiresCoin(player);
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine("Would you like to go");
			XleCore.TextArea.PrintLine("to the island caverns now?");
			XleCore.TextArea.PrintLine();

			if (XleCore.QuickMenuYesNo() == 0)
			{
				XleCore.ChangeMap(player, 1, 1, 0, 0);
			}
		}
	}
}
