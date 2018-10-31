using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
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

        public override async Task RunExhibit()
        {
            if (Player.Level < 3)
            {
                await TextArea.PrintLine("You must be more advanced");
                await TextArea.PrintLine("to use this exhibit.");
                await TextArea.PrintLine();

                await ReturnGem();
            }
            else
            {
                await base.RunExhibit();

                Story.RegisteredForTrist = true;
            }
        }
    }
}
