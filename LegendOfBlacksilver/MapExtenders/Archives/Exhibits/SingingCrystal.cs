using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
	class SingingCrystal : LobExhibit
	{
		public SingingCrystal()
			: base("Singing Crystal", Coin.BlueGem)
		{ }

		public override ExhibitIdentifier ExhibitIdentifier
		{
			get { return ExhibitIdentifier.SingingCrystal; }
		}

		public override bool IsClosed(ERY.Xle.Player player)
		{
			if (player.Story().ProcuredSingingCrystal)
				return true;

			return base.IsClosed(player);
		}
		public override void RunExhibit(Player player)
		{
			base.RunExhibit(player);

			XleCore.TextArea.Clear();

			player.Items[LobItem.SingingCrystal] = 1;
			player.Story().ProcuredSingingCrystal = true;

			SoundMan.PlaySoundSync(LotaSound.VeryGood);
		}
	}
}
