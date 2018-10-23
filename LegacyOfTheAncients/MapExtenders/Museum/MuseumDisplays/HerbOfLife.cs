using AgateLib;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class HerbOfLife : LotaExhibit
    {
        public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.HerbOfLife; } }
        public override string LongName
        {
            get
            {
                return "The herb of life";
            }
        }

        public override void RunExhibit()
        {
            ReadRawText(RawText);

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to eat the fruit?");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
            {
                TextArea.PrintLine("You feel a tingling sensation.", XleColor.Green);
                SoundMan.PlaySoundSync(LotaSound.Good);

                Story.EatenJutonFruit = true;

                MarkAsVisited();
            }
        }
    }
}
