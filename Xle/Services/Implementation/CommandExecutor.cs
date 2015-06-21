using System;
using System.Collections.Generic;
using System.Windows.Input;

using AgateLib.InputLib;
using ERY.Xle.Rendering;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Services.Implementation
{
    public class CommandExecutor : ICommandExecutor
    {
        GameState gameState;

        Dictionary<KeyCode, Direction> mDirectionMap = new Dictionary<KeyCode, Direction>();
        private IXleGameControl gameControl;
        private ITextArea textArea;
        private ISoundMan soundMan;
        private ICommandList commands;
        private IPlayerDeathHandler deathHandler;
        private IPlayerAnimator characterAnimator;

        Player player { get { return gameState.Player; } }

        public CommandExecutor(
            GameState state,
            ICommandList commands,
            ICommandFactory factory,
            IXleGameControl gameControl,
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

            mDirectionMap[KeyCode.Right] = Direction.East;
            mDirectionMap[KeyCode.Up] = Direction.North;
            mDirectionMap[KeyCode.Left] = Direction.West;
            mDirectionMap[KeyCode.Down] = Direction.South;

            mDirectionMap[KeyCode.OpenBracket] = Direction.North;
            mDirectionMap[KeyCode.Semicolon] = Direction.West;
            mDirectionMap[KeyCode.Quotes] = Direction.East;
            mDirectionMap[KeyCode.Slash] = Direction.South;
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
        private void CursorMovement(KeyCode cmd)
        {
            Direction dir = mDirectionMap[cmd];

            gameState.MapExtender.PlayerCursorMovement(gameState, dir);
        }

        bool IsCursorMovement(KeyCode cmd)
        {
            switch (cmd)
            {
                case KeyCode.Right:
                case KeyCode.Up:
                case KeyCode.Left:
                case KeyCode.Down:
                    return true;

                default:
                    return false;
            }
        }
        public void DoCommand(KeyCode cmd)
        {
            if (cmd == KeyCode.None)
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

                command.Execute(gameState);
            }
            else
            {
                soundMan.PlaySound(LotaSound.Invalid);

                gameControl.Wait(waitTime);
                return;
            }

            AfterDoCommand(waitTime, cmd);
        }

        private void ExecuteCursorMovement(KeyCode cmd)
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

        private void AfterDoCommand(int waitTime, KeyCode cmd)
        {
            gameState.MapExtender.AfterExecuteCommand(gameState, cmd);

            gameControl.Wait(waitTime);

            Prompt();
        }
    }
}