using System;
using System.Threading.Tasks;

using Xle.Data;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Maps.Dungeons.Commands
{
    public class DungeonOpenCommand : Open
    {
        public DungeonOpenCommand()
        {

        }

        public ISoundMan SoundMan { get; set; }
        public Random Random { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }
        public XleData Data { get; set; }

        public IDungeonAdapter DungeonAdapter { get; set; }

        public override async Task Execute()
        {
            DungeonTile tile = DungeonAdapter.TileAt(Player.X, Player.Y);

            if (tile == DungeonTile.Urn)
            {
                await OpenUrn();
            }
            else if (tile == DungeonTile.Box)
            {
                await OpenBox();
            }
            else if (tile == DungeonTile.Chest)
            {
                await OpenChest(DungeonAdapter.ChestValueAt(Player.X, Player.Y));
            }
            else
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("Nothing to open.");
                await GameControl.WaitAsync(1000);
            }
        }

        private async Task OpenUrn()
        {
            SoundMan.PlaySound(LotaSound.OpenChest);

            await TextArea.PrintLine(" Urn");
            await TextArea.PrintLine();
            await GameControl.WaitAsync(500);

            GiveUrnContents();

            await SoundMan.FinishSounds();
            DungeonAdapter.ClearSpace(Player.X, Player.Y);
        }

        private async Task OpenBox()
        {
            await TextArea.PrintLine(" Box");
            await TextArea.PrintLine();
            SoundMan.PlaySound(LotaSound.OpenChest);
            await GameControl.WaitAsync(500);

            GiveBoxContents();

            await SoundMan.FinishSounds();

            DungeonAdapter.ClearSpace(Player.X, Player.Y);
        }


        protected virtual void GiveUrnContents()
        {
            GiveHealing();
        }

        protected virtual void GiveBoxContents()
        {
            GiveHealing();
        }

        private void GiveHealing()
        {
            int amount = Random.Next(60, 200);

            amount = Math.Min(amount, Player.MaxHP - Player.HP);

            if (amount <= 0)
            {
                TextArea.PrintLine("You find nothing.", XleColor.Yellow);
            }
            else
            {
                TextArea.PrintLine("Hit points:  + " + amount, XleColor.Yellow);
                Player.HP += amount;
                SoundMan.PlaySound(LotaSound.Good);
                StatsDisplay.FlashHPWhileSound(XleColor.Yellow);
            }
        }

        private async Task OpenChest(int val)
        {
            await TextArea.PrintLine(" Chest");
            await TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.OpenChest);
            await GameControl.WaitAsync(GameState.GameSpeed.DungeonOpenChestSoundTime);

            GiveChestContents(val);
        }

        protected virtual void GiveChestContents(int val)
        {
            // TODO: give weapons
            // TODO: bobby trap chests.

            if (val == 0)
            {
                GiveGold();
            }
            else
            {
                GiveSpecialChestItem(val);
            }

            DungeonAdapter.ClearSpace(Player.X, Player.Y);
        }

        private void GiveGold()
        {
            int amount = Random.Next(90, 300);

            TextArea.PrintLine("You find " + amount + " gold.", XleColor.Yellow);

            Player.Gold += amount;

            StatsDisplay.FlashHPWhileSound(XleColor.Yellow);
        }

        protected virtual void GiveSpecialChestItem(int val)
        {
            int treasure = DungeonAdapter.GetTreasure(val);

            if (treasure > 0)
            {
                string text = "You find a " + Data.ItemList[treasure].LongName + "!!";
                TextArea.Clear();
                TextArea.PrintLine(text);

                Player.Items[treasure] += 1;

                SoundMan.PlaySound(LotaSound.VeryGood);

                TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
                    XleColor.White, XleColor.Yellow, 100);
            }
            else
            {
                TextArea.PrintLine("You find nothing.");
            }
        }
    }
}
