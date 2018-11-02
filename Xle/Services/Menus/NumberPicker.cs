using AgateLib;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Services.Menus
{
    public interface INumberPicker
    {
        Task<int> ChooseNumber(int max);
    }

    [Singleton]
    public class NumberPicker : INumberPicker
    {
        private IXleScreen screen;
        private IXleGameControl gameControl;
        private ITextArea TextArea;

        public NumberPicker(
            IXleScreen screen,
            IXleGameControl gameControl,
            ITextArea textArea)
        {
            this.screen = screen;
            this.gameControl = gameControl;
            this.TextArea = textArea;
        }

        /// <summary>
        /// Asks the user to choose a number.
        /// </summary>
        /// <param name="max">The maximum value the user is allowed to select.</param>
        /// <returns></returns>
        public async Task<int> ChooseNumber(int max)
        {
            int method = 0;
            int amount = 0;

            await TextArea.PrintLine();

            await TextArea.Print("Enter number by ", XleColor.White);
            await TextArea.Print("keyboard", XleColor.Yellow);
            await TextArea.Print(" or ", XleColor.White);
            await TextArea.Print("joystick", XleColor.Cyan);

            await TextArea.PrintLine();
            await TextArea.PrintLine();

            Keys key;

            do
            {
                key = await gameControl.WaitForKey(showPrompt: false);

                if (method == 0)
                {
                    switch (key)
                    {
                        case Keys.None:
                            break;

                        case Keys.Right:
                        case Keys.Up:
                        case Keys.Left:
                        case Keys.Down:

                            await TextArea.PrintLine("Use joystick - press button when done");
                            await TextArea.PrintLine();
                            await TextArea.PrintLine("  Horizontal - Slow change", XleColor.Cyan);
                            await TextArea.PrintLine("  Vertical   - Fast change", XleColor.Cyan);
                            await TextArea.PrintLine("                          - 0 -");

                            method = 2;

                            break;
                        default:
                            await TextArea.PrintLine("Keyboard entry-press return when done", XleColor.Yellow);
                            await TextArea.PrintLine();
                            await TextArea.PrintLine();
                            await TextArea.PrintLine("                          - 0 -");
                            method = 1;

                            break;
                    }

                }

                if (method == 1)
                {
                    if (key >= Keys.D0 && key <= Keys.D9)
                        amount = 10 * amount + key - Keys.D0;

                    if (key == Keys.Back)
                        amount /= 10;

                    amount = Math.Min(amount, max);
                    amount = Math.Max(amount, 0);

                    await TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }
                else if (method == 2)
                {
                    switch (key)
                    {
                        case Keys.Right:
                            amount++;
                            break;
                        case Keys.Up:
                            amount += 20;
                            break;
                        case Keys.Left:
                            amount--;
                            break;
                        case Keys.Down:
                            amount -= 20;
                            break;
                    }

                    if (amount > max)
                        amount = max;

                    if (amount < 0)
                        amount = 0;

                    await TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }


            } while (key != Keys.Enter);

            await TextArea.PrintLine();

            return amount;
        }

    }
}
