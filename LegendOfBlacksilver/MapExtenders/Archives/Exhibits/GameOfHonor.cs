using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class GameOfHonor : LobExhibit
    {
        public GameOfHonor()
            : base("Game Of Honor", Coin.RedGarnet)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.GameOfHonor; }
        }

        public override bool IsClosed(Player player)
        {
            return Story.RegisteredForTrist;
        }
        public override void RunExhibit(Player unused)
        {
            if (Player.Level < 3)
            {
                TextArea.PrintLine("You must be more advanced");
                TextArea.PrintLine("to use this exhibit.");
                TextArea.PrintLine();

                ReturnGem(Player);
            }
            else
            {
                base.RunExhibit(Player);

                Story.RegisteredForTrist = true;
            }
        }
    }
}
