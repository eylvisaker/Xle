using Xle.Services;
using Xle.Services.Commands.Implementation;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Xle.Maps.Dungeons.Commands
{
    [ServiceName("DungeonFight")]
    public class DungeonFight : Fight
    {
        private DungeonExtender map
        {
            get { return (DungeonExtender)GameState.MapExtender; }
        }

        private DungeonMonster MonsterInFrontOfPlayer(Player player, ref int distance)
        {
            return map.MonsterInFrontOfPlayer(player, ref distance);
        }

        private DungeonCombat Combat { get { return map.Combat; } }

        public override async Task Execute()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            int distance = 0;
            int maxDistance = 1;
            if (Player.CurrentWeapon.Info(Data).Ranged)
                maxDistance = 5;

            DungeonMonster monst = MonsterInFrontOfPlayer(Player, ref distance);

            if (monst == null)
            {
                await TextArea.PrintLine("Nothing to fight.");
                return;
            }
            else if (distance > maxDistance)
            {
                await TextArea.PrintLine("The " + monst.Name + " is out-of-range");
                await TextArea.PrintLine("of your " + Player.CurrentWeapon.BaseName(Data) + ".");
                return;
            }

            bool hit = RollToHitMonster(monst);

            await TextArea.Print("Hit ");
            await TextArea.Print(monst.Name, XleColor.White);
            await TextArea.PrintLine(" with " + Player.CurrentWeapon.BaseName(Data));

            if (hit)
            {
                int damage = RollDamageToMonster(monst);

                SoundMan.PlaySound(LotaSound.PlayerHit);

                await HitMonster(monst, damage, XleColor.Cyan);
            }
            else
            {
                SoundMan.PlaySound(LotaSound.PlayerMiss);
                await TextArea.PrintLine("Your attack misses.");
                await GameControl.WaitAsync(500);
            }

            return;
        }

        private async Task HitMonster(DungeonMonster monst, int damage, Color clr)
        {
            await TextArea.Print("Enemy hit by blow of ", clr);
            await TextArea.Print(damage.ToString(), XleColor.White);
            await TextArea.PrintLine("!");

            monst.HP -= damage;
            await GameControl.WaitAsync(1000);

            if (monst.HP <= 0)
            {
                Combat.Monsters.Remove(monst);
                await TextArea.PrintLine(monst.Name + " dies!!");

                SoundMan.PlaySound(LotaSound.EnemyDie);

                await GameControl.WaitAsync(500);
            }
        }

        protected virtual bool RollToHitMonster(DungeonMonster monster)
        {
            return true;
        }

        protected virtual int RollDamageToMonster(DungeonMonster monster)
        {
            return 9999;
        }
    }
}
