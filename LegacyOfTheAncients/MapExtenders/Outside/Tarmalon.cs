using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps.Outdoors;
using Xle.Services.Commands;
using Xle.Services.Game;

namespace Xle.Ancients.MapExtenders.Outside
{
    [Transient("Tarmalon")]
    public class Tarmalon : OutsideExtender
    {
        private int banditAmbush;

        protected LotaStory Story { get { return GameState.Story(); } }

        public SolidColorScreenRenderer UnconsciousRenderer { get; set; }

        public override void OnLoad()
        {
            base.OnLoad();

            Story.Invisible = false;

            Player.RenderColor = XleColor.White;

            SetBanditAmbushTime();
        }

        public int WaterAnimLevel
        {
            get { return RenderState.WaterAnimLevel; }
            set { RenderState.WaterAnimLevel = value; }
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Hold());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());
            commands.Items.Add(CommandFactory.Disembark());
            commands.Items.Add(CommandFactory.End());
            commands.Items.Add(CommandFactory.Fight("OutsideFight"));
            commands.Items.Add(CommandFactory.Magic("LotaOutsideMagic"));
            commands.Items.Add(CommandFactory.Speak("OutsideSpeak"));
            commands.Items.Add(CommandFactory.Use("LotaUse"));
            commands.Items.Add(CommandFactory.Xamine("OutsideXamine"));
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            base.SetColorScheme(scheme);

            scheme.MapAreaWidth = 23;
        }

        public override async Task<bool> UpdateEncounterState()
        {
            return await BanditAmbush();
        }

        private bool AllowBanditAmbush()
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

            int min = 40 - pastTime / 2;
            if (min < 3) min = 3;

            int max = 100 - pastTime / 5;
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

        private async Task<bool> BanditAmbush()
        {
            if (AllowBanditAmbush() == false)
                return false;

            if (banditAmbush <= 0)
                SetBanditAmbushTime();

            if (Player.TimeDays <= banditAmbush)
                return false;

            // set a random position for the appearance of the bandits.
            SetMonsterImagePosition();

            // bandit icon is number 4.
            RenderState.DisplayMonsterID = 4;

            await TextArea.PrintLine();
            await TextArea.PrintLine("You are ambushed by bandits!", XleColor.Cyan);

            GameControl.PlaySound(LotaSound.Encounter);
            await GameControl.WaitAsync(500);

            int maxDamage = Player.HP / 15;
            int minDamage = Math.Min(5, maxDamage / 2);

            for (int i = 0; i < 8; i++)
            {
                Player.HP -= Random.Next(minDamage, maxDamage + 1);

                GameControl.PlaySound(LotaSound.EnemyHit);
                await GameControl.WaitAsync(250);
            }

            await TextArea.PrintLine("You fall unconsious.", XleColor.Yellow);

            await GameControl.WaitAsync(1000);
            RenderState.DisplayMonsterID = -1;
            await GameControl.WaitAsync(3000, redraw: UnconsciousRenderer);

            await TextArea.PrintLine();
            await TextArea.PrintLine("You awake.  The compendium is gone.");
            await TextArea.PrintLine();

            Player.Items[LotaItem.Compendium] = 0;

            await GameControl.PlaySoundSync(LotaSound.VeryBad);

            await TextArea.PrintLine("You hear a voice...");

            await TextArea.PrintLine();
            await TextArea.PrintLineSlow("Do not be discouraged.  It was\ninevitable.  Keep to your quest.");

            await GameControl.WaitAsync(3000);
            banditAmbush = 0;

            return true;
        }

        /// <summary>
        /// Returns true if the player drowns.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private async Task<bool> CheckStormy(Player player)
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
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("You are sailing into stormy water.", XleColor.Yellow);
                }
                else if (WaterAnimLevel == 2 || WaterAnimLevel == 3)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("The water is now very rough.", XleColor.White);
                    await TextArea.PrintLine("It will soon swamp your raft.", XleColor.Yellow);
                }
                else if (WaterAnimLevel == 1 && wasStormy == 2)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("You are out of immediate danger.", XleColor.Yellow);
                }
                else if (WaterAnimLevel == 0 && wasStormy == 1)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("You leave the storm behind.", XleColor.Cyan);
                }

                if (WaterAnimLevel == 3)
                {
                    await TextArea.PrintLine();
                    await TextArea.PrintLine("Your raft sinks.", XleColor.Yellow);
                    await TextArea.PrintLine();
                }

                await GameControl.WaitAsync(1000);

                if (WaterAnimLevel == 3)
                {
                    player.HP = 0;
                    return true;
                }
            }

            return false;
        }

        public override async Task CastSpell(MagicSpell magic)
        {
            if (magic.ID == 6)
            {
                await CastSeekSpell();
            }
        }

        private async Task CastSeekSpell()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine("Cast seek spell.");

            if (Player.IsOnRaft)
            {
                await TextArea.PrintLine("The water mutes the spell.");
            }
            else if (TheMap.MapID != 1)
            {
                await TextArea.PrintLine("You're too far away.");
            }
            else
            {
                Player.FaceDirection = Direction.West;
                await GameControl.PlaySoundSync(LotaSound.VeryGood);

                await MapChanger.ChangeMap(1, 0);
                OutsideEncounters.CancelEncounter();
            }
        }

    }
}
