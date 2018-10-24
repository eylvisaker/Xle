using Xle.Services.Menus;
using Xle.Services.Rendering;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Stores.Extenders
{
    public class StoreFront : StoreExtender
    {
        private ColorScheme mColorScheme;

        public SpriteBatch spriteBatch { get; set; }

        public IQuickMenu QuickMenuService { get; set; }
        public IXleRenderer Renderer { get; set; }
        public IXleInput input { get; set; }
        public INumberPicker NumberPicker { get; set; }
        public ITextAreaRenderer TextAreaRenderer { get; set; }
        public ITextRenderer TextRenderer { get; set; }
        public IXleScreen Screen { get; set; }
        public List<TextWindow> Windows { get; private set; }
        public new Store TheEvent { get { return base.TheEvent; } }

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
            throw new NotImplementedException();
            //Display.BeginFrame();

            //DrawStore();

            //Display.EndFrame();
            //GameControl.KeepAlive();
        }

        protected void DrawStore()
        {
            Renderer.DrawObject(mColorScheme);

            // Draw the title
            DrawTitle(Title);

            foreach (var window in Windows)
            {
                Renderer.DrawObject(spriteBatch, window);
            }

            DrawGoldText();

            TextAreaRenderer.Draw(spriteBatch, TextArea);
        }

        private void DrawGoldText()
        {
            if (ShowGoldText == false)
                return;

            string goldText;
            if (robbing == false)
            {
                // Draw Gold
                goldText = " Gold: ";
                goldText += Player.Gold;
                goldText += " ";
            }
            else
            {
                // don't need gold if we're robbing it!
                goldText = " Robbery in progress ";
            }

            FillRect(
                320 - (goldText.Length / 2) * 16,
                ColorScheme.HorizontalLinePosition * 16,
                goldText.Length * 16,
                14,
                mColorScheme.BackColor);

            TextRenderer.WriteText(spriteBatch, 320 - (goldText.Length / 2) * 16, 18 * 16, goldText, XleColor.White);

        }

        private void FillRect(int v1, int v2, int v3, int v4, Color backColor) => throw new NotImplementedException();

        private void DrawTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return;

            FillRect(320 - (title.Length + 2) / 2 * 16, 0,
                         (title.Length + 2) * 16, 16, mColorScheme.BackColor);

            TextRenderer.WriteText(spriteBatch, 320 - (title.Length / 2) * 16, 0, title, mColorScheme.TitleColor);
        }

        protected Task StoreSound(LotaSound sound)
        {
            throw new NotImplementedException();
            //SoundMan.PlaySoundWait(RedrawStore, sound);
        }

        protected void Wait(int howLong)
        {
            GameControl.Wait(howLong, redraw: RedrawStore);
        }
        protected void WaitForKey(params Keys[] keys)
        {
            throw new NotImplementedException();
            //input.WaitForKey(RedrawStore, keys);
        }

        protected int QuickMenu(MenuItemList menu, int spaces)
        {
            throw new NotImplementedException();
            //return QuickMenuService.QuickMenu(menu, spaces, redraw: RedrawStore);
        }
        protected int QuickMenu(MenuItemList menu, int spaces, int value)
        {
            throw new NotImplementedException();
            //return QuickMenuService.QuickMenu(menu, spaces, value, redraw: RedrawStore);
        }
        protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit)
        {
            throw new NotImplementedException();
            //return QuickMenuService.QuickMenu(menu, spaces, value, clrInit, redraw: RedrawStore);
        }
        protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit, Color clrChanged)
        {
            throw new NotImplementedException();
            //return QuickMenuService.QuickMenu(menu, spaces, value, clrInit, clrChanged, redraw: RedrawStore);
        }

        protected int ChooseNumber(int max)
        {
            EventHandler draw = (sender, args) => { RedrawStore(); };

            try
            {
                //Screen.Draw += draw;

                return NumberPicker.ChooseNumber(RedrawStore, max);
            }
            finally
            {
                Screen.Draw -= draw;
            }
        }

        public string Title { get; set; }

        public override async Task<bool> Speak()
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

                return SpeakImpl();
            }
            finally
            {
                Renderer.ReplacementDrawMethod = null;
            }
        }

        protected override bool SpeakImpl()
        {
            return StoreNotImplementedMessage();
        }

    }
}
