using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
    public class SpeakGuard : EventExtender
    {
        public override bool Speak()
        {
            Guard guard = FindGuard();
            if (guard == null)
                return false;
            if (guard.OnPlayerAttack == null)
                guard.OnPlayerAttack += (state, guard1) => KillGuard(guard1);

            MoveGuardToBars(guard);

            TextArea.PrintLine();
            TextArea.PrintLine("Shut yer trap or I'll");
            TextArea.PrintLine("reach through and bop you.");

            return true;
        }

        private void MoveGuardToBars(Guard guard)
        {
            if (guard.Location.X <= TheEvent.X + 1)
                return;

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("The guard walks over.");

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

            GameControl.Wait(150);
        }

        private bool KillGuard(Guard guard)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You surprise the guard and kill him.");
            SoundMan.PlaySound(LotaSound.EnemyDie);

            Map.Guards.Remove(guard);
            Map.Guards.IsAngry = true;

            GameControl.Wait(1500);

            TextArea.PrintLine();
            TextArea.PrintLine("You find a rod on the body...", XleColor.Cyan);
            GameControl.Wait(1500);

            Map.RemoveJailBars(TheEvent.Rectangle, 21);

            TextArea.PrintLine();
            TextArea.PrintLine("It unlocks the door.", XleColor.Yellow);
            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            TextArea.PrintLine();
            TextArea.PrintLine("You find a broadaxe.", XleColor.White);
            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            Player.AddWeapon(7, 3);
            if (Player.CurrentWeapon.ID == 0)
                Player.CurrentWeapon = Player.Weapons.Last();

            TheEvent.Enabled = false;

            return true;
        }

        private Guard FindGuard()
        {
            foreach (var guard in Map.Guards)
            {
                if (TheEvent.Rectangle.Contains(guard.Location))
                    return guard;
            }
            return null;
        }

    }
}
