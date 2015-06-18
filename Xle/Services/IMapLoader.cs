using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IMapLoader : IXleService
    {
        void LoadMap(int mapId);

        void ChangeMap(Player player, int mapId, int entryPoint);
    }
}
