﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IMuseumCoinSale : IXleService
    {
        void OfferMuseumCoin();

        bool RollToOfferCoin();
    }
}
