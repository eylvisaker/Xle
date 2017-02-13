using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.XleEventTypes;
using AgateLib.Mathematics.Geometry;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
    [ServiceName("LotaOutsideMagic")]
    public class LotaOutsideMagic : MagicCommand
    {
        protected override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                yield return Data.MagicSpells[1];
                yield return Data.MagicSpells[2];
                yield return Data.MagicSpells[6];
            }
        }

        protected override void CastSpell(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 6:
                    CastSeekSpell(magic);
                    break;

                default:
                    base.CastSpell(magic);
                    break;
            }
        }

        private void CastSeekSpell(MagicSpell magic)
        {
            TextArea.PrintLine("\n\nCast Seek Spell.");
            GameControl.Wait(200);

            const int MainContinentMapId = 1;
            const int MuseumMapId = 5;

            if (GameState.Map.MapID != MainContinentMapId)
            {
                TextArea.PrintLine("Too far.");

                Player.Items[magic.ItemID]++;
                return;
            }

            GameState.Player.FaceDirection = Direction.East;

            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            var evt = GameState.Map.Events.OfType<ChangeMapEvent>()
                .First(x => x.MapID == MuseumMapId);

            GameState.Player.Location = new Point(
                evt.Location.X + 2, evt.Location.Y - 2);
        }
    }
}
