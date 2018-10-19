using System.Collections.Generic;

namespace Xle.Maps.XleMapTypes
{
    public class CastleMap : Town
    {
        public override IEnumerable<string> AvailableTileImages
        {
            get
            {
                yield return "CastleTiles.png";
            }
        }

    }
}
