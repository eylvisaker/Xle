using ERY.Xle.Services.Menus;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class Leave : Command
    {
        private IQuickMenu menu;

        public Leave(
            IQuickMenu menu,
            string promptText,
            bool confirmPrompt = true)
        {
            ConfirmPrompt = confirmPrompt;
            PromptText = promptText;

            this.menu = menu;
        }

        public override void Execute()
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
                if (menu.QuickMenuYesNo() == 1)
                    return;
            }

            GameState.MapExtender.PlayerLeave(GameState);
        }

        public bool ConfirmPrompt { get; set; }

        public string PromptText { get; set; }
    }
}
