using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.MapLoad;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class PirateTreasure : LotaExhibit
    {
        public PirateTreasure() : base("Pirate Treasure", Coin.Topaz) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.PirateTreasure; } }

        public override bool StaticBeforeCoin
        {
            get
            {
                return false;
            }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine("Would you like to go");
            await TextArea.PrintLine("to the pirate's lair?");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
            {
                await ReadRawText(ExhibitInfo.Text[2]);

                for (int i = 0; i < 8; i++)
                {
                    await GameControl.WaitAsync(50);
                    TextArea.SetCharacterColor(2, 12 + i, XleColor.Cyan);
                }

                await GameControl.WaitForKey();

                await MapChanger.ChangeMap(2, 0);
            }
        }
    }
}
