using Xle.Services.ScreenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Maps.Dungeons.Commands
{
    public interface IXamineFormatter 
    {
        void PrintNothingUnusualInSight();
        void PrintHiddenObjectsDetected();
        void DescribeTile(DungeonTile tripWire, int distance);
        void DescribeMonster(DungeonMonster monster);
    }

    [Singleton, InjectProperties]
    public class XamineFormatter : IXamineFormatter
    {
        public ITextArea TextArea { get; set; }

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

        public void DescribeTile(DungeonTile tile, int distance)
        {
            var objectName = TileName(tile);
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

        public void PrintHiddenObjectsDetected()
        {
            TextArea.PrintLine("Hidden objects detected!!!", XleColor.White);
        }

        public void PrintNothingUnusualInSight()
        {
            TextArea.PrintLine("Nothing unusual in sight.");
        }

        public void DescribeMonster(DungeonMonster monster)
        {
            string name = " " + monster.Name;
            if ("aeiou".Contains(monster.Name[0]))
                name = "n" + name;

            TextArea.PrintLine("A" + name + " is stalking you!", XleColor.White);

        }
    }
}
