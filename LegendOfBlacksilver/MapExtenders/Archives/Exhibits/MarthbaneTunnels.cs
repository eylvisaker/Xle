using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;
using ERY.Xle.Services;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
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

        public override bool RequiresCoin(Player player)
        {
            if (HasBeenVisited(player))
                return false;

            return base.RequiresCoin(player);
        }

        public override void RunExhibit(Player player)
        {
            base.RunExhibit(player);

            TextArea.PrintLine("Would you like to go");
            TextArea.PrintLine("to Marthbane tunnels?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                MapChanger.ChangeMap(player, 4, 0);
            }
        }
    }
}
