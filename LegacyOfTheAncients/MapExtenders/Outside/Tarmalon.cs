using ERY.Xle.Data;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
    public class Tarmalon : OutsideExtender
    {
        int banditAmbush;

        public IMapChanger MapChanger { get; set; }

        public override void OnLoad(GameState state)
        {
            base.OnLoad(state);

            Lota.Story.Invisible = false;

            Player.RenderColor = XleColor.White;

            SetBanditAmbushTime();
        }

        public int WaterAnimLevel
        {
            get { return MapRenderer.WaterAnimLevel; }
            set { MapRenderer.WaterAnimLevel = value; }
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LotaProgram.CommonLotaCommands);

            commands.Items.Add(CommandFactory.Disembark());
            commands.Items.Add(CommandFactory.End());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Speak());
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            base.SetColorScheme(scheme);

            scheme.MapAreaWidth = 23;
        }

        public override void UpdateEncounterState(GameState state, ref bool handled)
        {
            handled = BanditAmbush();
        }

        bool AllowBanditAmbush()
        {
            // make sure the player has the compendium
            if (Player.Items[LotaItem.Compendium] == 0) return false;

            // if the player has the guard jewels we bail.
            if (Player.Items[LotaItem.GuardJewel] == 4) return false;

            return true;
        }

        /// <summary>
        /// Check to see if we should have bandits ambush the player. If so,
        /// sets banditAmbush variable.
        /// </summary>
        /// <param name="player"></param>
        private void SetBanditAmbushTime()
        {
            if (AllowBanditAmbush() == false)
                return;

            int pastTime = (int)(Player.TimeDays - 100);
            if (pastTime < 0) pastTime = 0;

            int min = 40 - (int)(pastTime / 2);
            if (min < 3) min = 3;

            int max = 100 - (int)(pastTime / 5);
            if (max < 12) max = 12;

            int time = Random.Next(min, max);

            if (time > Player.Food - 2)
            {
                time = (int)Player.Food - 2;
                if (time < 0)
                    time = 1;
            }

            banditAmbush = (int)(Player.TimeDays) + time;
        }
        private bool BanditAmbush()
        {
            if (AllowBanditAmbush() == false)
                return false;

            if (banditAmbush <= 0)
                SetBanditAmbushTime();

            if (Player.TimeDays <= banditAmbush)
                return false;

            // set a random position for the appearance of the bandits.
            SetMonsterImagePosition(Player);

            // bandit icon is number 4.
            MapRenderer.DisplayMonsterID = 4;

            TextArea.PrintLine();
            TextArea.PrintLine("You are ambushed by bandits!", XleColor.Cyan);

            SoundMan.PlaySound(LotaSound.Encounter);
            GameControl.Wait(500);

            int maxDamage = Player.HP / 15;
            int minDamage = Math.Min(5, maxDamage / 2);

            for (int i = 0; i < 8; i++)
            {
                Player.HP -= Random.Next(minDamage, maxDamage + 1);

                SoundMan.PlaySound(LotaSound.EnemyHit);
                GameControl.Wait(250);
            }

            TextArea.PrintLine("You fall unconsious.", XleColor.Yellow);

            GameControl.Wait(1000);
            MapRenderer.DisplayMonsterID = -1;
            GameControl.Wait(3000, redraw: RedrawUnconscious);

            TextArea.PrintLine();
            TextArea.PrintLine("You awake.  The compendium is gone.");
            TextArea.PrintLine();

            Player.Items[LotaItem.Compendium] = 0;

            SoundMan.PlaySoundSync(LotaSound.VeryBad);

            TextArea.PrintLine("You hear a voice...");

            TextArea.PrintLine();
            TextArea.PrintLineSlow("Do not be discouraged.  It was\ninevitable.  Keep to your quest.");

            GameControl.Wait(3000);
            banditAmbush = 0;

            return true;
        }

        void RedrawUnconscious()
        {
            AgateLib.DisplayLib.Display.BeginFrame();
            AgateLib.DisplayLib.Display.Clear(XleColor.Gray);
            AgateLib.DisplayLib.Display.EndFrame();
        }

        /// <summary>
        /// Returns true if the player drowns.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool CheckStormy(Player player)
        {
            int wasStormy = WaterAnimLevel;


            if (player.X < -45 || player.X > TheMap.Width + 45 ||
                player.Y < -45 || player.Y > TheMap.Height + 45)
            {
                WaterAnimLevel = 3;
            }
            else if (player.X < -30 || player.X > TheMap.Width + 30 ||
                player.Y < -30 || player.Y > TheMap.Height + 30)
            {
                WaterAnimLevel = 2;
            }
            else if (player.X < -15 || player.X > TheMap.Width + 15 ||
                player.Y < -15 || player.Y > TheMap.Height + 15)
            {
                WaterAnimLevel = 1;
            }
            else
            {
                WaterAnimLevel = 0;
            }

            if (WaterAnimLevel != wasStormy || WaterAnimLevel >= 2)
            {
                if (WaterAnimLevel == 1 && wasStormy == 0)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("You are sailing into stormy water.", XleColor.Yellow);
                }
                else if (WaterAnimLevel == 2 || WaterAnimLevel == 3)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("The water is now very rough.", XleColor.White);
                    TextArea.PrintLine("It will soon swamp your raft.", XleColor.Yellow);
                }
                else if (WaterAnimLevel == 1 && wasStormy == 2)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("You are out of immediate danger.", XleColor.Yellow);
                }
                else if (WaterAnimLevel == 0 && wasStormy == 1)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("You leave the storm behind.", XleColor.Cyan);
                }

                if (WaterAnimLevel == 3)
                {
                    TextArea.PrintLine();
                    TextArea.PrintLine("Your raft sinks.", XleColor.Yellow);
                    TextArea.PrintLine();
                }

                GameControl.Wait(1000);

                if (WaterAnimLevel == 3)
                {
                    player.HP = 0;
                    return true;
                }

            }
            return false;
        }

        public override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                yield return Data.MagicSpells[1];
                yield return Data.MagicSpells[2];
                yield return Data.MagicSpells[6];
            }
        }

        public override void CastSpell(GameState state, MagicSpell magic)
        {
            if (magic.ID == 6)
            {
                CastSeekSpell(state);
            }
        }

        private void CastSeekSpell(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine("Cast seek spell.");

            if (state.Player.IsOnRaft)
            {
                TextArea.PrintLine("The water mutes the spell.");
            }
            else if (TheMap.MapID != 1)
            {
                TextArea.PrintLine("You're too far away.");
            }
            else
            {
                state.Player.FaceDirection = Direction.West;
                SoundMan.PlaySoundSync(LotaSound.VeryGood);

                MapChanger.ChangeMap(state.Player, 1, 0);
                EncounterState = 0;
            }
        }

    }
}
