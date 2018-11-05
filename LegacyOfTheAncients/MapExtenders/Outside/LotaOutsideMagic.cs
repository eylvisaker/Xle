using AgateLib;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Commands.Implementation;
using Xle.Game;
using Xle.XleEventTypes;

namespace Xle.Ancients.MapExtenders.Outside
{
    [Transient("LotaOutsideMagic")]
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

        protected override async Task CastSpell(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 6:
                    await CastSeekSpell(magic);
                    break;

                default:
                    await base.CastSpell(magic);
                    break;
            }
        }

        private async Task CastSeekSpell(MagicSpell magic)
        {
            await TextArea.PrintLine("\n\nCast Seek Spell.");
            await GameControl.WaitAsync(200);

            const int MainContinentMapId = 1;
            const int MuseumMapId = 5;

            if (GameState.Map.MapID != MainContinentMapId)
            {
                await TextArea.PrintLine("Too far.");

                Player.Items[magic.ItemID]++;
                return;
            }

            GameState.Player.FaceDirection = Direction.East;

            await GameControl.PlaySoundSync(LotaSound.VeryGood);

            var evt = GameState.Map.Events.OfType<ChangeMapEvent>()
                .First(x => x.MapID == MuseumMapId);

            GameState.Player.Location = new Point(
                evt.Location.X + 2, evt.Location.Y - 2);
        }
    }
}
