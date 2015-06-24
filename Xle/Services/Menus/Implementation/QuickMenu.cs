using System;

using AgateLib.Geometry;
using AgateLib.InputLib;

using ERY.Xle.Services.Game;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Menus.Implementation
{
    public class QuickMenuRunner : IQuickMenu
    {
        private IXleScreen screen;
        private ITextArea TextArea;
        private IXleInput input;
        private IXleGameControl gameControl;

        public QuickMenuRunner(
            IXleScreen screen,
            ITextArea textArea,
            IXleInput input,
            IXleGameControl gameControl)
        {
            this.screen = screen;
            this.TextArea = textArea;
            this.input = input;
            this.gameControl = gameControl;
        }

        /// <summary>
        /// Gives the player a yes/no choice, returning 0 if the player chose yes and
        /// 1 if the player chose no.
        /// </summary>
        /// <param name="defaultAtNo">Pass true to have the cursor start at no.</param>
        /// <returns>Returns 0 if the player chose yes, 1 if the player chose no.</returns>
        public int QuickMenuYesNo(bool defaultAtNo = false)
        {
            return QuickMenu(new MenuItemList("Yes", "No"), 3, defaultAtNo ? 1 : 0);
        }
        /// <summary>
        /// This function creates a quick menu at the bottow of the screen,
        /// allowing the player to pick from a few choices.	
        /// </summary>
        /// <param name="items">The items in the list.</param>
        /// <param name="spaces"></param>
        /// <returns></returns>
        public int QuickMenu(MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null, Action redraw = null)
        {
            return QuickMenu(redraw ?? screen.OnDraw, items, spaces, value, clrInit, clrChanged);
        }

        public int QuickMenu(Action redraw, MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null)
        {
            return QuickMenuImpl(redraw, items, spaces, value, clrInit ?? screen.FontColor, clrChanged ?? screen.FontColor);
        }

        public int QuickMenuImpl(Action redraw, MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
        {
            int[] spacing = new int[items.Count];
            int last = 0;
            string tempLine = "Choose: ";
            string topLine;
            string bulletLine;
            int lineIndex = TextArea.CursorLocation.Y;
            Color[] colors = new Color[40];

            if (lineIndex >= 4)
                lineIndex = 3;

            System.Diagnostics.Debug.Assert(value >= 0);
            System.Diagnostics.Debug.Assert(value < items.Count);

            if (value < 0)
                value = 0;

            for (int i = 0; i < 40; i++)
                colors[i] = clrChanged;


            spacing[0] = 8;

            // Construct the temporary line
            for (int i = 0; i < items.Count; i++)
            {
                bulletLine = items[i];

                tempLine += bulletLine + new string(' ', spaces);

                spacing[i] += last + bulletLine.Length - 1;
                last = spacing[i] + spaces + 1;
            }

            TextArea.PrintLine(tempLine, clrInit);
            TextArea.PrintLine();

            topLine = tempLine;
            tempLine = new string(' ', spacing[value]) + "`";

            TextArea.RewriteLine(lineIndex + 1, tempLine, clrInit);
            input.PromptToContinueOnWait = false;

            KeyCode key;

            do
            {
                key = input.WaitForKey(redraw);

                if (key == KeyCode.Left)
                {
                    value--;
                    if (value < 0)
                        value = 0;
                }
                if (key == KeyCode.Right)
                {
                    value++;
                    if (value >= items.Count)
                        value = items.Count - 1;
                }
                else if (key >= KeyCode.D0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        bulletLine = items[i];

                        if (key - KeyCode.A ==
                            char.ToUpperInvariant(bulletLine[0]) - 'A')
                        {
                            value = i;
                            key = KeyCode.Return;
                        }
                    }
                }

                tempLine = new string(' ', spacing[value]) + "`";

                if (key != KeyCode.None)
                {
                    TextArea.RewriteLine(lineIndex, topLine, clrChanged);
                    TextArea.RewriteLine(lineIndex + 1, tempLine, clrChanged);
                }


            } while (key != KeyCode.Return && screen.CurrentWindowClosed == false);

            gameControl.Wait(100, redraw: redraw);

            TextArea.PrintLine();

            return value;

        }

    }
}
