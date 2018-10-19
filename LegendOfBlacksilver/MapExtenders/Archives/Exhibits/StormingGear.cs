using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
	public class StormingGear : LobExhibit
	{
		public StormingGear()
			: base("Storming Gear", Coin.RedGarnet)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.StormingGear; }
		}

		public override bool IsClosed
		{
			get { return Player.Items[LobItem.RopeAndPulley] > 0; }
		}

		public override void RunExhibit()
		{
			base.RunExhibit();

			TextArea.PrintLine();
			TextArea.PrintLine("Do you want to borrow this gear?");
			TextArea.PrintLine();

			if (0 == QuickMenu.QuickMenuYesNo())
			{
				Player.Items[LobItem.RopeAndPulley] += 1;

				TextArea.PrintLine();
				TextArea.PrintLine("The equipment is now");
				TextArea.PrintLine("in your possession.");
			}
			else
			{
				ReturnGem();
			}
		}

	}
}
