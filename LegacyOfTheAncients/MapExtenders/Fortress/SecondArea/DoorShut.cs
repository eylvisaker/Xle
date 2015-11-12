﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.SecondArea
{
	public class DoorShut : EventExtender
	{
		int replacementTile = 40;

		public override bool StepOn()
		{
			for(int i = TheEvent.Rectangle.X; i < TheEvent.Rectangle.Right; i++)
			{
				Map[i, TheEvent.Rectangle.Bottom - 1] = replacementTile;
			}

			Enabled = false;

			return true;
		}
	}
}
