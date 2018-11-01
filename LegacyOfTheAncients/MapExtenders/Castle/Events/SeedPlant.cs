using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    [Transient("SeedPlant")]
    public class SeedPlant : TreasureChestExtender
    {
        public Random Random { get; set; }

        public override void BeforeGiveItem(ref int item, ref int count)
        {
            if (item == 8)
                count = Random.Next(3, 6);
        }

        public override bool MakesGuardsAngry
        {
            get { return false; }
        }

        public override Task<bool> Open()
        {
            return Task.FromResult(false);
        }

        public override async Task PrintObtainItemMessage(int item, int count)
        {
            await TextArea.PrintLine(string.Format(
                "You take {0} {1}s.", count, Data.ItemList[item].Name));
        }

        public override void PlayObtainItemSound(int item, int count)
        {
        }
        public override void PlayOpenChestSound()
        {
        }

        public override async Task<bool> Take()
        {
            await base.Open();
            return true;
        }

        protected override void UpdateCommand()
        {
            TextArea.PrintLine();
        }

        public override string AlreadyOpenMessage
        {
            get
            {
                return "Nothing to take.";
            }
        }

        public override void MarkChestAsOpen()
        {
            // do nothing, as this is supposed to refresh every time the map is reloaded.
        }
    }
}
