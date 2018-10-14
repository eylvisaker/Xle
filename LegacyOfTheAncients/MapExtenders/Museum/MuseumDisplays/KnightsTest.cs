﻿
using ERY.Xle.Services.MapLoad;
using Microsoft.Xna.Framework;

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
        public override void RunExhibit()
        {
            ReadRawText(RawText);

            int map = Player.MapID;
            int x = Player.X;
            int y = Player.Y;
            Direction facing = Player.FaceDirection;

            MapChanger.ChangeMap(72, 0);
            Player.SetReturnLocation(map, x, y, facing);

            MarkAsVisited();
        }

        public override bool StaticBeforeCoin
        {
            get { return false; }
        }

        public override Color TitleColor => XleColor.Cyan;
    }
}
