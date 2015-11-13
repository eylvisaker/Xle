using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Data;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Maps.Outdoors.Commands
{
    public class OutsideMagic : MagicCommand
    {
        public IOutsideEncounters OutsideEncounters { get; set; }

        OutsideExtender MapExtender {  get { return (OutsideExtender)GameState.MapExtender; } }
        EncounterState EncounterState {  get { return OutsideEncounters.EncounterState; } }

        protected override void CastSpell(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 1:
                case 2:
                    if (EncounterState == 0)
                    {
                        Player.Items[magic.ItemID]++;
                        TextArea.PrintLine("Nothing to fight.");
                        return;
                    }
                    else if (EncounterState != EncounterState.MonsterReady)
                    {
                        Player.Items[magic.ItemID]++;
                        TextArea.PrintLine("The unknown creature is out of range.");
                        return;
                    }

                    TextArea.PrintLine("Attack with " + magic.Name + ".");

                    var sound = (magic.ID == 1) ?
                        LotaSound.MagicFlame : LotaSound.MagicBolt;

                    if (RollSpellFizzle(magic))
                    {
                        SoundMan.PlayMagicSound(sound, LotaSound.MagicFizzle, 1);

                        TextArea.PrintLine("Attack fizzles.", XleColor.Yellow);
                        return;
                    }
                    else
                        SoundMan.PlayMagicSound(sound, LotaSound.MagicFlameHit, 1);

                    int damage = RollSpellDamage(magic, 0);

                    HitMonster(damage);

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void HitMonster(int damage)
        {
            OutsideEncounters.HitMonster(damage);
        }

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
