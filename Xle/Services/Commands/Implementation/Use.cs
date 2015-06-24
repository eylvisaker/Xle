using System.Linq;

using ERY.Xle.Data;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Use : Command
    {
        public Use(bool showItemMenu = true)
        {
            ShowItemMenu = showItemMenu;
        }

        public IXleGameFactory Factory { get; set; }
        public IXleGameControl GameControl { get; set; }
        public IXleRenderer Renderer { get; set; }
        public ISoundMan SoundMan { get; set; }
        public XleData Data { get; set; }
        public IXleSubMenu SubMenu { get; set; }

        public bool ShowItemMenu { get; set; }

        public override void Execute()
        {
            if (ShowItemMenu)
                ChooseHeldItem();
            else
                TextArea.PrintLine();

            string commandstring = string.Empty;
            bool noEffect = true;

            TextArea.PrintLine();

            string action = Data.ItemList[Player.Hold].Action;

            if (string.IsNullOrEmpty(action))
                action = "Use " + Data.ItemList[Player.Hold].Name;

            TextArea.PrintLine(action + ".");

            if (Player.Hold == Factory.HealingItemID)
            {
                noEffect = false;
                UseHealingItem(GameState, Player.Hold);
            }
            else
            {
                noEffect = !GameState.MapExtender.PlayerUse(GameState, Player.Hold);
            }

            if (noEffect == true)
            {
                TextArea.PrintLine();
                GameControl.Wait(400 + 100 * Player.Gamespeed);
                TextArea.PrintLine("No effect");
            }
        }

        public void ChooseHeldItem()
        {
            TextArea.PrintLine("-choose above", XleColor.Cyan);
            MenuItemList theList = new MenuItemList();
            int value = 0;

            theList.Add("Nothing");

            foreach (int i in from kvp in Data.ItemList
                              where Player.Items[kvp.Key] > 0 &&
                              Data.MagicSpells.Values.All(
                                  x => x.ItemID != kvp.Key)
                              select kvp.Key)
            {
                string itemName = Data.ItemList[i].Name;

                if (itemName.Contains("coin"))
                    continue;

                /*
                if (i == 9)			// mail
                {
                    itemName = XleCore.GetMapName(state.Player.mailTown) + " " + itemName;
                }*/

                if (i <= Player.Hold)
                {
                    value++;
                }

                theList.Add(itemName);
            }

            Player.HoldMenu(SubMenu.SubMenu("Hold Item", value, theList));
        }

        private void UseHealingItem(GameState state, int itemID)
        {
            Player.HP += Player.MaxHP / 2;
            Player.Items[itemID] -= 1;
            SoundMan.PlaySound(LotaSound.Good);

            Renderer.FlashHPWhileSound(XleColor.Cyan);
        }

    }
}
