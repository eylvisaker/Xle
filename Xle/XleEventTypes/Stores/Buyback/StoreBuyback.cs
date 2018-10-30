using AgateLib;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Menus;
using Xle.XleEventTypes.Stores.Extenders;

namespace Xle.XleEventTypes.Stores.Buyback
{
    [Transient("StoreBuyback")]
    public class StoreBuyback : StoreFront
    {
        public IBuybackFormatter BuybackFormatter { get; set; }
        public IEquipmentPicker EquipmentPicker { get; set; }
        public IBuybackOfferWindow OfferWindow { get; set; }
        public IBuybackNegotiator Negotiator { get; set; }
        public XleData Data { get; set; }

        public override bool AllowInteractionWhenLoanOverdue { get { return true; } }

        protected override void InitializeColorScheme(ColorScheme cs)
        {
            cs.BackColor = XleColor.Pink;
            cs.FrameColor = XleColor.Yellow;
            cs.FrameHighlightColor = XleColor.Yellow;
            cs.TextAreaBackColor = XleColor.Brown;
            cs.BorderColor = XleColor.Red;
        }

        protected override async Task<bool> SpeakImplAsync()
        {
            await RunStore();
            return true;
        }

        private async Task RunStore()
        {
            robbing = false;

            Screen.ClearWindows();
            Title = TheEvent.ShopName;

            var wind = new TextWindow();
            wind.Location = new Point(9, 4);

            wind.WriteLine("I will happily purchase");
            wind.WriteLine("your used arms and armor");

            var prompt = new TextWindow();

            prompt.Location = new Point(9, 9);
            prompt.WriteLine("Choose items to sell:");
            prompt.WriteLine();
            prompt.WriteLine(" 1.  Weapons");
            prompt.WriteLine(" 2.  Armor");

            Screen.AddWindow(wind);
            Screen.AddWindow(prompt);

            wind.SetColor(XleColor.Red);
            prompt.SetColor(XleColor.Red);

            await BuybackFormatter.InitialMenuPrompt();

            MenuItemList theList = new MenuItemList("0", "1", "2");
            int choice = await QuickMenu(theList, 2, 0);

            if (choice == 0)
                return;

            Screen.RemoveWindow(prompt);
            wind.Visible = false;

            ColorScheme.FrameColor = XleColor.Gray;
            ColorScheme.HorizontalLinePosition = 11;
            Title = "";
            ShowGoldText = false;

            Equipment item = null;
            TextArea.Clear();

            TextWindow questionWindow = new TextWindow { Location = new Point(5, 16) };

            Screen.AddWindow(questionWindow);

            switch (choice)
            {
                case 1:
                    questionWindow.WriteLine("What weapon will you sell me?");
                    item = await EquipmentPicker.PickWeapon(GameState, null, ColorScheme.BackColor);
                    break;

                case 2:
                    questionWindow.WriteLine("What armor will you sell me?");
                    item = await EquipmentPicker.PickArmor(GameState, null, ColorScheme.BackColor);
                    break;
            }

            if (item == null)
                return;

            Screen.RemoveWindow(questionWindow);

            wind.Visible = true;
            wind.SetColor(XleColor.White);
            wind.Location = new Point(9, 8);

            ColorScheme.HorizontalLinePosition = 14;
            ColorScheme.TextAreaBackColor = XleColor.Black;

            Title = "Buy-back shop";

            InitializeOfferWindow();

            await Negotiator.NegotiatePrice(item);
        }

        private void InitializeOfferWindow()
        {
            TextWindow offerText = new TextWindow();
            offerText.Location = new Point(2, 16);

            Screen.AddWindow(offerText);

            OfferWindow.TextWindow = offerText;
        }

    }
}
