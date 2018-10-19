﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Xle.Services.Rendering.Maps;

namespace Xle.LoB.MapExtenders.Archives
{
    public class ArchiveRenderer : MuseumRenderer
    {
        protected override Rectangle ExhibitCloseupRect
        {
            get
            {
                var result = base.ExhibitCloseupRect;

                result.Y -= 16;

                return result;
            }
        }
    }
}
