using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Menus;

namespace Xle.Services.Commands.Implementation
{
    public interface ILeave : ICommand
    {
        string PromptText { get; set; }
        bool ConfirmPrompt { get; set; }
    }

    [Transient]
    public class Leave : Command, ILeave
    {
        public IQuickMenu QuickMenu { get; set; }

        public override string Name
        {
            get { return "Leave"; }
        }

        public override async Task Execute()
        {
            if (!await ConfirmLeave())
                return;

            await GameState.MapExtender.LeaveMap();
        }

        /// <summary>
        /// Returns false if the player cancels the leave action.
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> ConfirmLeave()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (ConfirmPrompt)
            {
                if (string.IsNullOrWhiteSpace(PromptText) == false)
                {
                    await TextArea.PrintLine(PromptText);
                    await TextArea.PrintLine();
                }
                if (await QuickMenu.QuickMenuYesNo() == 1)
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
