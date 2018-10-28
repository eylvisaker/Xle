using AgateLib;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    [Transient("SpeakGuard")]
    public class SpeakGuard : EventExtender
    {
        public override async Task<bool> Speak()
        {
            Guard guard = FindGuard();

            if (guard == null)
                return false;
            if (guard.OnPlayerAttack == null)
                guard.OnPlayerAttack += (state, guard1) => KillGuard(guard1);

            await MoveGuardToBars(guard);

            await TextArea.PrintLine();
            await TextArea.PrintLine("Shut yer trap or I'll");
            await TextArea.PrintLine("reach through and bop you.");

            return true;
        }

        private async Task MoveGuardToBars(Guard guard)
        {
            if (guard.Location.X <= TheEvent.X + 1)
                return;

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("The guard walks over.");

            for (int i = 0; i < 5; i++)
            {
                await MoveGuard(guard, -1, 0);
            }

            await MoveGuard(guard, 0, -1);

            for (int i = 0; i < 5; i++)
            {
                await MoveGuard(guard, -1, 0);
            }

            await MoveGuard(guard, 0, -1);
            await MoveGuard(guard, 0, -1);
        }

        private async Task MoveGuard(Guard guard, int dx, int dy)
        {
            guard.X += dx;
            guard.Y += dy;
            guard.Facing = new Point(dx, dy).ToDirection();

            await GameControl.WaitAsync(150);
        }

        private async Task<bool> KillGuard(Guard guard)
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("You surprise the guard and kill him.");
            SoundMan.PlaySound(LotaSound.EnemyDie);

            Map.Guards.Remove(guard);
            Map.Guards.IsAngry = true;

            await GameControl.WaitAsync(1500);

            await TextArea.PrintLine();
            await TextArea.PrintLine("You find a rod on the body...", XleColor.Cyan);
            await GameControl.WaitAsync(1500);

            Map.RemoveJailBars(TheEvent.Rectangle, 21);

            await TextArea.PrintLine();
            await TextArea.PrintLine("It unlocks the door.", XleColor.Yellow);
            await GameControl.PlaySoundWait(LotaSound.VeryGood);

            await TextArea.PrintLine();
            await TextArea.PrintLine("You find a broadaxe.", XleColor.White);
            await GameControl.PlaySoundWait(LotaSound.VeryGood);

            Player.AddWeapon(7, 3);
            if (Player.CurrentWeapon.ID == 0)
                Player.CurrentWeapon = Player.Weapons.Last();

            Enabled = false;

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
