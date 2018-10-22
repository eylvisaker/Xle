using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using AgateLib;
using Xle.Services.Commands.Implementation;

namespace Xle.Services.Commands
{
    public interface ICommandExecutor
    {
        void Prompt();

        void DoCommand(Keys Keys);

        void ResetCurrentCommand();
    }

    [Singleton]
    public class CommandExecutor : ICommandExecutor
    {
        private GameState gameState;
        private Dictionary<Keys, Direction> mDirectionMap = new Dictionary<Keys, Direction>();
        private IXleGameControl gameControl;
        private ITextArea textArea;
        private ISoundMan soundMan;
        private ICommandList commands;
        private IPlayerDeathHandler deathHandler;
        private IPlayerAnimator characterAnimator;

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
            this.textArea = textArea;
            this.soundMan = soundMan;
            this.characterAnimator = characterAnimator;
            this.deathHandler = deathHandler;

            input.DoCommand += (sender, args) => DoCommand(args.Command);

            mDirectionMap[Keys.Right] = Direction.East;
            mDirectionMap[Keys.Up] = Direction.North;
            mDirectionMap[Keys.Left] = Direction.West;
            mDirectionMap[Keys.Down] = Direction.South;

            mDirectionMap[Keys.OemOpenBrackets] = Direction.North;
            mDirectionMap[Keys.OemSemicolon] = Direction.West;
            mDirectionMap[Keys.OemQuotes] = Direction.East;
            mDirectionMap[Keys.OemQuestion] = Direction.South;
        }

        public void Prompt()
        {
            if (player.HP <= 0 || player.Food <= 0)
            {
                deathHandler.PlayerIsDead();
            }

            textArea.Print("\nEnter command: ");
        }

        /// <summary>
        /// Returns true if the command is a cursor movement.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private void CursorMovement(Keys cmd)
        {
            Direction dir = mDirectionMap[cmd];

            gameState.MapExtender.PlayerCursorMovement(dir);
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
        public void DoCommand(Keys cmd)
        {
            if (cmd == Keys.None)
                return;

            int waitTime = 700;

            if (IsCursorMovement(cmd))
            {
                ExecuteCursorMovement(cmd);
                return;
            }

            var command = commands.FindCommand(cmd);

            if (command != null)
            {
                commands.CurrentCommand = command;

                textArea.Print(command.Name);

                command.Execute();
            }
            else
            {
                soundMan.PlaySound(LotaSound.Invalid);

                gameControl.Wait(waitTime);
                return;
            }

            AfterDoCommand(waitTime, cmd);
        }

        private void ExecuteCursorMovement(Keys cmd)
        {
            var wasRaft = player.BoardedRaft;

            CursorMovement(cmd);

            characterAnimator.AnimateStep();

            var waitTime = gameState.MapExtender.WaitTimeAfterStep;

            if (wasRaft != player.BoardedRaft)
            {
                if (player.IsOnRaft)
                {
                    textArea.PrintLine();
                    textArea.PrintLine("You climb onto a raft.");

                    soundMan.PlaySound(LotaSound.BoardRaft);
                }

            }

            AfterDoCommand(waitTime, cmd);
        }

        public void ResetCurrentCommand()
        {
            commands.Items.Sort((x, y) => x.Name.CompareTo(y.Name));
            commands.CurrentCommand = commands.Items.Find(x => x is Pass);

            if (commands.CurrentCommand == null)
                commands.CurrentCommand = commands.Items[0];
        }

        private void AfterDoCommand(int waitTime, Keys cmd)
        {
            gameState.MapExtender.AfterExecuteCommand(cmd);

            gameControl.Wait(waitTime);

            Prompt();
        }
    }
}