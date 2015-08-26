using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Geometry;

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

        DungeonExtender Dungeon { get { return (DungeonExtender)GameState.MapExtender; } }
        Dungeon Map { get { return (Dungeon)GameState.Map; } }

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
                TextArea.PrintLine("Hidden objects detected!!!", XleColor.White);
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
            Point faceDir = DungeonAdapter.FaceDirectionAsPoint;
            string objectName = "";
            int distance = 0;

            for (int i = 0; i < 5; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);
                var val = DungeonAdapter.TileAt(loc.X, loc.Y);

                if (val == DungeonTile.Wall) break;

                if (objectName == "")
                {
                    distance = i;

                    objectName = TileName(val);
                }
            }

            if (objectName != "")
            {
                string prefix = "A ";

                if ("aeiou".Contains(objectName.First()))
                    prefix = "An ";

                if (distance > 0)
                {
                    TextArea.PrintLine(prefix + objectName + " is in sight.");
                }
                else
                {
                    TextArea.PrintLine("You are standing next ");
                    TextArea.PrintLine("to " + prefix + objectName + ".");
                }
            }
            else
            {
                TextArea.PrintLine("Nothing unusual in sight.");
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
            var faceDir = DungeonAdapter.FaceDirectionAsPoint;
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
            var faceDir = DungeonAdapter.FaceDirectionAsPoint;

            for (int i = 0; i < 5; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);

                var foundMonster = Dungeon.MonsterAt(Player.DungeonLevel, loc);

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
            string name = " " + foundMonster.Name;
            if ("aeiou".Contains(foundMonster.Name[0]))
                name = "n" + name;

            TextArea.PrintLine("A" + name + " is stalking you!", XleColor.White);
        }

        protected virtual bool PrintLevelDuringXamine
        {
            get { return true; }
        }
        
        protected virtual string TileName(DungeonTile val)
        {
            switch (val)
            {
                case DungeonTile.CeilingHole: return "ceiling hole";
                case DungeonTile.FloorHole: return "floor hole";
                case DungeonTile.PoisonGasVent: return "poison gas vent";
                case DungeonTile.SlimeSplotch: return "slime splotch";
                case DungeonTile.TripWire: return "trip wire";
                case DungeonTile.GasVent: return "gas vent";
                case DungeonTile.Chest: return "treasure chest";
                case DungeonTile.Box: return "box";
                case DungeonTile.Urn: return "urn";
                default: return "";
            }
        }
    }
}
