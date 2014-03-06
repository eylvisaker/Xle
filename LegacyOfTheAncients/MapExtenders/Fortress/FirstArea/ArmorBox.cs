using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
	class ArmorBox : TreasureChestExtender
	{
		public override void Open(GameState state, ref bool handled)
		{
			handled = true;

			XleCore.TextArea.PrintLine();
			if (TheEvent.Closed)
			{
				XleCore.TextArea.PrintLine("you see yellow guard");
				XleCore.TextArea.PrintLine("armor in the bottom.");

				PlayOpenChestSound();
				TheEvent.SetOpenTilesOnMap(state.Map);

				XleCore.Wait(state.GameSpeed.CastleOpenChestSoundTime);
			}
			else
			{
				XleCore.TextArea.PrintLine("box open already.");
			}
		}

		public override void Take(GameState state, ref bool handled)
		{
			handled = true;

			((IHasGuards)state.Map).IsAngry = false;

			XleCore.Renderer.PlayerColor = XleColor.Yellow;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("you put on armor.");

			XleCore.Wait(1000);
		}
	}
}
