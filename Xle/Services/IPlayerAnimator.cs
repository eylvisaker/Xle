﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IPlayerAnimator : IXleService
    {
        void AnimateStep();

        bool Animating { get; }

        int AnimFrame { get; }
    }
}
