using System;

using AgateLib.Mathematics.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Maps.Dungeons.Commands
{
    [ServiceName("DungeonOpen")]
    public class DungeonOpenCommand : Open
    {
        public ISoundMan SoundMan { get; set; }
        public Random Random { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }
        public XleData Data { get; set; }

        public IDungeonAdapter DungeonAdapter { get; set; }

        public override void Execute()
        {
            DungeonTile tile = DungeonAdapter.TileAt(Player.X, Player.Y);

            if (tile == DungeonTile.Urn)
            {
                OpenUrn();
            }
            else if (tile == DungeonTile.Box)
            {
                OpenBox();
            }
            else if (tile == DungeonTile.Chest)
            {
                OpenChest(DungeonAdapter.ChestValueAt(Player.X, Player.Y));
            }
            else
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("Nothing to open.");
                GameControl.Wait(1000);
            }
        }

        private void OpenUrn()
        {
            TextArea.PrintLine(" Urn");
            TextArea.PrintLine();
            SoundMan.PlaySound(LotaSound.OpenChest);
            GameControl.Wait(500);

            GiveUrnContents();

            SoundMan.FinishSounds();
            DungeonAdapter.ClearSpace(Player.X, Player.Y);
        }

        private void OpenBox()
        {
            TextArea.PrintLine(" Box");
            TextArea.PrintLine();
            SoundMan.PlaySound(LotaSound.OpenChest);
            GameControl.Wait(500);

            GiveBoxContents();

            SoundMan.FinishSounds();

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

        private void OpenChest(int val)
        {
            TextArea.PrintLine(" Chest");
            TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.OpenChest);
            GameControl.Wait(GameState.GameSpeed.DungeonOpenChestSoundTime);

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
