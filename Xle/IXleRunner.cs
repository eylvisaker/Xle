﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle
{
    public interface IXleRunner : IXleService
    {
        void Run(IXleGameFactory gameFactory);
    }
}
