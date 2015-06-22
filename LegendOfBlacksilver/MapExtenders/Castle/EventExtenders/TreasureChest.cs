﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class TreasureChest : TreasureChestExtender
    {
        protected LobStory Story { get { return GameState.Story(); } }

        public override void SetAngry(GameState state)
        {
        }

        public override void MarkChestAsOpen(GameState state)
        {
            Story.CastleChests[ChestArrayIndex][TheEvent.ChestID] = 1;
        }

        public override void OpenIfMarked(GameState state)
        {
            if (Story.CastleChests[ChestArrayIndex][TheEvent.ChestID] == 1)
            {
                TheEvent.SetOpenTilesOnMap(state.Map);
            }
        }

        public int ChestArrayIndex { get; set; }

        public override string TakeFailMessage
        {
            get
            {
                return "Nothing to take.";
            }
        }
    }
}