using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Pegasus : LotaExhibit
	{
		public Pegasus() : base("Pegasus", Coin.Diamond) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Pegasus; } }
		public override string LongName
		{
			get { return "A flight of fancy"; }
		}
		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to climb on?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.ChangeMap(player, 3, 0, 0, 0);
			}
		}
		public override bool StaticBeforeCoin
		{
			get
			{
				return false;
			}
		}
	}
}
