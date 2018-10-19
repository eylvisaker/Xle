using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
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

        public override bool IsClosed
        {
            get { return Story.RegisteredForTrist; }
        }

        public override void RunExhibit()
        {
            if (Player.Level < 3)
            {
                TextArea.PrintLine("You must be more advanced");
                TextArea.PrintLine("to use this exhibit.");
                TextArea.PrintLine();

                ReturnGem();
            }
            else
            {
                base.RunExhibit();

                Story.RegisteredForTrist = true;
            }
        }
    }
}
