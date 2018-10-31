using System;
using System.Threading.Tasks;
using Xle.Services.Game;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class TheWealthy : LobExhibit
    {
        public TheWealthy()
            : base("The Wealthy", Coin.RedGarnet)
        { }

        public Random Random { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.TheWealthy; }
        }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want some gold?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                int amount = (int)(400 + Math.Pow(Player.Level, 1.35) + Random.Next(100));

                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("Gold + " + amount);
                await TextArea.PrintLine();
                await TextArea.PrintLine();

                Player.Gold += amount;

                await GameControl.PlaySoundSync(LotaSound.VeryGood);
            }
        }
    }
}
