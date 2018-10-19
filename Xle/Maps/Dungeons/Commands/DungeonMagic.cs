using Xle.Data;
using Xle.Services.Commands.Implementation;
using Microsoft.Xna.Framework;
using System;

namespace Xle.Maps.Dungeons.Commands
{
    public class DungeonMagic : MagicWithFancyPrompt
    {
        private DungeonExtender MapExtender { get { return (DungeonExtender)GameState.MapExtender; } }

        private DungeonCombat Combat { get { return MapExtender.Combat; } }

        protected override void CastSpell(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 1:
                case 2:
                    UseAttackMagic(magic);
                    break;

                default:
                    break;
            }
        }

        private void UseAttackMagic(MagicSpell magic)
        {
            int distance = 0;
            TextArea.PrintLine();
            TextArea.PrintLine("Shoot " + magic.Name + ".", XleColor.White);

            DungeonMonster monst = MonsterInFrontOfPlayer(Player, ref distance);
            var magicSound = magic.ID == 1 ? LotaSound.MagicFlame : LotaSound.MagicBolt;
            var hitSound = magic.ID == 1 ? LotaSound.MagicFlameHit : LotaSound.MagicBoltHit;

            if (monst == null)
            {
                SoundMan.PlayMagicSound(magicSound, hitSound, distance);
                TextArea.PrintLine("There is no effect.", XleColor.White);
            }
            else
            {
                if (RollSpellFizzle(magic))
                {
                    SoundMan.PlayMagicSound(magicSound, LotaSound.MagicFizzle, distance);
                    TextArea.PrintLine("Attack fizzles.", XleColor.White);
                    GameControl.Wait(500);
                }
                else
                {
                    SoundMan.PlayMagicSound(magicSound, hitSound, distance);
                    int damage = RollSpellDamage(magic, distance);

                    HitMonster(monst, damage, XleColor.White);
                }
            }
        }

        private void HitMonster(DungeonMonster monst, int damage, Color clr)
        {
            TextArea.Print("Enemy hit by blow of ", clr);
            TextArea.Print(damage.ToString(), XleColor.White);
            TextArea.PrintLine("!");

            monst.HP -= damage;
            GameControl.Wait(1000);

            if (monst.HP <= 0)
            {
                Combat.Monsters.Remove(monst);
                TextArea.PrintLine(monst.Name + " dies!!");

                SoundMan.PlaySound(LotaSound.EnemyDie);

                GameControl.Wait(500);
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

        private void ExecuteKillFlash()
        {
            SoundMan.PlaySoundSync(LotaSound.VeryBad);

            Combat.Monsters.RemoveAll(monst => monst.KillFlashImmune == false);
        }

    }
}
