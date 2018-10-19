using AgateLib.Mathematics.Geometry;
using Xle.Services.Menus;
using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Temples
{
    public class Priest : EventExtender
    {
        bool stairsOpened;

        LobTemple Temple { get { return (LobTemple)GameState.MapExtender; } }

        public IQuickMenu QuickMenu { get; set; }
        public INumberPicker NumberPicker { get; set; }
        public LobStory Story { get { return Player.Story(); } }

        bool IsHawkTemple { get { return Map.MapName.Contains("hawk"); } }
        bool IsOwlTemple { get { return Map.MapName.Contains("owl"); } }

        bool ArchivesPaid
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

        int ArchiveOpenCost => IsHawkTemple ? 500 : 100;

        public override bool Speak()
        {
            var stairs = Temple.Events.OfType<TempleStairs>().FirstOrDefault();

            TextArea.PrintLine(" to priest.\n");

            AskForTribute();

            if (stairs != null && stairsOpened == false)
            {
                AskOpenStairs(stairs);
            }

            return true;
        }

        private void AskForTribute()
        {
            if (Player.HP >= Player.MaxHP)
            {
                TextArea.PrintLine("We're not asking for tribute now.");
                return;
            }

            TextArea.PrintLine("Please offer tribute.");

            var choice = NumberPicker.ChooseNumber(Player.Gold);

            var max = (int)((Player.MaxHP - Player.HP) * 0.75 + 1);

            if (choice > max)
            {
                choice = max;
                TextArea.PrintLine("I only want " + choice + " gold.");
            }
            int hp = choice * 4 / 3;

            Player.HP += hp;

            TextArea.PrintLine("   HP  +  " + hp);

            SoundMan.PlaySound(LotaSound.Good);
            TextArea.FlashLinesWhile(() => SoundMan.IsAnyPlaying(), XleColor.White, XleColor.LightGreen, 50, 4);
        }

        private void AskOpenStairs(TempleStairs stairs)
        {
            if (ArchivesPaid)
            {
                TextArea.PrintLine("Would you like me to open the archives?");
            }
            else
            {
                TextArea.PrintLine("Would you like to tour the archives");
                TextArea.PrintLine("for " + ArchiveOpenCost + " gold?");
            }

            int choice = QuickMenu.QuickMenuYesNo();

            if (choice == 1)
                return;

            if (ArchivesPaid == false)
            {
                if (Player.Gold < ArchiveOpenCost)
                {
                    TextArea.PrintLine("You don't have enough gold.");
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
