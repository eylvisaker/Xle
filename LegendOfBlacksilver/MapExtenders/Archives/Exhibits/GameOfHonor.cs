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

		public override bool IsClosed(Player player)
		{
			return Lob.Story.RegisteredForTrist;
		}
		public override void RunExhibit(Player player)
		{
			if (player.Level < 3)
			{
				XleCore.TextArea.PrintLine("You must be more advanced");
				XleCore.TextArea.PrintLine("to use this exhibit.");
				XleCore.TextArea.PrintLine();

				ReturnGem(player);
			}
			else
			{
				base.RunExhibit(player);

				Lob.Story.RegisteredForTrist = true;
			}
		}
	}
}
