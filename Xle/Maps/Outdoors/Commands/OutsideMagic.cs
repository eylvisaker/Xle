using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Outdoors.Commands
{
    public class OutsideMagic : MagicCommand
    {
        public IOutsideEncounters OutsideEncounters { get; set; }

        private EncounterState EncounterState { get { return OutsideEncounters.EncounterState; } }

        protected override async Task CastSpell(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 1:
                case 2:
                    if (EncounterState == 0)
                    {
                        Player.Items[magic.ItemID]++;
                        await TextArea.PrintLine("Nothing to fight.");
                        return;
                    }
                    else if (EncounterState != EncounterState.MonsterReady)
                    {
                        Player.Items[magic.ItemID]++;
                        await TextArea.PrintLine("The unknown creature is out of range.");
                        return;
                    }

                    await TextArea.PrintLine("Attack with " + magic.Name + ".");

                    var sound = (magic.ID == 1) ?
                        LotaSound.MagicFlame : LotaSound.MagicBolt;

                    if (RollSpellFizzle(magic))
                    {
                        await GameControl.PlayMagicSound(sound, LotaSound.MagicFizzle, 1);

                        await TextArea.PrintLine("Attack fizzles.", XleColor.Yellow);
                        return;
                    }
                    else
                        await GameControl.PlayMagicSound(sound, LotaSound.MagicFlameHit, 1);

                    int damage = RollSpellDamage(magic, 0);

                    await HitMonster(damage);

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private Task HitMonster(int damage) => OutsideEncounters.HitMonster(damage);

        protected virtual int RollSpellDamage(MagicSpell magic, int v)
        {
            throw new NotImplementedException();
        }

        protected virtual bool RollSpellFizzle(MagicSpell magic)
        {
            throw new NotImplementedException();
        }
    }
}
