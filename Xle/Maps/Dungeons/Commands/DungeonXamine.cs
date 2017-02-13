using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Mathematics.Geometry;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Maps.Dungeons.Commands
{
    [ServiceName("DungeonXamine")]
    public class DungeonXamine : Xamine
    {
        public ISoundMan SoundMan { get; set; }
        public IXleGameControl GameControl { get; set; }
        public IDungeonAdapter DungeonAdapter { get; set; }

        public IXamineFormatter XamineFormatter { get; set; }

        public override void Execute()
        {
            SoundMan.PlaySound(LotaSound.Xamine);
            GameControl.Wait(500);

            TextArea.PrintLine("\n");
            PrintDungeonLevel();

            DungeonMonster foundMonster = FirstVisibleMonster(Player.Location, Player.FaceDirection, Player.DungeonLevel);
            int monsterDistance = RevealDistance(foundMonster);

            bool revealHidden = RevealTrapsUpTo(monsterDistance);

            if (revealHidden)
            {
                XamineFormatter.PrintHiddenObjectsDetected();
                SoundMan.PlaySound(LotaSound.XamineDetected);
            }

            if (foundMonster != null)
            {
                PrintExamineMonsterMessage(foundMonster);
            }
            else
            {
                PrintExamineObjectMessage();
            }
        }

        private void PrintExamineObjectMessage()
        {
            Point faceDir = GameState.Player.FaceDirection.ToPoint();
            DungeonTile tile = DungeonTile.Wall;
            int distance = 0;

            for (int i = 0; i < 5; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);
                tile = DungeonAdapter.TileAt(loc.X, loc.Y);

                distance = i;

                if (tile != DungeonTile.Empty)
                    break;
            }

            if (tile != DungeonTile.Wall &&
                    tile != DungeonTile.Empty)
            {
                XamineFormatter.DescribeTile(tile, distance);
            }
            else
            {
                XamineFormatter.PrintNothingUnusualInSight();
            }
        }

        private int RevealDistance(DungeonMonster foundMonster)
        {
            int distance = 5;

            if (foundMonster != null)
                distance = Math.Abs(foundMonster.Location.X - Player.Location.X) + Math.Abs(foundMonster.Location.Y + Player.Location.Y);

            return distance;
        }

        private bool RevealTrapsUpTo(int distance)
        {
            var faceDir = GameState.Player.FaceDirection.ToPoint();
            bool result = false;

            for (int i = 0; i < distance; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);

                if (DungeonAdapter.IsWallAt(loc))
                    break;

                result |= DungeonAdapter.RevealTrapAt(loc);
            }

            return result;
        }

        private DungeonMonster FirstVisibleMonster(Point location, Direction faceDirection, int dungeonLevel)
        {
            var faceDir = GameState.Player.FaceDirection.ToPoint();

            for (int i = 0; i < 5; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);

                var foundMonster = DungeonAdapter.MonsterAt(loc);

                if (foundMonster != null)
                    return foundMonster;
                if (DungeonAdapter.IsWallAt(loc))
                    return null;
            }

            return null;
        }

        private void PrintDungeonLevel()
        {
            if (PrintLevelDuringXamine)
            {
                TextArea.PrintLine("Level " + (Player.DungeonLevel + 1).ToString() + ".");
            }
        }

        protected virtual void PrintExamineMonsterMessage(DungeonMonster foundMonster)
        {
            XamineFormatter.DescribeMonster(foundMonster);
        }

        protected virtual bool PrintLevelDuringXamine
        {
            get { return true; }
        }

    }
}
