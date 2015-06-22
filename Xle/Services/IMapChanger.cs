using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps.Extenders;

namespace ERY.Xle.Services
{
    public interface IMapChanger : IXleService
    {
        void SetMap(MapExtender map);

        void ChangeMap(Player player, int mapId, int entryPoint);

        void ChangeMap(Player player, int mapId, AgateLib.Geometry.Point targetPoint);

        void ReturnToPreviousMap();
    }
}
