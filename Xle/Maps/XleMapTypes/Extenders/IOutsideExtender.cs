using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public interface IOutsideExtender : IMapExtender
	{
		void ModifyTerrainInfo(TerrainInfo info, TerrainType terrain);
		void UpdateEncounterState(GameState state, ref bool handled);

	}
}
