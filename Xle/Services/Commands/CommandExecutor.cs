using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Services.Commands
{
    public interface ICommandExecutor
    {
        Task Prompt();

        void ResetCurrentCommand();

        void Update(GameTime time);
    }

    [Singleton]
    public class CommandExecutor : ICommandExecutor
    {
        private GameState gameState;
        private Dictionary<Keys, Direction> mDirectionMap = new Dictionary<Keys, Direction>();
        private IXleGameControl gameControl;
        private readonly IXleInput input;
        private ITextArea textArea;
        private ISoundMan soundMan;
        private ICommandList commands;
        private IPlayerDeathHandler deathHandler;
        private IPlayerAnimator playerAnimator;
        private bool inputPrompt;
        private Task commandTask;
        private Keys lastInput;
        private object syncRoot = new object();

        private Player player { get { return gameState.Player; } }

        public CommandExecutor(
            GameState state,
            ICommandList commands,
            IXleGameControl gameControl,
            IXleInput input,
            ISoundMan soundMan,
            IPlayerDeathHandler deathHandler,
            IPlayerAnimator characterAnimator,
            ITextArea textArea)
        {
            gameState = state;
            this.commands = commands;
            this.gameControl = gameControl;
            this.input = input;
            this.textArea = textArea;
            this.soundMan = soundMan;
            this.playerAnimator = characterAnimator;
            this.deathHandler = deathHandler;

            input.DoCommand += (sender, args) =>
            {
                DoCommand(args.Command, args.KeyString);
            };

            mDirectionMap[Keys.Right] = Direction.East;
            mDirectionMap[Keys.Up] = Direction.North;
            mDirectionMap[Keys.Left] = Direction.West;
            mDirectionMap[Keys.Down] = Direction.South;

            mDirectionMap[Keys.OemOpenBrackets] = Direction.North;
            mDirectionMap[Keys.OemSemicolon] = Direction.West;
            mDirectionMap[Keys.OemQuotes] = Direction.East;
            mDirectionMap[Keys.OemQuestion] = Direction.South;
        }

        public async Task Prompt()
        {
            if (player.HP <= 0 || player.Food <= 0)
            {
                await deathHandler.PlayerIsDead();
            }

            await textArea.Print("\nEnter command: ");

            lock (syncRoot)
            {
                inputPrompt = true;
                commandTask = null;
            }
        }

        /// <summary>
        /// Returns true if the command is a cursor movement.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task CursorMovement(Keys cmd)
        {
            Direction dir = mDirectionMap[cmd];

            await gameState.MapExtender.PlayerCursorMovement(dir);
        }

        private bool IsCursorMovement(Keys cmd)
        {
            switch (cmd)
            {
                case Keys.Right:
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                    return true;

                default:
                    return false;
            }
        }

        public void DoCommand(Keys cmd, string keyString)
        {
            if (!inputPrompt)
            {
                return;
            }

            inputPrompt = false;
            commandTask = ProcessInput(cmd, keyString);
        }

        private async Task ProcessInput(Keys cmd, string keyString)
        {
            if (cmd == Keys.None)
                return;

            lastInput = cmd;

            int waitTime = 700;

            if (IsCursorMovement(cmd))
            {
                await ExecuteCursorMovement(cmd);
                return;
            }

            var command = commands.FindCommand(keyString);

            if (command != null)
            {
                commands.CurrentCommand = command;

                await textArea.Print(command.Name);

                await command.Execute();
            }
            else
            {
                soundMan.PlaySound(LotaSound.Invalid);

                await gameControl.WaitAsync(waitTime);
                inputPrompt = true;
                return;
            }

            await AfterDoCommand(waitTime, cmd);
        }

        private async Task ExecuteCursorMovement(Keys cmd)
        {
            await CursorMovement(cmd);

            playerAnimator.AnimateStep();

            var waitTime = gameState.MapExtender.WaitTimeAfterStep;
            
            await AfterDoCommand(waitTime, cmd);
        }

        public void ResetCurrentCommand()
        {
            commands.Items.Sort((x, y) => x.Name.CompareTo(y.Name));
            commands.CurrentCommand = commands.Items.Find(x => x is Pass);

            if (commands.CurrentCommand == null)
                commands.CurrentCommand = commands.Items[0];
        }

        private async Task AfterDoCommand(int waitTime, Keys cmd)
        {
            await gameState.MapExtender.AfterExecuteCommand(cmd);

            await gameControl.WaitAsync(waitTime);

            await Prompt();
        }

        public void Update(GameTime time)
        {
            if (commandTask?.IsFaulted ?? false)
            {
                commandTask = OutputException(commandTask.Exception);
            }

            input.Update(time);
        }

        private async Task OutputException(Exception exception)
        {
            if (exception is AggregateException agg && agg.InnerExceptions.Count == 1)
            {
                await OutputException(agg.InnerException);
                return;
            }

            try
            {
                inputPrompt = false;

                await textArea.PrintLine();
                await textArea.PrintLine(WrapText($"Error: {exception.GetType()}"), Color.Red);
                await textArea.PrintLine(WrapText(exception.Message), Color.Red);

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            catch (Exception exx)
            {
                int j = 4;
            }
            finally
            {
                await Prompt();
            }
        }

        private string WrapText(string message)
        {
            StringBuilder result = new StringBuilder();

            int index = 0;

            while (index < message.Length)
            {
                if (index > 0)
                    result.AppendLine();

                int length = Math.Min(message.Length - index, 36);

                result.Append(message.Substring(0, length));
                index += length;
            }

            return result.ToString();
        }
    }
}