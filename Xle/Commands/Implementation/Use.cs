using System.Linq;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps;
using Xle.Game;
using Xle.Menus;
using Xle.Rendering;
using Xle.ScreenModel;
using Xle.XleSystem;

namespace Xle.Commands.Implementation
{
    public interface IUse : ICommand{ }

    public abstract class Use : Command, IUse
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

        public override async Task Execute()
        {
            if (ShowItemMenu)
                Player.Hold = await ItemChooser.ChooseItem();
            else
                await TextArea.PrintLine();

            await TextArea.PrintLine();

            string action = Data.ItemList[Player.Hold].Action;

            if (string.IsNullOrEmpty(action))
                action = "Use " + Data.ItemList[Player.Hold].Name;

            await TextArea.PrintLine(action + ".");

            await UseItem();
        }

        protected async Task UseItem()
        {
            if (await UseHealingItem(Player.Hold))
                return;

            var effect = await UseWithEvent(Player.Hold);

            if (effect)
                return;

            effect = await UseWithMap(Player.Hold);

            if (effect)
                return;

            await PrintNoEffectMessage();
        }

        protected virtual async Task<bool> UseWithMap(int item)
        {
            return false;
        }

        protected abstract Task<bool> UseHealingItem(int itemID);

        protected async Task ApplyHealingEffect()
        {
            Player.HP += Player.MaxHP / 2;
            SoundMan.PlaySound(LotaSound.Good);

            await GameControl.FlashHPWhileSound(XleColor.Cyan);
        }

        protected async Task PrintNoEffectMessage()
        {
            await TextArea.PrintLine();
            await GameControl.WaitAsync(400 + 100 * Player.Gamespeed);
            await TextArea.PrintLine("No effect");
        }

        /// <summary>
        /// Returns true if there was an effect of using the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        protected Task<bool> UseWithEvent(int item)
        {
            return EventInteractor.InteractWithFirstEvent(async evt => await evt.Use(item));
        }
    }
}
