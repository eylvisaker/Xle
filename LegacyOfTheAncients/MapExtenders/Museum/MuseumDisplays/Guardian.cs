using AgateLib;

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

        public override void RunExhibit()
        {
            base.RunExhibit();

            Story.HasGuardianPassword = true;
        }
    }
}
