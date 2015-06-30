using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class Information : LotaExhibit
    {
        public Information() : base("Information", Coin.None) { }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Information; } }
        public override string InsertCoinText
        {
            get { return string.Empty; }
        }


        int ExhibitState
        {
            get { return Story.Museum[0]; }
            set { Story.Museum[0] = value; }
        }
        private void SetBit(int index, bool value)
        {
            int val = 1 << index;

            if (value)
                ExhibitState |= val;
            else
                ExhibitState = ~(~ExhibitState | val);
        }

        private bool GetBit(int index)
        {
            int val = 1 << index;

            return (ExhibitState & val) != 0;
        }
        private bool[] GetBitStatus(int value)
        {
            bool[] result = new bool[32];

            for (int i = 0; i < 32; i++)
            {
                int test = 1 << i;

                result[i] = (value & test) != 0;
            }

            return result;
        }



        bool FirstUse
        {
            get { return GetBit(0); }
            set { SetBit(0, value); }
        }
        bool LostCompendiumText
        {
            get { return GetBit(1); }
            set { SetBit(1, value); }
        }
        bool SceptorCrownHint
        {
            get { return GetBit(2); }
            set { SetBit(2, value); }
        }
        public override void RunExhibit()
        {
            bool[] bits = GetBitStatus(Story.Museum[0]);
            bool doneAnything = false;

            if (FirstUse == false)
            {
                // introductory text.  it does not follow through.
                ReadRawText(ExhibitInfo.Text[2]);
                FirstUse = true;
                return;
            }

            if (bits[1] == false && Player.Items[LotaItem.Compendium] == 0)
            {
                // lost compendium
                ReadRawText(ExhibitInfo.Text[3]);

                LostCompendiumText = true;
                doneAnything = true;
            }
            doneAnything |= CheckSceptorCrown();

            doneAnything |= CheckLevelUp();

            doneAnything |= OfferIronKey();

            if (doneAnything == false)
            {
                // You must do more before I can help you again.
                ReadRawText(ExhibitInfo.Text[1]);
            }
        }

        private bool CheckSceptorCrown()
        {
            // If the player has not accepted the caretakers offer and
            // received the iron key, skip any crown/sceptor checks.
            if (Player.Items[LotaItem.IronKey] == 0)
                return false;

            if (Player.Items[LotaItem.Scepter] > 0 && Player.Items[LotaItem.Crown] > 0)
            {
                // found scepter and crown
                ReadRawText(ExhibitInfo.Text[10]);

                // give magic ice.
                Player.Items[LotaItem.MagicIce] = 1;

                // remove sceptor and crown from inventory
                Player.Items[LotaItem.Scepter] = 0;
                Player.Items[LotaItem.Crown] = 0;

                return true;
            }
            else if (SceptorCrownHint == false)
            {
                if (Player.Items[LotaItem.Scepter] > 0)
                {
                    // found scepter, give hint about crown
                    ReadRawText(ExhibitInfo.Text[7]);

                    SceptorCrownHint = true;
                    return true;
                }
                else if (Player.Items[LotaItem.Crown] > 0)
                {
                    // found crown, give hint about sceptor
                    ReadRawText(ExhibitInfo.Text[8]);

                    SceptorCrownHint = true;
                    return true;
                }
            }

            return false;
        }

        private bool OfferIronKey()
        {
            if (Player.Level >= 3 && Player.Items[LotaItem.IronKey] == 0)
            {
                ReadRawText(ExhibitInfo.Text[5]);

                TextArea.PrintLine();
                TextArea.PrintLine("Do you accept the");
                TextArea.PrintLine("Caretaker's offer?");

                if (QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
                {
                    ReadRawText(ExhibitInfo.Text[6]);

                    Player.Items[LotaItem.IronKey] = 1;
                }

                return true;
            }
            else
                return false;
        }

        private bool CheckLevelUp()
        {
            if (ShouldLevelUp())
            {
                ReadRawText(ExhibitInfo.Text[0]);
                int newLevel = TargetLevel;

                TextArea.PrintLine();
                TextArea.PrintLine("Your level is now " + newLevel.ToString() + "!!");

                SoundMan.PlaySoundSync(LotaSound.VeryGood);

                Player.Level = newLevel;

                return true;
            }
            else
                return false;
        }

        public bool ShouldLevelUp()
        {
            if (TargetLevel > Player.Level)
                return true;
            else
                return false;
        }

        private int TargetLevel
        {
            get
            {
                // check if we have compendium and guard jewels
                if (Player.Items[LotaItem.Compendium] > 0 &&
                    Player.Items[LotaItem.GuardJewel] >= 4)
                {
                    return 10;
                }

                // check if we've gotten the four jewels
                if (Player.Items[LotaItem.GuardJewel] >= 4)
                    return 7;

                // check if we've found the leader of the guardians
                if (Story.FoundGuardianLeader == true)
                    return 6;

                int jadeExhibits = CountExhibits(2, 4);
                int topazExhibits = CountExhibits(5, 7);
                int amethystExhibits = CountExhibits(8, 9);
                int sapphireExhibits = CountExhibits(10, 11);

                // check if we've viewed both sapphire visits 
                // (thus having completed the first two dungeons
                // TODO - this won't work if the player dies in Armak!)
                // and check if we've returned the crown and sceptor
                // and received the magic ice.
                if (sapphireExhibits == 2 &&
                    Player.Items[LotaItem.MagicIce] > 0)
                {
                    return 5;
                }

                // check if we've viewed both amethyst exhibits and opened
                // the lost displays.
                if (amethystExhibits == 2 && sapphireExhibits == 1)
                    return 4;

                // check if we've viewed all jade and topaz exhibits and been 
                // to the pirate's lair
                if (Story.BeenInDungeon &&
                    jadeExhibits == 3 &&
                    topazExhibits == 3)
                {
                    return 3;
                }

                // check that we've seen all the jade coin exhibits and we've closed
                // down the weaponry exhibit.
                if (jadeExhibits == 3 && Story.Museum[2] < 0)
                    return 2;

                // geez, they've done nothing.
                return 1;
            }
        }

        private int CountExhibits(int start, int finish)
        {
            int ex = 0;

            for (int i = start; i <= finish; i++)
                ex += Story.Museum[i] != 0 ? 1 : 0;

            return ex;
        }
    }
}
