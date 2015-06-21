using ERY.Xle.LoB.MapExtenders.Archives.Exhibits;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
    public class OwlArchives : LobArchives
    {
        Dictionary<int, Exhibit> mExhibits = new Dictionary<int, Exhibit>();

        public OwlArchives(IExhibitFactory factory)
        {
            mExhibits.Add(0x5e, factory.MetalWork());
            mExhibits.Add(0x5f, factory.SingingCrystal());
            mExhibits.Add(0x5D, factory.IslandRetreat());

            mExhibits.Add(0x56, factory.GameOfHonor());
            mExhibits.Add(0x55, factory.StormingGear());
            mExhibits.Add(0x57, factory.TheWealthy());

            mExhibits.Add(0x58, factory.Mountains());
            mExhibits.Add(0x59, factory.MagicEtherium());
            mExhibits.Add(0x50, factory.VaseOfSouls());
        }


        public override Exhibit GetExhibitByTile(int tile)
        {
            if (mExhibits.ContainsKey(tile) == false)
                return null;

            return mExhibits[tile];
        }

        public override void NeedsCoinMessage(Player player, Exhibit ex)
        {
            TextArea.PrintLine(ex.UseCoinMessage);
        }
    }
}
