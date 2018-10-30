using AgateLib;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Xle.Services.MapLoad;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class FourJewels : LotaExhibit
    {
        public FourJewels() : base("Four Jewels", Coin.Ruby) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.FourJewels; } }

        public override string LongName
        {
            get
            {
                return "The four jewels";
            }
        }
        public override Color TitleColor
        {
            get { return XleColor.Yellow; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine("Would you like to go");
            await TextArea.PrintLine("to the four jewel dungeon?");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenuYesNo() == 0)
            {
                int map = Player.MapID;
                int x = Player.X;
                int y = Player.Y;
                Direction facing = Player.FaceDirection;

                Player.DungeonLevel = 0;

                await MapChanger.ChangeMap(73, 0);
                Player.SetReturnLocation(map, x, y, facing);
            }
        }

        public override bool StaticBeforeCoin
        {
            get
            {
                return false;
            }
        }
    }
}
