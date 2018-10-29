using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xle.Services.Menus;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public class StoreFront : StoreExtender
    {
        private StoreFrontScreen storeFrontScreen = new StoreFrontScreen();
        private StoreFrontRenderer renderer;

        public SpriteBatch spriteBatch { get; set; }

        public StoreFrontRenderer Renderer
        {
            get => renderer;
            set
            {
                renderer = value;
                renderer.Screen = storeFrontScreen;
            }
        }

        public string Title
        {
            get => storeFrontScreen.Title;
            set => storeFrontScreen.Title = value;
        }

        public IQuickMenu QuickMenuService { get; set; }
        public IXleInput input { get; set; }
        public INumberPicker NumberPicker { get; set; }
        public IXleScreen Screen { get; set; }

        public new Store TheEvent { get { return base.TheEvent; } }


        public List<TextWindow> Windows => storeFrontScreen.Windows;
        protected ColorScheme ColorScheme => storeFrontScreen.ColorScheme;

        protected bool ShowGoldText
        {
            get => Renderer.Screen.ShowGoldText;
            set => Renderer.Screen.ShowGoldText = value;
        }

        public StoreFront()
        {
            PrivateInitializeColorScheme();

            ClearWindow();
        }

        private void PrivateInitializeColorScheme()
        {
            InitializeColorScheme(storeFrontScreen.ColorScheme);
        }

        protected void ClearWindow()
        {
            Windows.Clear();
        }

        protected virtual void InitializeColorScheme(ColorScheme cs)
        { }

        protected Task<int> QuickMenu(MenuItemList menu, int spaces)
        {
            return QuickMenuService.QuickMenu(menu, spaces);
        }
        protected Task<int> QuickMenu(MenuItemList menu, int spaces, int value)
        {
            return QuickMenuService.QuickMenu(menu, spaces, value);
        }
        protected Task<int> QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit)
        {
            return QuickMenuService.QuickMenu(menu, spaces, value, clrInit);
        }
        protected Task<int> QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit, Color clrChanged)
        {
            return QuickMenuService.QuickMenu(menu, spaces, value, clrInit, clrChanged);
        }

        protected Task<int> ChooseNumber(int max) => NumberPicker.ChooseNumber(max);

        public override async Task<bool> Speak()
        {
            PrivateInitializeColorScheme();

            if (AllowInteractionWhenLoanOverdue == false)
            {
                if (IsLoanOverdue())
                {
                    await StoreDeclinePlayer();
                    return true;
                }
            }

            using (GameControl.PushRenderer(Renderer))
            {
                return await SpeakImplAsync();
            }
        }

        protected override Task<bool> SpeakImplAsync()
        {
            return StoreNotImplementedMessage();
        }

    }


    public class StoreFrontScreen
    {
        public StoreFrontScreen()
        {
            ColorScheme = new Xle.ColorScheme();
            ColorScheme.BackColor = XleColor.Green;

            ShowGoldText = true;
        }

        public ColorScheme ColorScheme { get; set; } = new ColorScheme();

        public bool Robbing { get; set; }

        public bool ShowGoldText { get; set; }

        public string Title { get; set; }

        public List<TextWindow> Windows { get; private set; } = new List<TextWindow>();
    }
}
