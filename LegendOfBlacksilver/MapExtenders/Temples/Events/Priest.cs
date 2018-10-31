using System;
using System.Linq;
using System.Threading.Tasks;
using Xle.Services.Menus;
using Xle.XleEventTypes.Extenders;

namespace Xle.LoB.MapExtenders.Temples
{
    public class Priest : EventExtender
    {
        private bool stairsOpened;

        private LobTemple Temple { get { return (LobTemple)GameState.MapExtender; } }

        public IQuickMenu QuickMenu { get; set; }
        public INumberPicker NumberPicker { get; set; }
        public LobStory Story { get { return Player.Story(); } }

        private bool IsHawkTemple { get { return Map.MapName.Contains("hawk"); } }

        private bool IsOwlTemple { get { return Map.MapName.Contains("owl"); } }

        private bool ArchivesPaid
        {
            get
            {
                if (IsHawkTemple)
                    return Story.ArchivesOpenedHawkTemmple;
                if (IsOwlTemple)
                    return Story.ArchivesOpenedOwlTemple;

                return false;
            }
            set
            {
                if (IsHawkTemple)
                    Story.ArchivesOpenedHawkTemmple = value;
                else if (IsOwlTemple)
                    Story.ArchivesOpenedOwlTemple = value;
                else
                    throw new InvalidOperationException("Cannot open archives in eagle temple.");
            }
        }

        private int ArchiveOpenCost => IsHawkTemple ? 500 : 100;

        public override async Task<bool> Speak()
        {
            var stairs = Temple.Events.OfType<TempleStairs>().FirstOrDefault();

            await TextArea.PrintLine(" to priest.\n");

            await AskForTribute();

            if (stairs != null && stairsOpened == false)
            {
                await AskOpenStairs(stairs);
            }

            return true;
        }

        private async Task AskForTribute()
        {
            if (Player.HP >= Player.MaxHP)
            {
                await TextArea.PrintLine("We're not asking for tribute now.");
                return;
            }

            await TextArea.PrintLine("Please offer tribute.");

            var choice = await NumberPicker.ChooseNumber(Player.Gold);

            var max = (int)((Player.MaxHP - Player.HP) * 0.75 + 1);

            if (choice > max)
            {
                choice = max;
                await TextArea.PrintLine("I only want " + choice + " gold.");
            }
            int hp = choice * 4 / 3;

            Player.HP += hp;

            await TextArea.PrintLine("   HP  +  " + hp);

            SoundMan.PlaySound(LotaSound.Good);
            await TextArea.FlashLinesWhile(() => SoundMan.IsAnyPlaying(), XleColor.White, XleColor.LightGreen, 50, 4);
        }

        private async Task AskOpenStairs(TempleStairs stairs)
        {
            if (ArchivesPaid)
            {
                await TextArea.PrintLine("Would you like me to open the archives?");
            }
            else
            {
                await TextArea.PrintLine("Would you like to tour the archives");
                await TextArea.PrintLine("for " + ArchiveOpenCost + " gold?");
            }

            int choice = await QuickMenu.QuickMenuYesNo();

            if (choice == 1)
                return;

            if (ArchivesPaid == false)
            {
                if (Player.Gold < ArchiveOpenCost)
                {
                    await TextArea.PrintLine("You don't have enough gold.");
                    return;
                }

                Player.Gold -= ArchiveOpenCost;
                ArchivesPaid = true;
            }

            stairsOpened = true;
            stairs.Enabled = true;

            for (int j = 0; j < stairs.Rectangle.Height; j++)
            {
                for (int i = 0; i < stairs.Rectangle.Width; i++)
                {
                    int x = i + stairs.Rectangle.X;
                    int y = j + stairs.Rectangle.Y;

                    int tile = 14 + i + 16 * j;

                    Map[x, y] = tile;
                }
            }
        }
    }
}
