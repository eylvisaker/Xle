using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class SeedPlant : TreasureChestExtender
    {
        public override void BeforeGiveItem(GameState state, ref int item, ref int count)
        {
            if (item == 8)
                count = XleCore.random.Next(3, 6);
        }

        public override bool MakesGuardsAngry
        {
            get { return false; }
        }

        public override void PrintObtainItemMessage(GameState state, int item, int count)
        {
            XleCore.TextArea.PrintLine("You take " + count.ToString() + " " + XleCore.Data.ItemList[item].Name + "s.");
        }
        public override void PlayObtainItemSound(GameState state, int item, int count)
        {
        }
        public override void PlayOpenChestSound()
        {
        }

        public override bool Take(GameState state)
        {
            Open(state);
            return true;
        }

        protected override void UpdateCommand()
        {
            XleCore.TextArea.PrintLine();
        }

        public override string AlreadyOpenMessage
        {
            get
            {
                return "Nothing to take.";
            }
        }

        public override void MarkChestAsOpen(GameState state)
        {
            // do nothing, as this is supposed to refresh every time the map is reloaded.
        }
    }
}
