using System;

using AgateLib.Geometry;

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
        Maps.XleMapTypes.Dungeon TheMap { get { return (Maps.XleMapTypes.Dungeon)GameState.Map; } }
        DungeonExtender Map { get { return (DungeonExtender)GameState.MapExtender; } }

        public ISoundMan SoundMan { get; set; }
        public Random Random { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }
        public XleData Data { get; set; }

        public override void Execute()
        {
            int val = TheMap[Player.X, Player.Y];

            if (val == 0x1d)
            {
                OpenUrn();
            }
            else if (val == 0x1e)
            {
                OpenBox();
            }
            else if (val >= 0x30 && val <= 0x3f)
            {
                OpenChest(val);
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
            ClearSpace();
        }

        private void ClearSpace()
        {
            TheMap[Player.X, Player.Y] = 0x10;
        }

        private void OpenBox()
        {
            TextArea.PrintLine(" Box");
            TextArea.PrintLine();
            SoundMan.PlaySound(LotaSound.OpenChest);
            GameControl.Wait(500);

            GiveBoxContents();

            SoundMan.FinishSounds();

            ClearSpace();
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

            if (amount + Player.HP > Player.MaxHP)
            {
                amount = Player.MaxHP - Player.HP;
                if (amount < 0)
                {
                    amount = 0;
                }
            }

            if (amount == 0)
            {
                TextArea.PrintLine("You find nothing.", Color.Yellow);
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
            val -= 0x30;

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

            ClearSpace();
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
            int treasure = GetTreasure(val);

            bool handled = false;

            // Used by LOB only.
            //OnBeforeGiveItem(ref treasure, ref handled, ref clearBox);

            if (handled == false)
            {
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

        private int GetTreasure(int val)
        {
            return Map.GetTreasure(GameState, Player.DungeonLevel + 1, val);
        }
    }
}
