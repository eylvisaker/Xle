using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps.Outdoors;
using Xle.Commands;
using Xle.Game;

namespace Xle.Ancients.MapExtenders.Outside
{
    [Transient("Tarmalon")]
    public class Tarmalon : OutsideExtender
    {
        protected LotaStory Story { get { return GameState.Story(); } }

        public override void OnLoad()
        {
            base.OnLoad();

            Story.Invisible = false;

            Player.RenderColor = XleColor.White;
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
