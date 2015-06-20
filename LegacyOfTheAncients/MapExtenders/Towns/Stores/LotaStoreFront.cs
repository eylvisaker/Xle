using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
    public class LotaStoreFront : StoreFront
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

    }
}
