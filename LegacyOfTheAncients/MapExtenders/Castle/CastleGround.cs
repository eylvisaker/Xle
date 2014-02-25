using ERY.Xle.XleEventTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleGround : NullCastleExtender
	{
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			string name = evt.ExtenderName.ToLowerInvariant();

			if (evt is Door)
				return new CastleDoor();
			if (name == "magicice")
				return new MagicIce();
			if (name == "seeds")
				return new SeedPlant();
			if (evt is TreasureChestEvent)
				return new Chest { CastleLevel = 1 };

			return base.CreateEventExtender(evt, defaultExtender);
		}

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (y >= TheMap.Height)
				return 16;
			else
				return base.GetOutsideTile(playerPoint, x, y);
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			switch (item)
			{
				case (int)LotaItem.MagicSeed:
					handled = UseMagicSeeds(state.Player);
					break;
			}
		}
		private bool UseMagicSeeds(Player player)
		{
			XleCore.Wait(150);

			player.Story().Invisible = true;
			XleCore.TextArea.PrintLine("You're invisible.");
			XleCore.PlayerColor = XleColor.DarkGray;

			((IHasGuards)TheMap).IsAngry = false;

			XleCore.Wait(500);

			player.Items[LotaItem.MagicSeed]--;

			return true;
		}

		public override void SpeakToGuard(GameState state, ref bool handled)
		{
			if (state.Story().Invisible)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("The guard looks startled.");

				handled = true;
			}
			else if (g.guard)  // for fortress
			{

			}
		}

		public override void SetAngry(bool value)
		{
			var state = XleCore.GameState;

			state.Story().Invisible = false;
			XleCore.PlayerColor = XleColor.White;
		}
	}
}
