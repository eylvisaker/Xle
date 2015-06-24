using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.MapLoad;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class FourJewels : LotaExhibit
    {
        public FourJewels() : base("Four Jewels", Coin.Ruby) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.FourJewels; } }

        public override string LongName
        {
            get
            {
                return "The four jewels";
            }
        }
        public override AgateLib.Geometry.Color TitleColor
        {
            get { return XleColor.Yellow; }
        }
        public override void RunExhibit(Player player)
        {
            base.RunExhibit(player);

            TextArea.PrintLine("Would you like to go");
            TextArea.PrintLine("to the four jewel dungeon?");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenuYesNo() == 0)
            {
                int map = player.MapID;
                int x = player.X;
                int y = player.Y;
                Direction facing = player.FaceDirection;

                player.DungeonLevel = 0;

                MapChanger.ChangeMap(player, 73, 0);
                player.SetReturnLocation(map, x, y, facing);
            }
        }

        public override bool StaticBeforeCoin
        {
            get
            {
                return false;
            }
        }
    }
}
