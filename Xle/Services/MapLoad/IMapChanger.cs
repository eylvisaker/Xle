using AgateLib.Mathematics.Geometry;

using Xle.Maps;
using Microsoft.Xna.Framework;

namespace Xle.Services.MapLoad
{
    public interface IMapChanger : IXleService
    {
        void SetMap(IMapExtender map);

        void ChangeMap(int mapId, int entryPoint);
        void ChangeMap(int mapId, Point targetPoint);

        void ReturnToPreviousMap();
    }
}
