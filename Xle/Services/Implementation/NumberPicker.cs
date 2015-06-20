using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
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
            return ChooseNumber(screen.Redraw, max);
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

            KeyCode key;


            do
            {
                input.PromptToContinueOnWait = false;

                key = input.WaitForKey(redraw);

                if (method == 0)
                {
                    switch (key)
                    {
                        case KeyCode.None:
                            break;

                        case KeyCode.Right:
                        case KeyCode.Up:
                        case KeyCode.Left:
                        case KeyCode.Down:

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
                    if (key >= KeyCode.D0 && key <= KeyCode.D9)
                        amount = 10 * amount + key - KeyCode.D0;

                    if (key == KeyCode.BackSpace)
                        amount /= 10;

                    if (amount > max)
                        amount = max;

                    if (amount < 0)
                        amount = 0;

                    TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }
                else if (method == 2)
                {
                    switch (key)
                    {
                        case KeyCode.Right:
                            amount++;
                            break;
                        case KeyCode.Up:
                            amount += 20;
                            break;
                        case KeyCode.Left:
                            amount--;
                            break;
                        case KeyCode.Down:
                            amount -= 20;
                            break;
                    }

                    if (amount > max)
                        amount = max;

                    if (amount < 0)
                        amount = 0;

                    TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }


            } while (key != KeyCode.Return);

            input.PromptToContinueOnWait = true;
            TextArea.PrintLine();

            return amount;
        }

    }
}
