using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
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
