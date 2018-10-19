using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Services.Commands;

namespace Xle.LoB.MapExtenders.Dungeon
{
    public class PitsOfBlackmire : LobDungeon
    {
        public override int GetTreasure(int dungeonLevel, int chestID)
        {
            if (chestID == 3)
            {
                switch (dungeonLevel)
                {
                    case 2: return -1;
                    case 3: return (int)LobItem.RustyKey;
                    case 6: return (int)LobItem.SkeletonKey;
                    case 10: return (int)LobItem.Blacksilver;
                }
            }

            if (chestID == 2)
            {
                switch (dungeonLevel)
                {
                    case 2:
                        return (int)LobItem.BlackWand;

                    default:
                        return (int)LobItem.WhiteDiamond;
                }
            }

            return base.GetTreasure(dungeonLevel, chestID);
        }

        public override void SetCommands(ICommandList commands)
        {
            base.SetCommands(commands);

            commands.Items.Remove(commands.Items.First(x => x.Name.Equals("Climb", StringComparison.OrdinalIgnoreCase)));

            commands.Items.Add(CommandFactory.Climb("BlackmireClimb"));

            commands.Items.Remove(commands.Items.First(x => x.Name.Equals("Use", StringComparison.OrdinalIgnoreCase)));

            commands.Items.Add(CommandFactory.Use("BlackmireUse"));

        }

        public override void OnBeforeGiveItem(ref int treasure, ref bool handled, ref bool clearBox)
        {
            if (treasure == -1)
            {
                TextArea.PrintLine("You need a key.", XleColor.Yellow);

                handled = true;
                clearBox = false;
            }
        }
        
        protected override int MonsterGroup(int dungeonLevel)
        {
            if (dungeonLevel <= 2) return 0;
            if (dungeonLevel <= 6) return 1;

            return 2;
        }

        public override Map3DSurfaces Surfaces()
        {
            return Lob3DSurfaces.PitsOfBlackmire;
        }
    }
}
