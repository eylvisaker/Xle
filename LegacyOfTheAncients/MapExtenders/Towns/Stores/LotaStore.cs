using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns
{
    public class LotaStore : StoreExtender
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

    }
}
