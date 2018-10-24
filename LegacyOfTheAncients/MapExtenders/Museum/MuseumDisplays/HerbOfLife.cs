using AgateLib;
using System.Threading.Tasks;

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

        public override async Task RunExhibit()
        {
            await ReadRawText(RawText);

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to eat the fruit?");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
            {
                await TextArea.PrintLine("You feel a tingling sensation.", XleColor.Green);
                await SoundMan.PlaySoundWait(LotaSound.Good);

                Story.EatenJutonFruit = true;

                MarkAsVisited();
            }
        }
    }
}
