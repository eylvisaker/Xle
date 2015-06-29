using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
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

        public override void OnBeforeGiveItem(ref int treasure, ref bool handled, ref bool clearBox)
        {
            if (treasure == -1)
            {
                TextArea.PrintLine("You need a key.", XleColor.Yellow);

                handled = true;
                clearBox = false;
            }
        }

        public override void PlayerUse(int item, ref bool handled)
        {
            if (item == (int)LobItem.RustyKey)
            {
                if (Player.DungeonLevel + 1 == 2 &&
                    TheMap[Player.X, Player.Y] == 0x33)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("A hole appears!", XleColor.White);

                    TheMap[Player.X, Player.Y] = 0x12;

                    SoundMan.PlaySoundSync(LotaSound.VeryGood);

                    handled = true;
                }
            }
            base.PlayerUse(item, ref handled);
        }

        public override void PlayerMagic()
        {
            base.PlayerMagic();

            if (Player.DungeonLevel >= 6 && Story.Illusion == false)
            {
                // turn off the display.
            }
        }
        public override bool PlayerClimb()
        {
            var result = base.PlayerClimb();

            if (Player.DungeonLevel == 4 && Story.RotlungContracted == false)
            {
                SoundMan.PlaySound(LotaSound.VeryBad);

                TextArea.PrintLineSlow("You have contracted rotlung.");
                TextArea.PrintLine("Endurance  - 10");

                Story.RotlungContracted = true;
                Player.Attribute[Attributes.endurance] -= 10;
            }

            if (Player.DungeonLevel == 6 && Story.Illusion == false)
            {
                // TODO: turn off the display.
            }

            return result;
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
