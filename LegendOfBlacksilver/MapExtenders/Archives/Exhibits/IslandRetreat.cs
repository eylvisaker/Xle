using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.MapLoad;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class IslandRetreat : LobExhibit
    {
        public IslandRetreat() : base("Island Retreat", Coin.BlueGem) { }

        public IMapChanger MapChanger { get; set; }

        public override string LongName
        {
            get
            {
                return "An island retreat";
            }
        }
        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.IslandRetreat; }
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
            TextArea.PrintLine("to the island caverns now?");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenuYesNo() == 0)
            {
                MapChanger.ChangeMap(Player, 1, 1);
            }
        }
    }
}
