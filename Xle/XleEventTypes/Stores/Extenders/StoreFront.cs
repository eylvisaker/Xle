using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
    public class StoreFront : StoreExtender
    {
        ColorScheme mColorScheme;

        public IQuickMenu QuickMenuService { get; set; }
        public IXleRenderer Renderer { get; set; }
        public IXleInput input { get; set; }
        public INumberPicker NumberPicker { get; set; }
        public ITextAreaRenderer TextAreaRenderer { get; set; }
        public ITextRenderer TextRenderer { get; set; }

        public List<TextWindow> Windows { get; private set; }
        public new Store TheEvent { get { return (Store)base.TheEvent; } }

        protected ColorScheme ColorScheme { get { return mColorScheme; } }
        protected bool ShowGoldText { get; set; }

        public StoreFront()
        {
            PrivateInitializeColorScheme();

            Windows = new List<TextWindow>();

            ClearWindow();

        }

        private void PrivateInitializeColorScheme()
        {
            mColorScheme = new Xle.ColorScheme();
            mColorScheme.BackColor = XleColor.Green;

            ShowGoldText = true;

            InitializeColorScheme(mColorScheme);
        }

        protected void ClearWindow()
        {
            Windows.Clear();
        }

        protected virtual void InitializeColorScheme(ColorScheme cs)
        { }

        protected internal void RedrawStore()
        {
            Display.BeginFrame();

            DrawStore();

            Display.EndFrame();
            GameControl.KeepAlive();
        }

        protected void DrawStore()
        {
            Renderer.DrawObject(mColorScheme);

            // Draw the title
            DrawTitle(Title);

            foreach (var window in Windows)
            {
                Renderer.DrawObject(window);
            }

            DrawGoldText(Player);

            TextAreaRenderer.Draw(TextArea);
        }

        private void DrawGoldText(Player player)
        {
            if (ShowGoldText == false)
                return;

            string goldText;
            if (robbing == false)
            {
                // Draw Gold
                goldText = " Gold: ";
                goldText += player.Gold;
                goldText += " ";
            }
            else
            {
                // don't need gold if we're robbing it!
                goldText = " Robbery in progress ";
            }

            Display.FillRect(
                320 - (goldText.Length / 2) * 16,
                ColorScheme.HorizontalLinePosition * 16,
                goldText.Length * 16,
                14,
                mColorScheme.BackColor);

            TextRenderer.WriteText(320 - (goldText.Length / 2) * 16, 18 * 16, goldText, XleColor.White);

        }

        private void DrawTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return;

            Display.FillRect(320 - (title.Length + 2) / 2 * 16, 0,
                         (title.Length + 2) * 16, 16, mColorScheme.BackColor);

            TextRenderer.WriteText(320 - (title.Length / 2) * 16, 0, title, mColorScheme.TitleColor);
        }

        protected void StoreSound(LotaSound sound)
        {
            SoundMan.PlaySoundSync(RedrawStore, sound);
        }

        protected void Wait(int howLong)
        {
            GameControl.Wait(howLong, redraw: RedrawStore);
        }
        protected void WaitForKey(params KeyCode[] keys)
        {
            input.WaitForKey(RedrawStore, keys);
        }

        protected int QuickMenu(MenuItemList menu, int spaces)
        {
            return QuickMenuService.QuickMenu(menu, spaces, redraw: RedrawStore);
        }
        protected int QuickMenu(MenuItemList menu, int spaces, int value)
        {
            return QuickMenuService.QuickMenu(menu, spaces, value, redraw: RedrawStore);
        }
        protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit)
        {
            return QuickMenuService.QuickMenu(menu, spaces, value, clrInit, redraw: RedrawStore);
        }
        protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit, Color clrChanged)
        {
            return QuickMenuService.QuickMenu(menu, spaces, value, clrInit, clrChanged, redraw: RedrawStore);
        }

        protected int ChooseNumber(int max)
        {
            return NumberPicker.ChooseNumber(RedrawStore, max);
        }

        public string Title { get; set; }

        public override bool Speak(GameState state)
        {
            PrivateInitializeColorScheme();

            if (AllowInteractionWhenLoanOverdue == false)
            {
                if (IsLoanOverdue())
                {
                    StoreDeclinePlayer();
                    return true;
                }
            }

            try
            {
                Renderer.ReplacementDrawMethod = DrawStore;

                return SpeakImpl(GameState);
            }
            finally
            {
                Renderer.ReplacementDrawMethod = null;
            }
        }

        protected override bool SpeakImpl(GameState state)
        {
            return StoreNotImplementedMessage();
        }

    }
}
