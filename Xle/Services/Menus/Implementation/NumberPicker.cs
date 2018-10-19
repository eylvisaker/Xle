using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework.Input;
using System;

namespace Xle.Services.Menus.Implementation
{
    public class NumberPicker : INumberPicker
    {
        private IXleScreen screen;
        private IXleInput input;
        private ITextArea TextArea;

        public NumberPicker(
            IXleScreen screen,
            IXleInput input,
            ITextArea textArea)
        {
            this.screen = screen;
            this.input = input;
            this.TextArea = textArea;
        }

        /// <summary>
        /// Asks the user to choose a number.
        /// </summary>
        /// <param name="max">The maximum value the user is allowed to select.</param>
        /// <returns></returns>
        public int ChooseNumber(int max)
        {
            return ChooseNumber(screen.OnDraw, max);
        }
        public int ChooseNumber(Action redraw, int max)
        {
            int method = 0;
            int amount = 0;

            TextArea.PrintLine();

            TextArea.Print("Enter number by ", XleColor.White);
            TextArea.Print("keyboard", XleColor.Yellow);
            TextArea.Print(" or ", XleColor.White);
            TextArea.Print("joystick", XleColor.Cyan);

            TextArea.PrintLine();
            TextArea.PrintLine();

            Keys key;


            do
            {
                input.PromptToContinueOnWait = false;

                key = input.WaitForKey(redraw);

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

                            TextArea.PrintLine("Use joystick - press button when done");
                            TextArea.PrintLine();
                            TextArea.PrintLine("  Horizontal - Slow change", XleColor.Cyan);
                            TextArea.PrintLine("  Vertical   - Fast change", XleColor.Cyan);
                            TextArea.PrintLine("                          - 0 -");

                            method = 2;

                            break;
                        default:
                            TextArea.PrintLine("Keyboard entry-press return when done", XleColor.Yellow);
                            TextArea.PrintLine();
                            TextArea.PrintLine();
                            TextArea.PrintLine("                          - 0 -");
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

                    TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
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

                    TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }


            } while (key != Keys.Enter);

            input.PromptToContinueOnWait = true;
            TextArea.PrintLine();

            return amount;
        }

    }
}
