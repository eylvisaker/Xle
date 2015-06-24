using AgateLib.Geometry;

using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
    public class StoreHealer : LotaStoreFront
    {
        bool buyingHerbs = false;

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = buyingHerbs ? XleColor.LightBlue : XleColor.Green;
            cs.FrameColor = XleColor.LightGreen;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.Gray;
        }

        protected override bool SpeakImpl(GameState state)
        {
            buyingHerbs = false;

            int woundPrice = (int)((Player.MaxHP - Player.HP) * 0.75);
            int herbsPrice = (int)(Player.Level * 300 * TheEvent.CostFactor);

            Windows.Clear();

            Title = TheEvent.ShopName;

            SetDescriptionText();
            SetOptionsText(woundPrice, herbsPrice);

            var museum = Story.Museum;

            // display ready message
            if (Story.EatenJutonFruit && Story.PurchasedHerbs == false)
            {
                TextWindow wind = new TextWindow();
                wind.Location = new Point(3, 15);

                wind.WriteLine("You're ready for herbs!", XleColor.Blue);

                StoreSound(LotaSound.VeryGood);
            }

            MenuItemList theList = new MenuItemList("0", "1", "2");

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("Make choice (hit 0 to cancel)");
            TextArea.PrintLine();

            int choice = QuickMenu(theList, 2, 0);

            if (choice == 0)
            {
                TextArea.PrintLine();
                TextArea.PrintLine("Nothing purchased");
                TextArea.PrintLine();

                StoreSound(LotaSound.Medium);
            }
            else if (choice == 1)
            {
                TextArea.PrintLine("You are cured.");
                Player.HP = Player.MaxHP;

                StoreSound(LotaSound.VeryGood);
            }
            else if (choice == 2)
            {
                if (Story.EatenJutonFruit == false)
                {
                    TextArea.PrintLine("You're not ready yet.");
                    SoundMan.PlaySound(LotaSound.Medium);
                }
                else
                {
                    int max = Player.Gold / herbsPrice;
                    max = Math.Min(max, 40 - Player.Items[LotaItem.HealingHerb]);

                    buyingHerbs = true;

                    TextArea.PrintLine();
                    TextArea.PrintLine("Purchase how many healing herbs?");

                    int number = ChooseNumber(max);

                    if (number == 0)
                    {
                        TextArea.PrintLine("Nothing purchased.");
                        SoundMan.PlaySound(LotaSound.Medium);
                    }
                    else
                    {
                        if (Player.Spend(number * herbsPrice) == false)
                        {
                            throw new Exception("Not enough money!");
                        }

                        Player.Items[LotaItem.HealingHerb] += number;

                        TextArea.PrintLine(number.ToString() + " healing herbs purchased.");
                        Story.PurchasedHerbs = true;

                        StoreSound(LotaSound.Sale);
                    }
                }
            }

            AfterSpeak(state);

            return true;
        }

        protected virtual void AfterSpeak(GameState state)
        {
            if (Story.HasGuardianMark == false)
                return;

            TextArea.PrintLine("A distant healer awaits you.", XleColor.Yellow);

            SoundMan.PlaySoundSync(LotaSound.Encounter);
        }

        private void SetOptionsText(int woundPrice, int herbsPrice)
        {
            TextWindow window = new TextWindow();
            window.Location = new Point(3, 9);

            window.Write("1. Wound Care  -  ");

            if (woundPrice <= 0)
                window.WriteLine("Not needed", XleColor.Yellow);
            else
                window.WriteLine(woundPrice.ToString() + " gold");

            window.WriteLine();
            window.WriteLine();

            window.WriteLine("2. Healing Herbs -  " + herbsPrice.ToString() + " apiece");

            Windows.Add(window);
        }

        private void SetDescriptionText()
        {
            TextWindow window = new TextWindow();

            window.Location = new AgateLib.Geometry.Point(7, 3);
            window.WriteLine("Our sect offers restorative");
            window.WriteLine("    cures for your wounds.");

            Windows.Add(window);
        }
    }
}
