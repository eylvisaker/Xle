﻿using ERY.Xle.Data;
using ERY.Xle.XleEventTypes.Extenders;
using System;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class SeedPlant : TreasureChestExtender
    {
        public Random Random { get; set; }

        public override void BeforeGiveItem(GameState state, ref int item, ref int count)
        {
            if (item == 8)
                count = Random.Next(3, 6);
        }

        public override bool MakesGuardsAngry
        {
            get { return false; }
        }

        public override bool Open(GameState state)
        {
            return false;
        }
        public override void PrintObtainItemMessage(GameState state, int item, int count)
        {
            TextArea.PrintLine(string.Format(
                "You take {0} {1}s.", count, Data.ItemList[item].Name));
        }
        public override void PlayObtainItemSound(GameState state, int item, int count)
        {
        }
        public override void PlayOpenChestSound()
        {
        }

        public override bool Take(GameState state)
        {
            base.Open(state);
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

        public override void MarkChestAsOpen(GameState state)
        {
            // do nothing, as this is supposed to refresh every time the map is reloaded.
        }
    }
}
