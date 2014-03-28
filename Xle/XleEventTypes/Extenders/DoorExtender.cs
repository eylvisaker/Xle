﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class DoorExtender : EventExtender
	{
		public new Door TheEvent { get { return (Door)base.TheEvent; } }

		public virtual void PlayRemoveSound(ref bool handled)
		{
		}

		public virtual void RemoveDoor(GameState state, ref bool handled)
		{
		}

		public virtual void ItemUnlocksDoor(GameState state, int item, ref bool itemUnlocksDoor)
		{
		}

		public virtual void PrintUnlockText(GameState state, int item, ref bool handled)
		{
		}

		public virtual void PrintUnlockFailureText(GameState state, int item, ref bool handled)
		{
		}
	}
}
