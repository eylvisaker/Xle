using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Towns.Stores
{
    [Transient("StoreHealder")]
    public class StoreHealer : LotaStoreFront
    {
        private bool buyingHerbs = false;

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = buyingHerbs ? XleColor.LightBlue : XleColor.Green;
            cs.FrameColor = XleColor.LightGreen;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.BorderColor = XleColor.Gray;
        }

        protected override async Task<bool> SpeakImplAsync()
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

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Make choice (hit 0 to cancel)");
            await TextArea.PrintLine();

            int choice = await QuickMenu(theList, 2, 0);

            if (choice == 0)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine("Nothing purchased");
                await TextArea.PrintLine();

                await StoreSound(LotaSound.Medium);
            }
            else if (choice == 1)
            {
                await TextArea.PrintLine("You are cured.");
                Player.HP = Player.MaxHP;

                await StoreSound(LotaSound.VeryGood);
            }
            else if (choice == 2)
            {
                if (Story.EatenJutonFruit == false)
                {
                    await TextArea.PrintLine("You're not ready yet.");
                    SoundMan.PlaySound(LotaSound.Medium);
                }
                else
                {
                    int max = Player.Gold / herbsPrice;
                    max = Math.Min(max, 40 - Player.Items[LotaItem.HealingHerb]);

                    buyingHerbs = true;

                    await TextArea.PrintLine();
                    await TextArea.PrintLine("Purchase how many healing herbs?");

                    int number = await ChooseNumber(max);

                    if (number == 0)
                    {
                        await TextArea.PrintLine("Nothing purchased.");
                        SoundMan.PlaySound(LotaSound.Medium);
                    }
                    else
                    {
                        if (Player.Spend(number * herbsPrice) == false)
                        {
                            throw new Exception("Not enough money!");
                        }

                        Player.Items[LotaItem.HealingHerb] += number;

                        await TextArea.PrintLine(number.ToString() + " healing herbs purchased.");
                        Story.PurchasedHerbs = true;

                        await StoreSound(LotaSound.Sale);
                    }
                }
            }

            await AfterSpeak();

            return true;
        }

        protected virtual async Task AfterSpeak()
        {
            if (Story.HasGuardianMark == false)
                return;

            await TextArea.PrintLine("A distant healer awaits you.", XleColor.Yellow);

            await StoreSound(LotaSound.Encounter);
        }

        private void SetOptionsText(int woundPrice, int herbsPrice)
        {
            TextWindow window = new TextWindow();
            window.Location = new Point(3, 9);

            window.Write("1. Wound Care  -  ");

            if (woundPrice <= 0)
                window.WriteLine("Not needed", XleColor.Yellow);
            else
                window.WriteLine(woundPrice + " gold");

            window.WriteLine();
            window.WriteLine();

            window.WriteLine("2. Healing Herbs -  " + herbsPrice + " apiece");

            Windows.Add(window);
        }

        private void SetDescriptionText()
        {
            TextWindow window = new TextWindow();

            window.Location = new Point(7, 3);
            window.WriteLine("Our sect offers restorative");
            window.WriteLine("    cures for your wounds.");

            Windows.Add(window);
        }
    }
}
