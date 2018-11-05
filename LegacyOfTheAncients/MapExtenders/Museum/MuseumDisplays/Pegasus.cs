using AgateLib;
using System.Threading.Tasks;
using Xle.MapLoad;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class Pegasus : LotaExhibit
    {
        public Pegasus() : base("Pegasus", Coin.Diamond) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Pegasus; } }

        public override string LongName
        {
            get { return "A flight of fancy"; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to climb on?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                if (Player.Food < 150)
                    Player.Food = 150;

                await MapChanger.ChangeMap(3, 0);
            }
        }

        public override bool StaticBeforeCoin
        {
            get { return false; }
        }
    }
}
