using AgateLib.Quality;
using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using AgateLib;
using System.Threading.Tasks;

namespace Xle.Services.Menus
{
    public interface IQuickMenu
    {
        Task<int> QuickMenuYesNo(bool defaultAtNo = false);

        Task<int> QuickMenu(MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null);
    }

    [Singleton]
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
        public Task<int> QuickMenuYesNo(bool defaultAtNo = false)
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
        public Task<int> QuickMenu(MenuItemList items, int spaces, int value = 0, Color? clrInit = null, Color? clrChanged = null)
        {
            return QuickMenuCore(items, spaces, value, clrInit ?? screen.FontColor, clrChanged ?? screen.FontColor);
        }

        public async Task<int> QuickMenuCore(MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
        {
            Require.That<ArgumentOutOfRangeException>(value >= 0, "value should be positive");
            Require.That<ArgumentOutOfRangeException>(value < items.Count, "value should be less than items.Count");

            int result = value;
            string topLine;
            string bulletLine;
            int lineIndex = TextArea.CursorLocation.Y;
            Color[] colors = new Color[40];

            if (lineIndex >= 4)
                lineIndex = 3;

            for (int i = 0; i < 40; i++)
                colors[i] = clrChanged;

            int[] spacing = new int[items.Count];
            int last = 0;

            spacing[0] = 8;

            // Construct the temporary line
            string tempLine = "Choose: ";
            for (int i = 0; i < items.Count; i++)
            {
                bulletLine = items[i];

                tempLine += bulletLine + new string(' ', spaces);

                spacing[i] += last + bulletLine.Length - 1;
                last = spacing[i] + spaces + 1;
            }

            await TextArea.PrintLine(tempLine, clrInit);
            await TextArea.PrintLine();

            topLine = tempLine;
            tempLine = new string(' ', spacing[result]) + "`";

            await TextArea.RewriteLine(lineIndex + 1, tempLine, clrInit);

            Keys key;

            do
            {
                // Set this on each iteration because it gets reset after a key is pressed.
                input.PromptToContinueOnWait = false;

                key = await input.WaitForKey();

                if (key == Keys.Left)
                {
                    result--;
                    if (result < 0)
                        result = 0;
                }
                if (key == Keys.Right)
                {
                    result++;
                    if (result >= items.Count)
                        result = items.Count - 1;
                }
                else if (key >= Keys.D0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        bulletLine = items[i];

                        if (key - Keys.A ==
                            char.ToUpperInvariant(bulletLine[0]) - 'A')
                        {
                            result = i;
                            key = Keys.Enter;
                        }
                    }
                }

                tempLine = new string(' ', spacing[result]) + "`";

                if (key != Keys.None)
                {
                    await TextArea.RewriteLine(lineIndex, topLine, clrChanged);
                    await TextArea.RewriteLine(lineIndex + 1, tempLine, clrChanged);
                }

            } while (key != Keys.Enter);

            await gameControl.WaitAsync(100);

            await TextArea.PrintLine();

            input.PromptToContinueOnWait = true;

            return result;
        }

    }
}
