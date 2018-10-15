using System.Collections.Generic;

namespace ERY.Xle.Maps.XleMapTypes
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
