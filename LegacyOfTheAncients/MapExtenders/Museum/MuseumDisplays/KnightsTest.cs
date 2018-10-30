
using AgateLib;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Xle.Services.MapLoad;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class KnightsTest : LotaExhibit
    {
        public KnightsTest() : base("The Test", Coin.Sapphire) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.KnightsTest; } }

        public override string LongName
        {
            get
            {
                return "A test for knights";
            }
        }

        public override async Task RunExhibit()
        {
            await ReadRawText(RawText);

            int map = Player.MapID;
            int x = Player.X;
            int y = Player.Y;
            Direction facing = Player.FaceDirection;

            await MapChanger.ChangeMap(72, 0);
            Player.SetReturnLocation(map, x, y, facing);

            MarkAsVisited();
        }

        public override bool StaticBeforeCoin
        {
            get { return false; }
        }

        public override Color TitleColor => XleColor.Cyan;
    }
}
