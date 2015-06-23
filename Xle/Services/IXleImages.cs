using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IXleImages : IXleService
    {
        void LoadTiles(string tileset);

        ISurface Tiles { get; }
    }
}
