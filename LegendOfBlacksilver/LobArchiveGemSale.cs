using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle;
using Xle.Menus;
using Xle.Menus.Implementation;

namespace Xle.Blacksilver.Services
{
    [Singleton]
    public class LobArchiveGemSale : MuseumCoinSale
    {
        public LobFactory Factory { get; set; }

        public override double ChanceToOfferCoin
        {
            get { return 0.045; }
        }

        protected override int NextMuseumCoinOffer()
        {
            return -1;
        }

        public override void ResetCoinOffers()
        {
            
        }
    }
}
