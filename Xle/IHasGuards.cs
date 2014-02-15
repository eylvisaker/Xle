using System;
using System.Collections.Generic;
using AgateLib.Geometry;

namespace ERY.Xle
{
	public interface IHasGuards
	{
		int DefaultAttack { get; set; }
		int DefaultDefense { get; set; }
		int DefaultHP { get; set; }
		Color DefaultColor { get; set; }

		void AnimateGuards();
		bool GuardInSpot(int x, int y);
		List<Guard> Guards { get; set; }
		bool IsAngry { get; set; }

		void UpdateGuards(Player player);
		void InitializeGuardData();
	}
}
