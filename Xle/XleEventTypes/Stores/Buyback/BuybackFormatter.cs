using AgateLib;
using System.Threading.Tasks;
using Xle.Data;
using Xle.ScreenModel;

namespace Xle.XleEventTypes.Stores.Buyback
{
    public interface IBuybackFormatter
    {
        Task Offer(Equipment item, int offer, bool finalOffer);
        Task ComeBackWhenSerious();
        Task MaybeDealLater();
        Task CompleteSale(Equipment item, int amount);
        Task SeeYouLater();
        Task InitialMenuPrompt();
        void ClearTextArea();
    }

    [Singleton, InjectProperties]
    public class BuybackFormatter : IBuybackFormatter
    {
        public ITextArea TextArea { get; set; }
        public XleData Data { get; set; }

        public async Task Offer(Equipment item, int offer, bool finalOffer)
        {
            TextArea.Clear();
            await TextArea.PrintLine("I'll give " + offer + " gold for your");
            await TextArea.Print(item.NameWithQuality(Data));

            if (finalOffer)
            {
                await TextArea.PrintLine(" -final offer!!!", XleColor.Yellow);
            }
            else
                await TextArea.PrintLine(".");
        }

        public async Task ComeBackWhenSerious()
        {
            TextArea.Clear();
            await TextArea.PrintLine("Come back when you're serious.");
        }

        public async Task MaybeDealLater()
        {
            TextArea.Clear();
            await TextArea.PrintLine("Maybe we can deal later.");
        }

        public async Task CompleteSale(Equipment item, int offer)
        {
            TextArea.Clear();
            await TextArea.PrintLine("It's a deal!");
            await TextArea.PrintLine(item.BaseName(Data) + " sold for " + offer + " gold.");
        }

        public Task SeeYouLater() => TextArea.PrintLine("\n\n\n\nSee you later.\n");

        public async Task InitialMenuPrompt()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Select (0 to cancel)");
            await TextArea.PrintLine();
        }

        public void ClearTextArea()
        {
            TextArea.Clear();
        }
    }
}
