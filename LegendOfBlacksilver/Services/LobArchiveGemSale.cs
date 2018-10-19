using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Menus.Implementation;

namespace Xle.LoB.Services
{
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
