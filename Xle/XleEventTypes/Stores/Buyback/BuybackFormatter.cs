using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Data;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.XleEventTypes.Stores.Buyback
{
    public class BuybackFormatter : IBuybackFormatter
    {
        public ITextArea TextArea { get; set; }
        public XleData Data { get; set; }

        public void Offer(Equipment item, int offer, bool finalOffer)
        {
            TextArea.Clear();
            TextArea.PrintLine("I'll give " + offer + " gold for your");
            TextArea.Print(item.NameWithQuality(Data));

            if (finalOffer)
            {
                TextArea.PrintLine(" -final offer!!!", XleColor.Yellow);
            }
            else
                TextArea.PrintLine(".");
        }

        public void ComeBackWhenSerious()
        {
            TextArea.Clear();
            TextArea.PrintLine("Come back when you're serious.");
        }

        public void MaybeDealLater()
        {
            TextArea.Clear();
            TextArea.PrintLine("Maybe we can deal later.");
        }

        public void CompleteSale(Equipment item, int offer)
        {
            TextArea.Clear();
            TextArea.PrintLine("It's a deal!");
            TextArea.PrintLine(item.BaseName(Data) + " sold for " + offer + " gold.");
        }

        public void SeeYouLater()
        {
            TextArea.PrintLine("\n\n\n\nSee you later.\n");
        }

        public void InitialMenuPrompt()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("Select (0 to cancel)");
            TextArea.PrintLine();
        }

        public void ClearTextArea()
        {
            TextArea.Clear();
        }
    }
}
