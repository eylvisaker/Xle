using AgateLib;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class Guardian : LotaExhibit
    {
        public Guardian() : base("The guardian", Coin.Turquoise) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Guardian; } }

        public override bool IsClosed
        {
            get { return Story.HasGuardianPassword; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            Story.HasGuardianPassword = true;
        }
    }
}
