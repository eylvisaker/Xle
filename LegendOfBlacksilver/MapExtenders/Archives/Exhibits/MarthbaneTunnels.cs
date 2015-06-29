using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.MapLoad;

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

        public override bool RequiresCoin
        {
            get
            {
                if (HasBeenVisited)
                    return false;

                return base.RequiresCoin;
            }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.PrintLine("Would you like to go");
            TextArea.PrintLine("to Marthbane tunnels?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                MapChanger.ChangeMap(4, 0);
            }
        }
    }
}
