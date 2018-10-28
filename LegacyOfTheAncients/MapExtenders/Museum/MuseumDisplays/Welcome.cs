using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class Welcome : LotaExhibit
    {
        public Welcome() : base("Welcome", Coin.None) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Welcome; } }
        public override string LongName
        {
            get { return "Welcome to the famed"; }
        }
        public override string InsertCoinText
        {
            get { return "Tarmalon Museum!"; }
        }

        public Task PlayGoldArmbandMessage()
        {
            return ReadRawText(ExhibitInfo.Text[2]);
        }
    }

}
