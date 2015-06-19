using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IMapChanger : IXleService
    {
        void SetMap(Maps.XleMap map);

        void ChangeMap(Player player, int mapId, int entryPoint);

        void ChangeMap(Player player, int mapId, AgateLib.Geometry.Point targetPoint);
    }
}
