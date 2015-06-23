﻿namespace ERY.Xle.Services.Implementation.Commands
{
    public class Open : Command
    {
        private IXleGameControl gameControl;

        public Open(IXleGameControl gameControl)
        {
            this.gameControl = gameControl;
        }

        public override void Execute()
        {
            if (GameState.MapExtender.PlayerOpen(GameState) == false)
            {
                TextArea.PrintLine("\n\nNothing opens.");

                gameControl.Wait(500);
            }
        }
    }
}
