﻿using AgateLib;
using System.Threading.Tasks;
using Xle.MapLoad;

namespace Xle.Blacksilver.MapExtenders.Archives.Exhibits
{
    [Transient]
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

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine("Would you like to go");
            await TextArea.PrintLine("to the island caverns now?");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenuYesNo() == 0)
            {
                await MapChanger.ChangeMap(1, 1);
            }
        }
    }
}
