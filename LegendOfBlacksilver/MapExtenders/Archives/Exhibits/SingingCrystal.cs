using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.Game;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    public class SingingCrystal : LobExhibit
    {
        public SingingCrystal()
            : base("Singing Crystal", Coin.BlueGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.SingingCrystal; }
        }

        public override bool IsClosed
        {
            get { return Story.ProcuredSingingCrystal; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            TextArea.Clear();

            Player.Items[LobItem.SingingCrystal] = 1;
            Story.ProcuredSingingCrystal = true;

            await GameControl.PlaySoundSync(LotaSound.VeryGood);
        }
    }
}
