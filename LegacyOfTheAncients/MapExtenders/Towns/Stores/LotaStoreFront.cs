﻿using Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Towns.Stores
{
    public class LotaStoreFront : StoreFront
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

    }
}
