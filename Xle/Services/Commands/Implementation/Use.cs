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

        public IEventInteractor EventInteractor { get; set; }
        public IItemChooser ItemChooser { get; set; }
        public IXleGameControl GameControl { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }
        public ISoundMan SoundMan { get; set; }
        public XleData Data { get; set; }
        public IXleSubMenu SubMenu { get; set; }

        public bool ShowItemMenu { get; set; }

        IMapExtender MapExtender { get { return GameState.MapExtender; } }

        public override string Name
        {
            get { return "Use"; }
        }

        public override void Execute()
        {
            if (ShowItemMenu)
                Player.Hold = ItemChooser.ChooseItem();
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
            return EventInteractor.InteractWithFirstEvent(evt => evt.Use(item));

        }
    }
}
