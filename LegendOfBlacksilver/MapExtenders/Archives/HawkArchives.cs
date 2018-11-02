using Xle.Blacksilver.MapExtenders.Archives.Exhibits;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Archives
{
    [Transient("HawkArchives")]
    public class HawkArchives : LobArchives
    {
        Dictionary<int, LobExhibit> mExhibits = new Dictionary<int, LobExhibit>();

        public HawkArchives(IExhibitFactory factory)
        {
            mExhibits.Add(0x52, factory.MorningStar());
            mExhibits.Add(0x53, factory.MarthbaneTunnels());

            mExhibits.Add(0x55, factory.UnderwaterPort());
            mExhibits.Add(0x56, factory.DarkWand());

            mExhibits.Add(0x50, factory.Blacksmith());
            mExhibits.Add(0x51, factory.FlaxtonIncense());
            mExhibits.Add(0x5e, factory.KloryksCage());

            mExhibits.Add(0x5a, factory.CrystalTears());
        }

        public override Exhibit GetExhibitByTile(int tile)
        {
            if (mExhibits.ContainsKey(tile) == false)
                return null;

            return mExhibits[tile];
        }
    }
}
