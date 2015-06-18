namespace ERY.Xle.Services.Implementation.Commands
{
    public class Leave : Command
    {
        public Leave(bool confirmPrompt = true)
        {
            ConfirmPrompt = confirmPrompt;
        }

        public override void Execute(GameState state)
        {
            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine();

            if (ConfirmPrompt)
            {
                if (string.IsNullOrWhiteSpace(PromptText) == false)
                {
                    XleCore.TextArea.PrintLine(PromptText);
                    XleCore.TextArea.PrintLine();
                }
                if (XleCore.QuickMenuYesNo() == 1)
                    return;
            }

            state.MapExtender.PlayerLeave(state);
        }

        public bool ConfirmPrompt { get; set; }

        public string PromptText { get; set; }
    }
}
