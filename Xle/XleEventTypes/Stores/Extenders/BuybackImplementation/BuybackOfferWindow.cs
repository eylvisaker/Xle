using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.XleEventTypes.Stores.Extenders.BuybackImplementation
{
    public class BuybackOfferWindow : IBuybackOfferWindow
    {
        private TextWindow offerWind;

        public TextWindow TextWindow
        {
            get { return offerWind; }
            set { offerWind = value; }
        }

        public void RejectAskingPrice(int ask, bool wayTooHigh)
        {
            var clr = wayTooHigh ? XleColor.Yellow : XleColor.Cyan;

            offerWind.Clear();
            offerWind.WriteLine(" " + ask + " is " +
                (wayTooHigh ? "way " : "") + "too high!", clr);
        }

        public void SetOffer(int offer, int ask)
        {
            offerWind.Clear();
            offerWind.Write("My latest offer: ", XleColor.White);
            offerWind.WriteLine(offer.ToString(), XleColor.Cyan);

            if (ask > 0)
            {
                offerWind.Write("You asked for: ");
                offerWind.WriteLine(ask.ToString(), XleColor.Cyan);
            }
            else
                offerWind.WriteLine();

            offerWind.Write("What will you sell for? ");
            offerWind.Write("(0 to quit)", XleColor.Purple);
        }
    }
}
