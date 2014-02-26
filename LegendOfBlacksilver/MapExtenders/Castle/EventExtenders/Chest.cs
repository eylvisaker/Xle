using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
	class Chest : TreasureChestExtender
	{
		public override void SetAngry(GameState state)
		{
		}

		public override void MarkChestAsOpen(GameState state)
		{
			state.Story().CastleChests[ChestArrayIndex][TheEvent.ChestID] = 1;
		}

		public override void OpenIfMarked(GameState state)
		{
			if (state.Story().CastleChests[ChestArrayIndex][TheEvent.ChestID] == 1)
			{
				TheEvent.SetOpenTilesOnMap(state.Map);
			}
		}

		public int ChestArrayIndex { get; set; }
	}
}
