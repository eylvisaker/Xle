using ERY.Xle.Data;
using ERY.Xle.Rendering;
using System.Linq;

namespace ERY.Xle.Services.Implementation.Commands
{
    public class Use : Command
    {
        public Use(bool showItemMenu = true)
        {
            ShowItemMenu = showItemMenu;
        }

        public IXleGameControl GameControl { get; set; }
        public IXleRenderer Renderer { get; set; }

        public XleData Data { get; set; }
        public IXleMenu Menu { get; set; }

        public bool ShowItemMenu { get; set; }

        public override void Execute(GameState state)
        {
            if (ShowItemMenu)
                ChooseHeldItem(state);
            else
                TextArea.PrintLine();

            string commandstring = string.Empty;
            bool noEffect = true;

            TextArea.PrintLine();

            string action = Data.ItemList[state.Player.Hold].Action;

            if (string.IsNullOrEmpty(action))
                action = "Use " + Data.ItemList[state.Player.Hold].Name;

            TextArea.PrintLine(action + ".");

            if (state.Player.Hold == XleCore.Factory.HealingItemID)
            {
                noEffect = false;
                UseHealingItem(state, state.Player.Hold);
            }
            else
            {
                noEffect = !state.MapExtender.PlayerUse(state, state.Player.Hold);
            }

            if (noEffect == true)
            {
                TextArea.PrintLine();
                GameControl.Wait(400 + 100 * state.Player.Gamespeed);
                TextArea.PrintLine("No effect");
            }
        }

        public void ChooseHeldItem(GameState state)
        {
            TextArea.PrintLine("-choose above", XleColor.Cyan);
            MenuItemList theList = new MenuItemList();
            int value = 0;

            theList.Add("Nothing");

            foreach (int i in from kvp in Data.ItemList
                              where state.Player.Items[kvp.Key] > 0 &&
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

                if (i <= state.Player.Hold)
                {
                    value++;
                }

                theList.Add(itemName);
            }

            state.Player.HoldMenu(Menu.SubMenu("Hold Item", value, theList));
        }

        private void UseHealingItem(GameState state, int itemID)
        {
            state.Player.HP += state.Player.MaxHP / 2;
            state.Player.Items[itemID] -= 1;
            SoundMan.PlaySound(LotaSound.Good);

            Renderer.FlashHPWhileSound(XleColor.Cyan);
        }

    }
}
