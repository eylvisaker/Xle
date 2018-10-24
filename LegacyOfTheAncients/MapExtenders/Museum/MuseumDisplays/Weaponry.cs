using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
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

        private bool viewedThisTime;

        public override async Task RunExhibit()
        {
            if (StoryVariable == 0 && ExhibitHasBeenVisited(ExhibitIdentifier.Thornberry))
            {
                StoryVariable = 1;
            }

            if (StoryVariable == 0)
            {
                await ReadRawText(ExhibitInfo.Text[1]);

                // fair knife
                Player.AddWeapon(1, 1);
            }
            else if (StoryVariable == 1)
            {
                await ReadRawText(ExhibitInfo.Text[2]);

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
