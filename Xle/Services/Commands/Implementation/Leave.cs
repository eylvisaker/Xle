using AgateLib;
using Xle.Services.Menus;

namespace Xle.Services.Commands.Implementation
{
    [Transient]
    public class Leave : Command
    {
        public Leave(
            string promptText,
            bool confirmPrompt = true)
        {
            ConfirmPrompt = confirmPrompt;
            PromptText = promptText;
        }

        public IQuickMenu QuickMenu { get; set; }

        public override string Name
        {
            get { return "Leave"; }
        }

        public override void Execute()
        {
            if (!ConfirmLeave())
                return;

            GameState.MapExtender.LeaveMap();
        }

        /// <summary>
        /// Returns false if the player cancels the leave action.
        /// </summary>
        /// <returns></returns>
        protected bool ConfirmLeave()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (ConfirmPrompt)
            {
                if (string.IsNullOrWhiteSpace(PromptText) == false)
                {
                    TextArea.PrintLine(PromptText);
                    TextArea.PrintLine();
                }
                if (QuickMenu.QuickMenuYesNo() == 1)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ConfirmPrompt { get; set; }

        public string PromptText { get; set; }
    }
}
