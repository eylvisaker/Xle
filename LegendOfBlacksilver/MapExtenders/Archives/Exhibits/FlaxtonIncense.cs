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
			get { return ExhibitIdentifier.FlaxtonIncense; }
		}

		public override bool IsClosed(Player player)
		{
			return Lob.Story.EatenFlaxton;
		}

		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Do you want to partake?");
			XleCore.TextArea.PrintLine();

			if (0 == XleCore.QuickMenuYesNo())
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("It's sour but doesn't taste that bad.");

				Lob.Story.EatenFlaxton = true;
			}
			else
				ReturnGem(player);
		}
	}
}
