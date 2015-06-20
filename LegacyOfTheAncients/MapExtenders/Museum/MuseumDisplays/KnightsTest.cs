using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;
using ERY.Xle.Services;
using AgateLib.Geometry;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class KnightsTest : LotaExhibit
    {
        public KnightsTest() : base("The Test", Coin.Sapphire) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.KnightsTest; } }

        public override string LongName
        {
            get
            {
                return "A test for knights";
            }
        }
        public override void RunExhibit(Player player)
        {
            ReadRawText(RawText);

            int map = player.MapID;
            int x = player.X;
            int y = player.Y;
            Direction facing = player.FaceDirection;

            MapChanger.ChangeMap(player, 72, 0);
            player.SetReturnLocation(map, x, y, facing);

        }

        public override bool StaticBeforeCoin
        {
            get { return false; }
        }

        public override Color TitleColor
        {
            get { return XleColor.Cyan; }
        }
    }
}
