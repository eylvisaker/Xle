using System.Linq;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Commands.Implementation
{
    public abstract class Use : Command
    {
        public Use(bool showItemMenu = true)
        {
            ShowItemMenu = showItemMenu;
        }

        public IXleGameControl GameControl { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }
        public ISoundMan SoundMan { get; set; }
        public XleData Data { get; set; }
        public IXleSubMenu SubMenu { get; set; }

        public bool ShowItemMenu { get; set; }

        MapExtender MapExtender { get { return GameState.MapExtender; } }

        public override string Name
        {
            get { return "Use"; }
        }

        public override void Execute()
        {
            if (ShowItemMenu)
                ChooseHeldItem(TextArea, Data, GameState.Player, SubMenu);
            else
                TextArea.PrintLine();

            TextArea.PrintLine();

            string action = Data.ItemList[Player.Hold].Action;

            if (string.IsNullOrEmpty(action))
                action = "Use " + Data.ItemList[Player.Hold].Name;

            TextArea.PrintLine(action + ".");

            UseItem();
        }

        protected void UseItem()
        {
            if (UseHealingItem(Player.Hold))
                return;

            var effect = UseWithEvent(Player.Hold);

            if (effect)
                return;

            effect = UseWithMap(Player.Hold);

            if (effect)
                return;

            PrintNoEffectMessage();
        }

        protected virtual bool UseWithMap(int item)
        {
            return false;
        }

        public static void ChooseHeldItem(
            ITextArea textArea,
            XleData data,
            Player player,
            IXleSubMenu subMenu
            )
        {
            textArea.PrintLine("-choose above", XleColor.Cyan);
            MenuItemList theList = new MenuItemList();
            int value = 0;

            theList.Add("Nothing");

            foreach (int i in from kvp in data.ItemList
                              where player.Items[kvp.Key] > 0 &&
                              data.MagicSpells.Values.All(
                                  x => x.ItemID != kvp.Key)
                              select kvp.Key)
            {
                string itemName = data.ItemList[i].Name;

                if (itemName.Contains("coin"))
                    continue;

                /*
                if (i == 9)			// mail
                {
                    itemName = XleCore.GetMapName(state.Player.mailTown) + " " + itemName;
                }*/

                if (i <= player.Hold)
                {
                    value++;
                }

                theList.Add(itemName);
            }

            player.HoldMenu(subMenu.SubMenu("Hold Item", value, theList));
        }

        protected abstract bool UseHealingItem(int itemID);

        protected void ApplyHealingEffect()
        {
            Player.HP += Player.MaxHP / 2;
            SoundMan.PlaySound(LotaSound.Good);

            StatsDisplay.FlashHPWhileSound(XleColor.Cyan);
        }

        protected void PrintNoEffectMessage()
        {
            TextArea.PrintLine();
            GameControl.Wait(400 + 100 * Player.Gamespeed);
            TextArea.PrintLine("No effect");
        }


        /// <summary>
        /// Returns true if there was an effect of using the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        protected bool UseWithEvent(int item)
        {
            foreach (var evt in MapExtender.EventsAt(1))
            {
                if (evt.Use(item))
                    return true;
            }

            return false;
        }
    }
}
