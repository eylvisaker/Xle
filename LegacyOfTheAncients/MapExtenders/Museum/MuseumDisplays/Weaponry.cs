using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class Weaponry : LotaExhibit
    {
        public Weaponry() : base("Weaponry", Coin.Jade) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Weaponry; } }
        public override string LongName
        {
            get
            {
                return "The ancient art of weaponry";
            }
        }

        bool viewedThisTime;

        public override void RunExhibit()
        {
            if (StoryVariable == 0 && ExhibitHasBeenVisited(ExhibitIdentifier.Thornberry))
            {
                StoryVariable = 1;
            }

            if (StoryVariable == 0)
            {
                ReadRawText(ExhibitInfo.Text[1]);

                // fair knife
                Player.AddWeapon(1, 1);
            }
            else if (StoryVariable == 1)
            {
                ReadRawText(ExhibitInfo.Text[2]);

                // great bladed staff
                Player.AddWeapon(3, 3);

                StoryVariable = -1;
            }

            viewedThisTime = true;
        }

        public override bool IsClosed
        {
            get
            {
                if (viewedThisTime)
                    return true;

                if (StoryVariable < 0)
                    return true;

                return false;
            }
        }
    }

}
