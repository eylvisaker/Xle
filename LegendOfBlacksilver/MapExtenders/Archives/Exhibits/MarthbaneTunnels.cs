using System.Threading.Tasks;
using Xle.Services.MapLoad;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    public class MarthbaneTunnels : LobExhibit
    {
        public MarthbaneTunnels()
            : base("Marthbane Tunnels", Coin.Emerald)
        { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.MarthbaneTunnels; }
        }

        public override bool RequiresCoin
        {
            get
            {
                if (HasBeenVisited)
                    return false;

                return base.RequiresCoin;
            }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine("Would you like to go");
            await TextArea.PrintLine("to Marthbane tunnels?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                await MapChanger.ChangeMap(4, 0);
            }
        }
    }
}
