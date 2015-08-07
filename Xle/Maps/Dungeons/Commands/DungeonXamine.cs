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

        DungeonExtender Dungeon { get { return (DungeonExtender)GameState.MapExtender; } }
        Dungeon TheMap { get { return (Dungeon)GameState.Map; } }

        public override void Execute()
        {
            SoundMan.PlaySound(LotaSound.Xamine);
            GameControl.Wait(500);

            Point faceDir = new Point();

            switch (Player.FaceDirection)
            {
                case Direction.East: faceDir = new Point(1, 0); break;
                case Direction.West: faceDir = new Point(-1, 0); break;
                case Direction.North: faceDir = new Point(0, -1); break;
                case Direction.South: faceDir = new Point(0, 1); break;
            }

            TextArea.PrintLine("\n");

            bool revealHidden = false;
            DungeonMonster foundMonster = null;

            for (int i = 0; i < 5; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);

                foundMonster = Dungeon.MonsterAt(Player.DungeonLevel, loc);

                if (foundMonster != null)
                    break;
                if (TheMap[loc.X, loc.Y] < 0x10)
                    break;
                if (TheMap[loc.X, loc.Y] >= 0x21 && TheMap[loc.X, loc.Y] < 0x2a)
                {
                    TheMap[loc.X, loc.Y] -= 0x10;
                    revealHidden = true;
                }
            }

            if (revealHidden)
            {
                TextArea.PrintLine("Hidden objects detected!!!", XleColor.White);
                SoundMan.PlaySound(LotaSound.XamineDetected);
            }

            string extraText = string.Empty;
            int distance = 0;

            if (foundMonster != null)
            {
                PrintExamineMonsterMessage(foundMonster);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);
                    int val = TheMap[loc.X, loc.Y];

                    if (val < 0x10) break;

                    if (extraText == string.Empty)
                    {
                        distance = i;

                        if (val > 0x10 && val < 0x1a)
                        {
                            extraText = TrapName(val);
                        }
                        if (val >= 0x30 && val <= 0x3f)
                        {
                            extraText = "treasure chest";
                        }
                        if (val == 0x1e)
                        {
                            extraText = "box";
                        }
                    }
                }

                if (extraText != string.Empty)
                {
                    if (distance > 0)
                    {
                        TextArea.PrintLine("A " + extraText + " is in sight.");
                    }
                    else
                    {
                        TextArea.PrintLine("You are standing next ");
                        TextArea.PrintLine("to a " + extraText + ".");
                    }
                }
                else
                {
                    if (PrintLevelDuringXamine)
                    {
                        TextArea.PrintLine("Level " + (Player.DungeonLevel + 1).ToString() + ".");
                    }

                    TextArea.PrintLine("Nothing unusual in sight.");
                }
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

        protected virtual string TrapName(int val)
        {
            switch (val)
            {
                case 0x11: return "ceiling hole";
                case 0x12: return "floor hole";
                case 0x13: return "poison gas vent";
                case 0x14: return "slime splotch";
                case 0x15: return "trip wire";
                case 0x16: return "gas vent";
                default: throw new ArgumentException();
            }
        }
    }
}
