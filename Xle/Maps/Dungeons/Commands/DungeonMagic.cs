using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Commands.Implementation;

namespace Xle.Maps.Dungeons.Commands
{
    [Transient("DungeonMagic")]
    public class DungeonMagic : MagicWithFancyPrompt
    {
        private DungeonExtender MapExtender { get { return (DungeonExtender)GameState.MapExtender; } }

        private DungeonCombat Combat { get { return MapExtender.Combat; } }

        protected override async Task CastSpell(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 1:
                case 2:
                    await UseAttackMagic(magic);
                    break;

                default:
                    break;
            }
        }

        private async Task UseAttackMagic(MagicSpell magic)
        {
            int distance = 0;
            await TextArea.PrintLine();
            await TextArea.PrintLine("Shoot " + magic.Name + ".", XleColor.White);

            DungeonMonster monst = MonsterInFrontOfPlayer(Player, ref distance);
            var magicSound = magic.ID == 1 ? LotaSound.MagicFlame : LotaSound.MagicBolt;
            var hitSound = magic.ID == 1 ? LotaSound.MagicFlameHit : LotaSound.MagicBoltHit;

            if (monst == null)
            {
                await GameControl.PlayMagicSound(magicSound, hitSound, distance);
                await TextArea.PrintLine("There is no effect.", XleColor.White);
            }
            else
            {
                if (RollSpellFizzle(magic))
                {
                    await GameControl.PlayMagicSound(magicSound, LotaSound.MagicFizzle, distance);
                    await TextArea.PrintLine("Attack fizzles.", XleColor.White);
                    await GameControl.WaitAsync(500);
                }
                else
                {
                    await GameControl.PlayMagicSound(magicSound, hitSound, distance);
                    int damage = RollSpellDamage(magic, distance);

                    await HitMonster(monst, damage, XleColor.White);
                }
            }
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

        public virtual bool RollSpellFizzle(MagicSpell magic)
        {
            return Random.Next(10) < 5;
        }

        public virtual int RollSpellDamage(MagicSpell magic, int distance)
        {
            return (int)((magic.ID + 0.5) * 15 * (Random.NextDouble() + 1));
        }

        private DungeonMonster MonsterInFrontOfPlayer(Player player, ref int distance)
        {
            return MapExtender.MonsterInFrontOfPlayer(player, ref distance);
        }

        //private Task ExecuteKillFlash()
        //{
        //    GameControl.PlaySoundSync(LotaSound.VeryBad);

        //    Combat.Monsters.RemoveAll(monst => monst.KillFlashImmune == false);
        //}
    }
}
