using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes
{
	public enum EncounterState
	{
		NoEncounter,
		/// <summary>
		/// 
		/// </summary>
		JustDisengaged,
		UnknownCreatureApproaching,
		/// <summary>
		/// creature is appearing
		/// </summary>
		CreatureAppearing,

		/// <summary>
		/// Avoided monster.
		/// </summary>
		MonsterAvoided,

		/// <summary>
		/// monster has appeared
		/// </summary>
		MonsterAppeared,
		/// <summary>
		/// appeared and ready
		/// monster is ready
		/// </summary>
		MonsterReady,
	}
}
