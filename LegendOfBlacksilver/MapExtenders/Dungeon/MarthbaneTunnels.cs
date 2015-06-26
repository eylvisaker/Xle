using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Services;
using ERY.Xle.Services.Menus;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
    public class MarthbaneTunnels : LobDungeon
    {
        DungeonMonster king;

        public IQuickMenu QuickMenu { get; set; }

        public override void OnLoad(GameState state)
        {
            if (Story.RescuedKing)
            {
                OpenEscapeRoute(state);
            }
            else
            {
                king = new DungeonMonster(Data.DungeonMonsters[18])
                    {
                        DungeonLevel = 7,
                        Location = new Point(10, 0),
                        HP = 400,
                        KillFlashImmune = true,
                    };

                Combat.Monsters.Add(king);
            }
        }
        protected override int MonsterGroup(int dungeonLevel)
        {
            if (dungeonLevel <= 2) return 0;
            if (dungeonLevel <= 6) return 1;

            return 2;
        }

        public override DungeonMonster GetMonsterToSpawn(GameState state)
        {
            if (Player.DungeonLevel == 7)
                return null;

            return base.GetMonsterToSpawn(state);
        }

        public override bool SpawnMonsters(GameState state)
        {
            if (Player.DungeonLevel == 7)
                return false;
            else
                return true;
        }

        public override void UpdateMonsters(GameState state)
        {
            // disable normal monster processing if we see the king.
            if (Player.DungeonLevel == 7)
                return;

            base.UpdateMonsters(state);
        }

        public override void PrintExamineMonsterMessage(DungeonMonster foundMonster, ref bool handled)
        {
            if (foundMonster.Data.Name == "king")
            {
                TextArea.PrintLine("You see a king!", XleColor.White);
                handled = true;
            }
        }

        public override bool PlayerSpeak(GameState unused)
        {
            if (Player.DungeonLevel != 7) return false;
            if (king == null) return false;
            if (king.HP <= 0) return false;

            if (Story.MarthbaneOfferedHelpToKing == false)
            {
                SoundMan.PlaySound(LotaSound.VeryGood);

                TextArea.Clear(true);
                TextArea.PrintLineSlow("I am king durek!!", XleColor.White);
                TextArea.PrintLineSlow("Do you come to help me?", XleColor.White);
                TextArea.PrintLineSlow();

                if (QuickMenu.QuickMenuYesNo() == 1)
                {
                    DoomedMessage();
                    return true;
                }

                Story.MarthbaneOfferedHelpToKing = true;

                TextArea.Clear(true);
                TextArea.PrintLineSlow("I fear you have been caught in the", XleColor.White);
                TextArea.PrintLineSlow("same trap that imprisons me...", XleColor.White);
                TextArea.PrintLineSlow();
                TextArea.PrintLineSlow("unless...", XleColor.White);

                GameControl.Wait(2000);
            }

            TextArea.Clear(true);
            TextArea.PrintLineSlow("Do you carry my signet ring?", XleColor.White);
            TextArea.PrintLineSlow();

            if (QuickMenu.QuickMenuYesNo() == 1)
            {
                DoomedMessage();
                return true;
            }

            GameState.Player.Items[LobItem.SignetRing] = 0;

            TextArea.Clear(true);
            TextArea.PrintLineSlow("In times of distress, the ring will\nreturn me to the castle!!  I fear it\ncan do nothing more than give you a\nroute of escape.", XleColor.White);

            GameControl.Wait(3000);

            TextArea.Clear(true);
            TextArea.PrintLineSlow("\n\n\nNoble adventurer, i am in your debt.\nMay we meet in better times.", XleColor.White);

            GameControl.Wait(3000);

            SoundMan.PlaySound(LotaSound.EnemyMiss);

            Story.RescuedKing = true;
            Combat.Monsters.Remove(king);

            OpenEscapeRoute(GameState);

            return true;
        }

        private void OpenEscapeRoute(GameState state)
        {
            // 11, 0, 7 change to 17
            // 13, 0, 4 change to 18

            TheMap[11, 0, 7] = 17;
            TheMap[13, 0, 4] = 18;
        }

        private void DoomedMessage()
        {
            TextArea.PrintLine();
            TextArea.PrintLine("Then I fear we are both doomed.", XleColor.White);
            TextArea.PrintLine();
        }

        public override Maps.Map3DSurfaces Surfaces(GameState state)
        {
            return Lob3DSurfaces.MarthbaneTunnels;
        }
    }
}
