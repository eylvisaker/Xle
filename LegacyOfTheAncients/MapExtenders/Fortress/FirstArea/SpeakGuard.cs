using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
	class SpeakGuard : EventExtender
	{
		public override bool Speak(GameState state)
		{
			Guard guard = FindGuard(state);
			if (guard == null)
				return false;
			if (guard.OnPlayerAttack == null)
				guard.OnPlayerAttack += KillGuard;

			MoveGuardToBars(state, guard);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Shut yer trap or I'll");
			XleCore.TextArea.PrintLine("reach through and bop you.");

			return true;
		}

		private void MoveGuardToBars(GameState state, Guard guard)
		{
			if (guard.Location.X <= TheEvent.X + 1)
				return;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The guard walks over.");

			for (int i = 0; i < 5; i++)
			{
				MoveGuard(guard, -1, 0);
			}

			MoveGuard(guard, 0, -1);

			for (int i = 0; i < 5; i++)
			{
				MoveGuard(guard, -1, 0);
			}

			MoveGuard(guard, 0, -1);
			MoveGuard(guard, 0, -1);

		}

		private void MoveGuard(Guard guard, int dx, int dy)
		{
			guard.X += dx;
			guard.Y += dy;
			guard.Facing = new Point(dx, dy).ToDirection();
			
			XleCore.Wait(150);
		}

		private bool KillGuard(GameState state, Guard guard)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You surprise the guard and kill him.");
			SoundMan.PlaySound(LotaSound.EnemyDie);

			state.Map.Guards.Remove(guard);
			state.Map.Guards.IsAngry = true;

			XleCore.Wait(1500);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You find a rod on the body...", XleColor.Cyan);
			XleCore.Wait(1500);

			state.Map.RemoveJailBars(TheEvent.Rectangle, 21);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("It unlocks the door.", XleColor.Yellow);
			SoundMan.PlaySoundSync(LotaSound.VeryGood);
			
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You find a broadaxe.", XleColor.White);
			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			state.Player.AddWeapon(7, 3);

			TheEvent.Enabled = false;

			return true;
		}

		private Guard FindGuard(GameState state)
		{
			foreach(var guard in state.Map.Guards)
			{
				if (TheEvent.Rectangle.Contains(guard.Location))
					return guard;
			}
			return null;
		}

	}
}
