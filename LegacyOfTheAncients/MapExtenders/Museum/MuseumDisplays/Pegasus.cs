using AgateLib;
using Xle.Services.MapLoad;

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

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to climb on?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                if (Player.Food < 150)
                    Player.Food = 150;

                MapChanger.ChangeMap(3, 0);
            }
        }

        public override bool StaticBeforeCoin
        {
            get { return false; }
        }
    }
}
